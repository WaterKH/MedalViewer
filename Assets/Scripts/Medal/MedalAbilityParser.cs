using MedalViewer.Medal;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class MedalAbilityParser : MonoBehaviour
{
    // Static singleton instance
    private static MedalAbilityParser instance;
    //private Coroutine lastRoutine = null;

    // Static singleton property
    public static MedalAbilityParser Instance
    {
        get { return instance ?? (instance = new GameObject("MedalAbilityParser").AddComponent<MedalAbilityParser>()); }
    }

    #region Parser Vars

    #region DEALS

    private static readonly Regex DealsRegex = new Regex(@"Deals (\d+|an?) (?:\w+\s?)*hits?(?: with (no attributes))?");
    private static readonly Regex UnleashesRegex = new Regex(@"Unleashes an attack with no attributes");

    #endregion

    #region BUFFS/DEBUFFS

    //private static readonly Regex RaisedBasedRegex = new Regex(@"(Raises) (?:(P|S|M)(?:\w+)?-?(?:based )?(strength|defense|STR|DEF)?(?: and | & ))?(?:(P|S|M)(?:\w+)?-?(?:based )?)?(strength|defense|STR|DEF)( of all attributes)?(?: by)? (\d+) tiers? for (\d+ turns?|attacks?)");
    //private static readonly Regex LowerBasedRegex = new Regex(@"(Lowers) (target's )?(?:(P|S|M)(?:\w+)?-based )?(strength|defense|STR|DEF) (of all targets )?by (\d+) tiers? for (\d+ turns?|attacks?)");

    //private static readonly Regex RaiseLowerRegex = new Regex(@"(\d+ \w+): (.*)");
    //// This is not used in the enumerations - used in methods
    //private static readonly Regex ThreeToAttributeRegex = new Regex(@"(target'?s'? )?(Lowers |Raises )(target'?s'? )?(\w+)(?:, | & )(\w)-(?:\w+)? & (\w)-(?:\w+) (?:by )?(\d+)");
    //private static readonly Regex SubRaiseLowerRegex = new Regex(@"(target's |targets' )?(R?r?aises |L?l?owers |-\d+ |\d+ )?(target's |targets' )?(?:(\w+)?-?(STR|DEF|strength|defense)?(?: by (\d+))?(?: and | & ))?(\w+)?\s?-?(STR|DEF|strength|defense)(?: by)?\s?(\d+)?");
    //private static readonly Regex SelfRegex = new Regex(@"self \[(.*)\]");
    //private static readonly Regex TargetRegex = new Regex(@"targets? \[(.*)\]");
    

    // Older Raise/Lower Regex
    private static readonly string RaiseLowerBased = @"(?:(Upright|Reversed?|PSM|P|S|M)(?:\w+)?)?-?(?:based)?\s?(strength|defense|STR|DEF)?( of all attributes| of all targets)?(?: (?:by )?(\d+) tiers?)?";
    private static readonly Regex RaiseLowerBasedRegex = new Regex($@"(R?r?aises|L?l?owers) (target'?s'? |enemies'? )?(?:{RaiseLowerBased}(?: and | & ))?{RaiseLowerBased}(?: for |\/)(\d+ \w+)");

    // Current Raise/Lower Regex
    private static readonly Regex GeneralRaiseLowerRegex = new Regex(@"(\d+ \w+): (.*)");

    #region Sub Regexes

    // To be used in conjunction with GeneralRaiseLower Regex - Not to be included in the list
    private static readonly Regex SelfTargetRegex = new Regex(@"(self |targets? )\[(.*?)\]");

    // To be used in conjunction with GeneralRaiseLower/Self/Target Regex - Not to be included in the list
    private static readonly string AddToRaiseLower = @"(?:(?:(Reversed? |Upright |PSM-|P-|S-|M-|R-|U-))?(STR|DEF)?(?: (?:by )?(\d+))?(?:, | & )?)?";
    private static readonly Regex AddToRaiseLowerRegex = new Regex($@"(↑|R?r?aises|↓|L?l?owers)( target'?s'?)? {AddToRaiseLower}\s?{AddToRaiseLower}\s?{AddToRaiseLower}\s?{AddToRaiseLower}");

    // To be used in conjunction with GeneralRaiseLower/Self/Target Regex - Not to be included in the list
    private static readonly string SetRaiseLower = @"(?:(PSM-|P-|S-|M-|R-|U-))?(STR|DEF)?(?:, | & )?";
    private static readonly Regex SetRaiseLowerRegex = new Regex($@"(target'?s?'? )?(-?\d+ ){SetRaiseLower}(?:(-?\d+ )?{SetRaiseLower})?(?:(-?\d+ )?{SetRaiseLower})?(?:(-?\d+ )?{SetRaiseLower})?");

    // To be used in conjunction with GeneralRaiseLower Regex - Not to be included in the list
    private static readonly Regex StrDefRegex = new Regex(@"(targets’ |target’s )?(?:(\w)-Medal )?(STR|DEF) ((?:\+|\-)\d+)");

    #endregion

    #endregion

    #region INFLICTS/ DAMAGE+/ MORE DAMAGE

    //private static readonly Regex InflictFixedRegex = new Regex(@"Inflicts (?:a )?(?:F|f)ixed(?:.* (defense))?");
    private static readonly Regex InflictFixedRegex = new Regex(@"(?:Inflicts (?:a )?)?((?:F|f)ixed)(?:.* (defense))?");
    private static readonly Regex InflixtRegex = new Regex(@"(?:Inflicts )?(?:(?:M|m)ore )?damage (?:the |if the |in |when |with |to )(.*)");
    //private static readonly Regex InflictStatusRegex = new Regex(@"Inflicts more damage (?:.*?)+ (paralyzed|poisoned|sleeping)");//|slot \d+|\d+ enemy)");
    //private static readonly Regex InflictTheComparisonRegex = new Regex(@"(?:Inflicts )?(?:M|m)ore damage the (.*)+");
    //private static readonly Regex InflictMiscRegex = new Regex(@"(?:Inflicts )?(?:(?:M|m)ore )?damage (with 1 enemy left|in slot (\d+)|in exchange for (\w+))");


    //private static readonly Regex MoreDamageRegex = new Regex(@"^More damage (?:with |the ) (slot number|\d+ enemy left)");
    //private static readonly Regex MorePowerfulRegex = new Regex(@"^More powerful when (critical hit)");
    private static readonly Regex CriticalHitRegex = new Regex(@"^Has a (\d+)% chance of being a critical attack");

    private static readonly Regex DamagePlusRegex = new Regex(@"Damage\+: (.*)");

    #endregion

    #region RECOVER/CURE

    private static readonly Regex RecoverAndCureRegex = new Regex(@"(\w+) recovers HP(?: and (cures))?");
    private static readonly Regex CuresRegex = new Regex(@"(?:C|c)ures(?: own status)? ailments(?: and (\w+) recovers HP)?");
    private static readonly Regex HpRecoveryRegex = new Regex(@"HP (?:recovery LV (\d+|MAX))");
    //private static readonly Regex HpMaxRegex = new Regex(@"^HP (MAX)");

    #endregion

    #region GAUGE

    private static readonly Regex FillAndCureRegex = new Regex(@"^(?:Fills|Restores) (\d+) gauges(?: and (cures))?");
    private static readonly Regex GaugeRegex = new Regex(@"^Gauge \+(\d+)");
    private static readonly Regex GaugeUseRegex = new Regex(@"(?:^Gauge (use \+\d+)|^(Uses all gauges))");

    #endregion

    #region DISPEL/ REMOVES

    private static readonly Regex RemovesRegex = new Regex(@"Removes (.*)?status effects\s?(.*)?");

    #endregion

    #region COUNT

    private static readonly Regex EnemyCountdownRegex = new Regex(@"^(?:Enemy )?(?:C|c)ount\w* \±?\+?(\d+|unaffected)");
    private static readonly Regex AddCountRegex = new Regex(@"^Adds (\d+) to enemy count\w*");
    private static readonly Regex ResetCountRegex = new Regex(@"^(Resets) count\w*");
    //private static readonly Regex CountRegex = new Regex(@"^Count\w* .(\d+)");

    #endregion

    #region COPY

    private static readonly Regex CopyRegex = new Regex(@"Unleashes(?:.*?)(next Medal|previous Medal|2 \w+ before| 2 \w+ after)");

    #endregion

    #region IF NONE

    private static readonly Regex IfNoneRegex = new Regex(@"If none(?:.*)(\d+ \w+): (.*)");

    #endregion

    #region NEXT MEDAL

    private static readonly Regex NextMedalRegex = new Regex(@"Next Medal:? (group attack|turns (?:Power|Magic|Speed))");

    #endregion

    #region SP ATK B

    private static readonly Regex SPAtkRegex = new Regex(@"SP (?:ATK|attack) (?:B|bonus) \+(\d+)%");

    #endregion

    #region MIRROR

    private static readonly Regex MirrorsRegex = new Regex(@"Mirrors (target's) status enhancements");

    #endregion

    #region REFLECT

    private static readonly Regex ReflectRegex = new Regex(@"Reflects (\d+)% (\w+)");

    #endregion

    #region GUARD BREAK

    private static readonly Regex GuardBreakRegex = new Regex(@"(\d+)% chance to ignore target’s Defense Boost");

    #endregion

    #endregion

    public static Regex[] Regexes = new Regex[]
    {
        DealsRegex, // 0
        InflictFixedRegex, InflixtRegex, // 1, 2
        CriticalHitRegex,  // 3
        DamagePlusRegex,    // 4
        RecoverAndCureRegex, CuresRegex, HpRecoveryRegex,   // 5, 6, 7
        FillAndCureRegex, GaugeRegex, GaugeUseRegex,        // 8, 9, 10
        RemovesRegex,   // 11
        EnemyCountdownRegex, AddCountRegex, ResetCountRegex, //12, 13, 14
        CopyRegex,      // 15
        IfNoneRegex,    // 16
        NextMedalRegex, // 17
        SPAtkRegex,     // 18
        RaiseLowerBasedRegex, GeneralRaiseLowerRegex,  // 19, 20
        MirrorsRegex,    // 21
        UnleashesRegex,  // 22
        ReflectRegex,    // 23
        GuardBreakRegex  // 24
    };

    public MedalAbility Parser(string abilityDescription)
    {
        //TODO Perhaps make only one MedalAbility object and init that at Start() and then just simply change the items...
        var ability = new MedalAbility();

        var parts = abilityDescription.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var item in parts)
        {
            var trimmedItem = item.Trim().Replace("↑", "Raises").Replace("↓", "Lowers").Replace("’", "'");
            if (trimmedItem.Length == 0) continue;
            Debug.Log(trimmedItem);

            for (int i = 0; i < Regexes.Length; ++i)
            {
                var currRegex = Regexes[i];
                var result = currRegex.Match(trimmedItem);

                if (!result.Success) continue;

                // TODO SWITCH THIS ASAP - From index to enums
                // TODO ^ Is this still what I want? ^
                switch (i)
                {
                    case 0: // 0 Deals
                        ability.Deal = result.Groups[1].Value == "an" || result.Groups[1].Value == "a" ? "1" : result.Groups[1].Value;
                        ability.IgnoreAttributes = result.Groups[2].Value;
                        break;
                    case 1: // 1 - 3 Inflicts
                        ability.Inflicts = "fixed";
                        break;
                    case 2:
                    case 3:
                        ability.Inflicts = result.Groups[1].Value;
                        break;
                    case 4: // 4 Damage +
                        ability.DamagePlus = result.Groups[1].Value;
                        break;
                    case 5: // 5 - 7 Heal/ Cure
                        ability.Heal = result.Groups[1].Value;
                        ability.Esuna = result.Groups[2].Value;
                        break;
                    case 6:
                        ability.Esuna = "cures";
                        ability.Heal = result.Groups[1].Value;
                        break;
                    case 7:
                        ability.Heal = result.Groups[1].Value;
                        break;
                    case 8: // 8 - 10 Gauges
                        ability.Gauge = result.Groups[1].Value;
                        ability.Esuna = result.Groups[2].Value;
                        break;
                    case 9:
                    case 10:
                        ability.Gauge = !string.IsNullOrEmpty(result.Groups[1].Value) ? result.Groups[1].Value : result.Groups[2].Value;
                        break;
                    case 11: // 11 Removes/ Dispel
                        var split = result.Groups[1].Value.Split(new[] { " & ", " and " }, StringSplitOptions.RemoveEmptyEntries);

                        if (split.Length == 2)
                        {
                            ability.DispelPlayer = split[0];
                            ability.DispelEnemy = split[1];
                        }
                        else
                            ability.DispelEnemy = split[0];
                        break;
                    case 12: // 12 - 14 Count
                    case 13:
                    case 14:
                        ability.Count = result.Groups[1].Value;
                        break;
                    case 15: // 15 Copy
                        ability.Copy = result.Groups[1].Value;
                        break;
                    case 16: // 16 If None
                        //ParseRaiseLower(result.Groups, ability);
                        //ability.IfNone = result.Groups[1].Value;
                        // TODO Pass result to boost/sap parser
                        break;
                    case 17: // 17 Next Medal
                        ability.NextMedal = result.Groups[1].Value;
                        break;
                    case 18: // 18 SP Bonus
                        ability.SPBonus = result.Groups[1].Value;
                        break;
                    case 19: // 19 - 21 Raise/ Lower
                        OlderParseRaiseLowerBased(result.Groups, ability);
                        break;
                    case 20:
                        ParseRaiseLower(result.Groups, ability);
                        break;
                    case 21: // 21 Mirros
                        ability.Mirrors = result.Groups[1].Value;
                        break;
                    case 22: // 22 Ignore Attributes
                        ability.Deal = "1";
                        ability.IgnoreAttributes = "ignore";
                        break;
                    case 23: // 23 Reflect
                        ability.Reflect = result.Groups[1].Value + result.Groups[2].Value;
                        break;
                    case 24: // 24 Guard Break
                        ability.GuardBreak = "GuardBreak";
                        break;
                    default:
                        Debug.Log("ERROR: " + item);
                        break;
                }
                //break;
            }
        }

        return ability;
    }

    private void OlderParseRaiseLowerBased(GroupCollection resultGroups, MedalAbility ability)
    {
        #region Vars

        var direction = resultGroups[1].Value;
        var target = !string.IsNullOrEmpty(resultGroups[1].Value) ? resultGroups[1].Value :
                     !string.IsNullOrEmpty(resultGroups[5].Value) && resultGroups[5].Value.Contains("target") ? resultGroups[5].Value :
                     !string.IsNullOrEmpty(resultGroups[9].Value) && resultGroups[9].Value.Contains("target") ? resultGroups[9].Value :
                     "";

        //var attribute1 = resultGroups[3].Value;
        //var strDef1 = resultGroups[4].Value;

        //var attribute2 = resultGroups[7].Value;
        //var strDef2 = resultGroups[8].Value;

        var allAttributes = !string.IsNullOrEmpty(resultGroups[5].Value) && resultGroups[5].Value.Contains("attribute") ? resultGroups[5].Value :
                            !string.IsNullOrEmpty(resultGroups[9].Value) && resultGroups[9].Value.Contains("attribute") ? resultGroups[9].Value :
                            "";

        var duration = resultGroups[11].Value;

        #endregion

        var index = 3;
        for(int i = 0; i < 2; ++i)
        {
            var attribute = resultGroups[index].Value;
            var strDef = resultGroups[index + 1].Value;

            if(attribute != "" || strDef != "")
            {
                var amount = !string.IsNullOrEmpty(resultGroups[index + 3].Value) ? resultGroups[index + 3].Value :
                             (index + 3) + 4 < resultGroups.Count && !string.IsNullOrEmpty(resultGroups[(index + 3) + 4].Value) ? resultGroups[(index + 3) + 4].Value : 
                             "";

                if (attribute == "")
                    attribute = "Normal";
                if (strDef == "" && (index + 1) + 4 < resultGroups.Count)
                    strDef = resultGroups[(index + 1) + 4].Value;

                var medalCombatAbility = new MedalCombatAbility()
                {
                    Direction = direction,
                    Attribute = attribute.Replace("-", ""),
                    Tier = amount,
                    Duration = duration,
                    IsPlayerAffected = string.IsNullOrEmpty(target),
                    IsEnemyAffected = !string.IsNullOrEmpty(target),
                };

                // !! Medal Add Here !!
                AddMedal(strDef, medalCombatAbility, ability);
            }
                
            index += 4;
        }
    }

    private void ParseRaiseLower(GroupCollection resultGroups, MedalAbility ability)
    {
        var duration = resultGroups[1].Value;
        var section = resultGroups[2].Value;

        #region SP Atk B

        var spAtkBonusResult = SPAtkRegex.Match(section);

        if (spAtkBonusResult.Success)
        {
            ability.SPBonus = spAtkBonusResult.Groups[1].Value;
        }

        #endregion

        #region Str Def Increase

        var strDefResult = StrDefRegex.Match(section);

        if (strDefResult.Success)
        {
            if (strDefResult.Groups[3].Value == "STR")
            {
                ability.StrUp = "STR" + strDefResult.Groups[4].Value + strDefResult.Groups[2];
            }
            else if (strDefResult.Groups[3].Value == "DEF")
            {
                ability.DefUp = "DEF" + strDefResult.Groups[4].Value;
            }
        }

        #endregion

        #region Self/ Target Regex Check

        var selfTargetResults = SelfTargetRegex.Matches(section);

        if (selfTargetResults.Count > 0)
        {
            foreach (Match selfTargetResult in selfTargetResults)
            {
                var addResult = this.AddToRaiseLowerMedal(duration, selfTargetResult.Groups[2].Value, ability, selfTargetResult.Groups[1].Value.Trim());
                if (addResult)
                    continue;

                var setResult = this.SetRaiseLowerMedal(duration, selfTargetResult.Groups[2].Value, ability, selfTargetResult.Groups[1].Value.Trim());
                if (setResult)
                    continue;
            }

            return;
        }

        #endregion

        #region Add Regex Check

        var addCheckResult = this.AddToRaiseLowerMedal(duration, section, ability);
        if (addCheckResult)
            return;

        #endregion

        #region Set Regex Check

        var setCheckResult = this.SetRaiseLowerMedal(duration, section, ability);
        if (setCheckResult)
            return;

        #endregion
    }

    public bool AddToRaiseLowerMedal(string duration, string section, MedalAbility ability, string selfTarget = "")
    {
        var addResults = AddToRaiseLowerRegex.Matches(section);

        if (addResults.Count > 0)
        {
            foreach (Match addResult in addResults)
            {
                var direction = addResult.Groups[1].Value;
                var targetBool = string.IsNullOrEmpty(selfTarget) || selfTarget.Equals("self") ? true : false;

                var index = 3;
                for (int i = 0; i < 4; ++i)
                {
                    var attribute = addResult.Groups[index].Value;
                    var strDef = addResult.Groups[index + 1].Value;

                    if (attribute != "" || strDef != "")
                    {
                        var amount = !string.IsNullOrEmpty(addResult.Groups[index + 2].Value) 
                                         ? addResult.Groups[index + 2].Value :
                                     (index + 2) + 3 < addResult.Groups.Count && !string.IsNullOrEmpty(addResult.Groups[(index + 2) + 3].Value) 
                                         ? addResult.Groups[(index + 2) + 3].Value :
                                     (index + 2) + 6 < addResult.Groups.Count && !string.IsNullOrEmpty(addResult.Groups[(index + 2) + 6].Value) 
                                         ? addResult.Groups[(index + 2) + 6].Value :
                                     (index + 2) + 9 < addResult.Groups.Count && !string.IsNullOrEmpty(addResult.Groups[(index + 2) + 9].Value)
                                         ? addResult.Groups[(index + 2) + 9].Value :
                                     "";

                        if (attribute == "")
                            attribute = "Normal";

                        if (strDef == "" && (index + 1) + 3 < addResult.Groups.Count )
                            strDef = addResult.Groups[(index + 1) + 3].Value;
                        if (strDef == "" && (index + 1) + 6 < addResult.Groups.Count)
                            strDef = addResult.Groups[(index + 1) + 6].Value;
                        if (strDef == "" && (index + 1) + 9 < addResult.Groups.Count)
                            strDef = addResult.Groups[(index + 1) + 9].Value;

                        var medalCombatAbility = new MedalCombatAbility()
                        {
                            Direction = direction,
                            Attribute = attribute.Replace("-", ""),
                            Tier = amount,
                            Duration = duration,
                            IsPlayerAffected = targetBool,
                            IsEnemyAffected = !targetBool,
                        };

                        // !! Medal Add Here !!
                        this.AddMedal(strDef, medalCombatAbility, ability);
                    }

                    index += 3;
                }
            }

            return true;
        }

        return false;
    }

    public bool SetRaiseLowerMedal(string duration, string section, MedalAbility ability, string selfTarget = "")
    {
        var setResults = SetRaiseLowerRegex.Matches(section);

        if (setResults.Count > 0)
        {
            foreach (Match setResult in setResults)
            {
                var targetBool = string.IsNullOrEmpty(selfTarget) || selfTarget.Equals("self") ? true : false;
                int.TryParse(setResult.Groups[2].Value, out var amount);
                var direction = amount > 0 ? "Raises" : "Lowers";

                var index = 2;
                for (int i = 0; i < 4; ++i)
                {
                    if(index > 2 && !string.IsNullOrEmpty(setResult.Groups[index].Value))
                    {
                        int.TryParse(setResult.Groups[index].Value, out amount);
                        direction = amount > 0 ? "Raises" : "Lowers";
                    }

                    var attribute = setResult.Groups[index + 1].Value;
                    var strDef = setResult.Groups[index + 2].Value;

                    if (attribute != "" || strDef != "")
                    {

                        if (attribute == "")
                            attribute = "Normal";

                        if (strDef == "" && (index + 2) + 3 < setResult.Groups.Count)
                            strDef = setResult.Groups[(index + 2) + 3].Value;
                        if (strDef == "" && (index + 2) + 6 < setResult.Groups.Count)
                            strDef = setResult.Groups[(index + 2) + 6].Value;
                        if (strDef == "" && (index + 2) + 9 < setResult.Groups.Count)
                            strDef = setResult.Groups[(index + 2) + 9].Value;

                        var medalCombatAbility = new MedalCombatAbility()
                        {
                            Direction = direction,
                            Attribute = attribute.Replace("-", ""),
                            Tier = amount.ToString().Replace("-", ""),
                            Duration = duration,
                            IsPlayerAffected = targetBool,
                            IsEnemyAffected = !targetBool,
                        };

                        // !! Medal Add Here !!
                        this.AddMedal(strDef, medalCombatAbility, ability);
                    }

                    index += 3;
                }
            }

            ability.SetStrDef = "SetStrDef";

            return true;
        }

        return false;
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
                    Tier = combatAbility.Tier,
                    IsPlayerAffected = combatAbility.IsPlayerAffected,
                    IsEnemyAffected = combatAbility.IsEnemyAffected,
                });
            }
        }
        else if (combatAbility.Attribute == "UR")
        {
            foreach (var s in "UR")
            {
                medalCombatAbilities.Add(new MedalCombatAbility
                {
                    Attribute = s.ToString(),
                    Direction = combatAbility.Direction,
                    Duration = combatAbility.Duration,
                    Tier = combatAbility.Tier,
                    IsPlayerAffected = combatAbility.IsPlayerAffected,
                    IsEnemyAffected = combatAbility.IsEnemyAffected,
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
                foreach (var combat in medalCombatAbilities)
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
