using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class MedalAbilityParser
{
    #region DEALS

    private static readonly Regex DealsRegex = new Regex(@"^Deals (\d+|an?) (?:\w+\s?)*hits?(.*)");

    #endregion

    #region BUFFS/DEBUFFS

    //stars 1-6
    private static readonly Regex RaiseLowerRegex = new Regex(@" ^ (Raises|Lowers) (.*?)(?: for |\/)(\d+ \w+)");
    private static readonly Regex ByTierRegex = new Regex(@"(.*?) by (\d+) tier\w?");
    // star 7
    private static readonly Regex UpdatedRaiseLowerRegex = new Regex(@"(\d+ \w+): (.*)");
    //private static readonly Regex 

    #endregion

    #region INFLICTS/ DAMAGE+/ MORE DAMAGE

    private static readonly Regex InflictFixedRegex = new Regex(@"Inflicts (?:a )?fixed(?:.* (defense))?");
    private static readonly Regex InflictStatusRegex = new Regex(@"Inflicts more damage (?:.*?)+ (paralyzed|poisoned|sleeping");//|slot \d+|\d+ enemy)");
    private static readonly Regex InflictTheComparisonRegex = new Regex(@"(?:Inflicts )?(?:M|m)ore damage the (\w+) (.*)+");
    private static readonly Regex InflictMiscRegex = new Regex(@"(?:Inflicts )?(?:(?:M|m)ore )?damage (with 1 enemy left|in slot \d+|in exchange for \w+)");
    

    //private static readonly Regex MoreDamageRegex = new Regex(@"^More damage (?:with |the ) (slot number|\d+ enemy left)");
    private static readonly Regex MorePowerfulRegex = new Regex(@"^More powerful when (critical hit)");
    

    private static readonly Regex DamagePlusRegex = new Regex(@"Damage\+: (.*)");

    #endregion
    
    #region RECOVER/CURE

    private static readonly Regex RecoverAndCureRegex = new Regex(@"(\w+) recovers HP(?: and (cures))?");
    private static readonly Regex CuresRegex = new Regex($"(?:C|c)ures(?: own status)? ailments(?: and {RecoverAndCureRegex})");
    private static readonly Regex HpRecoveryRegex = new Regex(@"HP (?:recovery LV (\d+)|(MAX))");
    //private static readonly Regex HpMaxRegex = new Regex(@"^HP (MAX)");

    #endregion

    #region GAUGE

    private static readonly Regex FillAndCureRegex = new Regex($"^(?:Fills|Restores) (\\d+) gauges(?: and ({CuresRegex}))?");
    private static readonly Regex GaugeRegex = new Regex(@"^Gauge \+(\d+)");
    private static readonly Regex GaugeUseRegex = new Regex(@"^Gauge use \+(\d+)");

    #endregion

    #region REMOVES

    private static readonly Regex RemovesRegex = new Regex(@"^Removes (.*)?status effects\s?(.*)?");

    #endregion

    #region COUNT

    private static readonly Regex EnemyCountdownRegex = new Regex(@"^(?:Enemy )?(?:C|c)ount\w* \+?(\d+|unaffected)");
    private static readonly Regex AddCountRegex = new Regex(@"^Adds (\d+) to enemy count\w*");
    private static readonly Regex ResetCountRegex = new Regex(@"^(Resets) count\w*");
    //private static readonly Regex CountRegex = new Regex(@"^Count\w* .(\d+)");

    #endregion

    #region COPY

    private static readonly Regex CopyRegex = new Regex(@"^Unleashes(?:.*?)(next|previous)");
    private static readonly Regex IfNoneRegex = new Regex($"^If none(?:.*)({RaiseLowerRegex})");

    #endregion

    #region NEXT MEDAL

    private static readonly Regex NextMedalRegex = new Regex(@"^Next Medal:? (group attack|turns (?:Power|Magic|Speed))");

    #endregion

    #region SP ATK B

    private static readonly Regex SPAtkRegex = new Regex(@"SP ATK (?:B|bonus) \+(\d+)%");

    #endregion


    public static Regex[] Regexes = new Regex[]
    {
        DealMultipleRegex, DealOneRegex,
        RaiseLowerRegex, UpdatedRaiseLowerRegex,
        InflictFixedRegex, InflictTheComparisonRegex, InflictStatusRegex,
        MoreDamageRegex, MorePowerfulRegex, DamagePlusRegex,
        RecoverAndCureRegex, CuresRegex, HpRecoveryRegex, HpMaxRegex,
        FillAndCureRegex, GaugeRegex,
        RemovesRegex,
        EnemyCountdownRegex, AddCountRegex, ResetCountRegex, CountRegex,
        CopyRegex,
        NextMedalRegex,
        SPAtkRegex
    };

    public MedalAbility Parser(string abilityDescription)
    {
        //TODO Perhaps make only one MedalAbility object and init that at Start() and then just simply change the items...
        var ability = new MedalAbility();

        var parts = abilityDescription.Split('.');

        foreach (var item in parts)
        {
            var trimmedItem = item.Trim();
            if (trimmedItem.Length == 0) continue;
            Debug.Log(trimmedItem);

            for (int i = 0; i < Regexes.Length; ++i)
            {
                var currRegex = Regexes[i];
                var result = currRegex.Match(trimmedItem);

                if (!result.Success) continue;

                // TODO SWITCH THIS ASAP - From index to enums
                switch (i)
                {
                    case 0: // 0 - 1 Deals
                    case 1:
                        ability.DEAL = result.Groups.Count == 2 ? result.Groups[1].Value : "1";
                        break;
                    case 2: // 2 - 3 Raise/ Lower
                        var tempResults = result.Groups[2].Value.Split('&');
                        var passed = false;

                        foreach (var t in tempResults)
                        {
                            passed = boostsSapsRegex.Match(t.Trim()).Success;

                            if (passed == false)
                                break;
                        }

                        if (passed)
                        {
                            foreach (var t in tempResults)
                            {
                                ParseRaiseLower(result.Groups, ability, t.Trim());
                            }
                        }
                        else
                        {
                            // TODO Account for Raises and Lowers in the same sentence
                            ParseRaiseLower(result.Groups, ability);
                        }
                        break;
                    case 3: // 
                        ParseUpdatedRaiseLower(result.Groups, ability);
                        break;
                    case 4: // 4 - 8 Inflicts
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                        ability.INFL = result.Groups.Count == 3 ? result.Groups[1].Value + " " + result.Groups[2].Value : result.Groups.Count == 2 ? result.Groups[1].Value : "fixed";
                        break;
                    case 9:  // 9 Damage+
                        ability.DAMAGE = result.Groups[1].Value;
                        break;
                    case 10: // 10 - 13 Recover/ Cures
                        ability.HEAL = result.Groups[1].Value;
                        ability.ESUNA = result.Groups[2].Success; // TODO Double check this, this may be the wrong variable
                        break;
                    case 11:
                        ability.ESUNA = result.Groups[1].Success;
                        break;
                    case 12:
                    case 13:
                        ability.HEAL = result.Groups[1].Value;
                        break;
                    case 14: // 14 - 15 Gauges
                    case 15:
                        ability.GAUGE = result.Groups[1].Value;

                        if (result.Groups.Count == 3)
                            ability.ESUNA = result.Groups[2].Success;

                        break;
                    case 16: // 16 Removes
                        ability.ENEMYESUNA = result.Groups[1].Value != ""
                            ? result.Groups[1].Success
                            : result.Groups[2].Success;

                        if (result.Groups[1].Value == "own & targets'")
                            ability.ESUNA = result.Groups[1].Success;
                        else if (result.Groups[2].Value == "from self and all targets")
                            ability.ESUNA = result.Groups[2].Success;

                        break;
                    case 17: // 17 - 20 Count
                    case 18:
                    case 19:
                    case 20: 
                        ability.COUNT = result.Groups[1].Value;
                        break;
                    case 21: // 21 Copy
                        ability.COPYDIRECTION = result.Groups[1].Value;
                        break;
                    case 22:  // 22 Next Medal
                        ability.NEXTMEDAL = result.Groups[1].Value;
                        break;
                    case 23: // 23 SP ATK B
                        ability.SPBONUS = result.Groups[1].Value;
                        break;
                    default:
                        Debug.Log("ERROR: " + item);
                        break;
                }

                break;
            }
        }

        return ability;
    }

    private Regex boostsSapsRegex =
        new Regex(
            @"(\w*?)(?:-?)\s?(strength|STR|defense|DEF)?\s?(?:&|and)?\s?(\w*)(?:-?\w*)\s?(strength|STR|defense|DEF) (of all attributes)?(?:of all targets)?\s?by (\d+) tier\w?");

    private void ParseRaiseLower(GroupCollection resultGroups, MedalAbility ability, string boostsSapsOverride = null)
    {
        var lowersRaises = resultGroups[1].Value;
        var boostsSaps = boostsSapsOverride ?? resultGroups[2].Value.Trim();
        var turnsAttacks = resultGroups[3].Value;
        
        var results = boostsSapsRegex.Match(boostsSaps).Groups;

        BoostsSapsAssignment(results, ability, lowersRaises, turnsAttacks);
    }

    private void BoostsSapsAssignment(GroupCollection results, MedalAbility ability, string lowersRaises, string turnsAttacks, string tierValue = "")
    {
        var psmValues = new List<string> { results[1].Value, results[3].Value };
        var strDefValues = new List<string> { results[2].Value, results[4].Value };

        var allAttrs = results[5].Value.Length > 0;
        var tiers = results[6].Value.Length > 0 ? results[6].Value : tierValue;

        for (int i = 0; i < 2; i++)
        {
            var psm = psmValues[i];
            var strDef = strDefValues[i];
            var medalCombatAbilities = new List<MedalCombatAbility>();

            if (psm == "" && strDef == "")
                continue;

            var medalCombatAbility = new MedalCombatAbility
            {
                Direction = lowersRaises,
                Attribute = psm == "" ? "Normal" : psm,
                Tier = tiers,
                Duration = turnsAttacks
            };
            
            if (strDef == "")
                strDef = strDefValues[1];

            AddMedal(strDef, medalCombatAbility, ability);
        }
    }

    private Regex updatedLowerRaise = new Regex(@"^(target(?:'s|s'))?\s?(-?\d+)?\s?(.*)");
    //private Regex earlierLowerRaise = new Regex(@"^(↑ |↓ )?(target(?:'s |s' ))?(.*?)(?: by)? (\d+)");
    private Regex lowerRaise = 
        new Regex(@"(↑|↓) (target(?:s'|'s))?\s?(?:(\w*)(?:-))?(Upright\s?|Reversed\s?)?(STR|DEF|strength|defense)?(?: & | and |\/|(?: by)? (\d+))?(?:, )?(?:(\w*)(?:-))?(Upright\s?|Reversed\s?)?(STR|DEF|strength|defense)?(?: & | and |\/|(?: by)? (\d+))?(?:, )?(?:(\w*)(?:-))?(Upright\s?|Reversed\s?)?(STR|DEF|strength|defense)?(?: & | and |\/|(?: by)? (\d+))?");

    //private string strDef = "strength|STR|defense|DEF";
    private Regex updatedBoostsSapsRegex =
        new Regex(
            @"(?:(\w*)(?:-)?(strength|STR|defense|DEF)?(?: & | and |\/))?(\w*)(?:-?\w*)\s?(strength|STR|defense|DEF)");

    private void ParseUpdatedRaiseLower(GroupCollection resultGroups, MedalAbility ability)
    {
        //var sections = resultGroups[2].Value.Split(',');

        var duration = resultGroups[1].Value;
        MatchCollection results = lowerRaise.Matches(resultGroups[2].Value);

        if (results.Count == 0)
        {
            var parts = resultGroups[2].Value.Trim().Split(',');
            var direction = "";
            var persistentAmount = 0;

            foreach (var p in parts)
            {
                var result = SPAtkRegex.Match(p.Trim());
                
                if (result.Success)
                {
                    ability.SPBONUS = result.Groups[1].Value;
                }
                else
                {
                    result = updatedLowerRaise.Match(p.Trim());
                    var amount = 0;

                    if (int.TryParse(result.Groups[2].Value.Trim(), out amount))
                    {
                        direction = amount > 0 ? "Raises" : "Lowers";

                        persistentAmount = Math.Abs(amount);
                    }

                    var boostResult = updatedBoostsSapsRegex.Match(result.Groups[3].Value);

                    BoostsSapsAssignment(boostResult.Groups, ability, direction, duration, persistentAmount.ToString());
                }
            }
        }
        else
        {
            
            foreach (Match result in results)
            {
                var direction = result.Groups[1].Value == "↑" ? "Raises" : "Lowers";
                var targets = result.Groups[2].Value;

                for (int i = 3; i < result.Groups.Count; i += 4)
                {
                    var amount = result.Groups[i + 3].Value;
                    var strDef = result.Groups[i + 2].Value;
                    var UR = result.Groups[i + 1].Value;
                    var attribute = result.Groups[i].Value == ""
                        ? UR == "" ? strDef == "" ? result.Groups[i].Value : "Normal" : UR
                        : result.Groups[i].Value;

                    amount = CalculateResult(amount, i + 3, result.Groups);
                    strDef = CalculateResult(strDef, i + 2, result.Groups);

                    if (amount == "" || strDef == "" || attribute == "")
                        continue;

                    var medalCombatAbility = new MedalCombatAbility()
                    {
                        Duration = duration,
                        Attribute = attribute.Trim(),
                        Direction = direction,
                        Tier = amount
                    };

                    AddMedal(strDef, medalCombatAbility, ability);
                }
            }

            var spAtk = SPAtkRegex.Match(resultGroups[2].Value);

            if(spAtk.Success)
                ability.SPBONUS = spAtk.Groups[1].Value;
        }
    }

    public string CalculateResult(string result, int index, GroupCollection groupCollection)
    {
        if (result == "")
        {
            if (index + 4 < groupCollection.Count)
            {
                if (groupCollection[index + 4].Value == "")
                {
                    if (index + 8 < groupCollection.Count)
                    {
                        result = groupCollection[index + 8].Value;
                    }
                }
                else
                {
                    result = groupCollection[index + 4].Value;
                }
            }
        }

        return result;
    }

    public void AddMedal(string strDef, MedalCombatAbility combatAbility, MedalAbility ability)
    {
        var medalCombatAbilities = new List<MedalCombatAbility>();

        if (combatAbility.Attribute == "PSM")
        {
            foreach (var s in "PSM")
            {
                medalCombatAbilities.Add(new MedalCombatAbility
                {
                    Attribute = s.ToString(),
                    Direction = combatAbility.Direction,
                    Duration = combatAbility.Duration,
                    Tier = combatAbility.Tier
                });
            }
        }
        else
        {
            medalCombatAbilities.Add(combatAbility);
        }

        switch (strDef)
        {
            case "strength":
            case "STR":
                foreach(var combat in medalCombatAbilities)
                    ability.STR.Add(combat);
                break;
            case "defense":
            case "DEF":
                foreach (var combat in medalCombatAbilities)
                    ability.DEF.Add(combat);
                break;
            default:
                Debug.Log("Something went wrong... " + strDef);
                break;
        }
    }
}
