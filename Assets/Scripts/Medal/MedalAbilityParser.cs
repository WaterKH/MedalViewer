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
    private Coroutine lastRoutine = null;

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

    private static readonly Regex RaisedBasedRegex = new Regex(@"(Raises) (?:(P|S|M)(?:\w+)?-?(?:based )?(strength|defense|STR|DEF)?(?: and | & ))?(?:(P|S|M)(?:\w+)?-?(?:based )?)?(strength|defense|STR|DEF)( of all attributes)?(?: by)? (\d+) tiers? for (\d+ turns?|attacks?)");
    private static readonly Regex LowerBasedRegex = new Regex(@"(Lowers) (target's )?(?:(P|S|M)(?:\w+)?-based )?(strength|defense|STR|DEF) (of all targets )?by (\d+) tiers? for (\d+ turns?|attacks?)");

    private static readonly Regex RaiseLowerRegex = new Regex(@"(\d+ \w+): (.*)");
    // This is not used in the enumerations
    private static readonly Regex SubRaiseLowerRegex = new Regex(@"(target's |targets' )?(R?r?aises |L?l?owers |-\d+ |\d+ )?(target's |targets' )?(?:(\w+)?-?(STR|DEF|strength|defense)?(?: by (\d+))?(?: and | & ))?(\w+)?\s?-?(STR|DEF|strength|defense)(?: by)?\s?(\d+)?");
    private static readonly Regex StrDefRegex = new Regex(@"(?:(\w)-Medal )?(STR|DEF) \+(\d+)");

    #endregion

    #region INFLICTS/ DAMAGE+/ MORE DAMAGE

    //private static readonly Regex InflictFixedRegex = new Regex(@"Inflicts (?:a )?(?:F|f)ixed(?:.* (defense))?");
    private static readonly Regex InflictFixedRegex = new Regex(@"(?:Inflicts (?:a )?)?((?:F|f)ixed)(?:.* (defense))?");
    private static readonly Regex InflixtRegex = new Regex(@"(?:Inflicts )?(?:(?:M|m)ore )?damage (?:the |if the |in |when |with |to )(.*)");
    //private static readonly Regex InflictStatusRegex = new Regex(@"Inflicts more damage (?:.*?)+ (paralyzed|poisoned|sleeping)");//|slot \d+|\d+ enemy)");
    //private static readonly Regex InflictTheComparisonRegex = new Regex(@"(?:Inflicts )?(?:M|m)ore damage the (.*)+");
    //private static readonly Regex InflictMiscRegex = new Regex(@"(?:Inflicts )?(?:(?:M|m)ore )?damage (with 1 enemy left|in slot (\d+)|in exchange for (\w+))");


    //private static readonly Regex MoreDamageRegex = new Regex(@"^More damage (?:with |the ) (slot number|\d+ enemy left)");
    private static readonly Regex MorePowerfulRegex = new Regex(@"^More powerful when (critical hit)");


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
    private static readonly Regex GaugeUseRegex = new Regex(@"^Gauge (use \+\d+)");

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

    #endregion

    public static Regex[] Regexes = new Regex[]
    {
        DealsRegex, // 0
        InflictFixedRegex, InflixtRegex, // 1, 2
        MorePowerfulRegex,  // 3
        DamagePlusRegex,    // 4
        RecoverAndCureRegex, CuresRegex, HpRecoveryRegex,   // 5, 6, 7
        FillAndCureRegex, GaugeRegex, GaugeUseRegex,        // 8, 9, 10
        RemovesRegex,   // 11
        EnemyCountdownRegex, AddCountRegex, ResetCountRegex, //12, 13, 14
        CopyRegex,      // 15
        IfNoneRegex,    // 16
        NextMedalRegex, // 17
        SPAtkRegex,     // 18
        RaiseLowerRegex, RaisedBasedRegex, LowerBasedRegex,  // 19, 20, 21
        MirrorsRegex,    // 22
        UnleashesRegex,  // 23
        //StrDefRegex      // 24
    };

    public MedalAbility Parser(string abilityDescription)
    {
        //TODO Perhaps make only one MedalAbility object and init that at Start() and then just simply change the items...
        var ability = new MedalAbility();

        var parts = abilityDescription.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var item in parts)
        {
            var trimmedItem = item.Trim().Replace("↑", "Raises").Replace("↓", "Lowers");
            if (trimmedItem.Length == 0) continue;
            //Debug.Log(trimmedItem);

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
                        ability.Gauge = result.Groups[1].Value;
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
                        ParseRaiseLower(result.Groups, ability);
                        break;
                    case 20:
                        ParseRaiseBased(result.Groups, ability);
                        break;
                    case 21:
                        ParseLowerBased(result.Groups, ability);
                        break;
                    case 22: // 22 Mirrors
                        ability.Mirrors = result.Groups[1].Value;
                        break;
                    case 23:
                        ability.Deal = "1";
                        ability.IgnoreAttributes = "ignore";
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

    private void ParseRaiseBased(GroupCollection resultGroups, MedalAbility ability)
    {
        #region Vars

        var raise = resultGroups[1].Value;

        var PSMUR_1 = resultGroups[2].Value;
        var StrDef_1 = resultGroups[3].Value;
        var PSMUR_2 = resultGroups[4].Value;
        var StrDef_2 = resultGroups[5].Value;

        var allAttributes = resultGroups[6].Value;

        var amount = resultGroups[7].Value;
        var duration = resultGroups[8].Value;

        #endregion

        #region First Addition (If Not Null)

        if (PSMUR_1 != "" || StrDef_1 != "")
        {
            if (StrDef_1 == "")
                StrDef_1 = StrDef_2;
            if (PSMUR_1 == "")
                PSMUR_1 = "Normal";

            var medalCombatAbility_1 = new MedalCombatAbility()
            {
                Direction = raise,
                Attribute = PSMUR_1,
                Tier = amount,
                Duration = duration
            };

            // !! Medal Add Here !!
            AddMedal(StrDef_1, medalCombatAbility_1, ability);
        }

        #endregion

        #region Second Addition (Should always add one)

        if (PSMUR_2 == "")
            PSMUR_2 = allAttributes != "" ? "PSM" : "Normal";

        var medalCombatAbility_2 = new MedalCombatAbility()
        {
            Direction = raise,
            Attribute = PSMUR_2,
            Tier = amount,
            Duration = duration
        };

        // !! Medal Add Here !!
        AddMedal(StrDef_2, medalCombatAbility_2, ability);

        #endregion
    }

    private void ParseLowerBased(GroupCollection resultGroups, MedalAbility ability)
    {
        #region Vars

        var lower = resultGroups[1].Value;
        var target = resultGroups[2].Value ?? resultGroups[5].Value;

        var PSMUR = resultGroups[3].Value;
        var StrDef = resultGroups[4].Value;

        var amount = resultGroups[6].Value;
        var duration = resultGroups[7].Value;

        #endregion

        #region Medal Addition

        if (PSMUR == "")
            PSMUR = "Normal";

        var medalCombatAbility = new MedalCombatAbility()
        {
            Direction = lower,
            Attribute = PSMUR,
            Tier = amount,
            Duration = duration
        };

        // !! Medal Add Here !!
        AddMedal(StrDef, medalCombatAbility, ability);

        #endregion
    }

    private void ParseRaiseLower(GroupCollection resultGroups, MedalAbility ability)
    {
        var duration = resultGroups[1].Value;
        var sections = resultGroups[2].Value.Split(',');

        var directionPersist = "";
        var amountPersist = "";

        foreach (var section in sections)
        {
            var strDefResult = StrDefRegex.Match(section.Trim());

            if (strDefResult.Success)
            {
                if (strDefResult.Groups[2].Value == "STR")
                {
                    ability.StrengthIncrease.Add(strDefResult.Groups[1].Value + "-" + strDefResult.Groups[2], int.Parse(strDefResult.Groups[3].Value));
                }
                else if (strDefResult.Groups[2].Value == "DEF")
                {
                    ability.DefenseIncrease = int.Parse(strDefResult.Groups[3].Value);
                }
            }
            else
            {
                var result = SubRaiseLowerRegex.Match(section.Trim());

                if (result.Success)
                {
                    #region Vars

                    var target = result.Groups[1].Value != "" ? result.Groups[1].Value.Trim() : result.Groups[3].Value.Trim();
                    var direction = result.Groups[2].Value.Trim();

                    var PSMUR_1 = result.Groups[4].Value;
                    var StrDef_1 = result.Groups[5].Value;
                    var amount_1 = result.Groups[6].Value;

                    var PSMUR_2 = result.Groups[7].Value;
                    var StrDef_2 = result.Groups[8].Value;
                    var amount_2 = result.Groups[9].Value;

                    // If we have a number, parse it into direction and tier
                    if (directionPersist != "" && direction == "")
                    {
                        direction = directionPersist;
                    }
                    else if (direction.Length < 6 && direction != "")
                    {
                        amount_1 = direction.Replace("-", "");
                        amountPersist = amount_1;

                        if (int.Parse(direction) > 0)
                            direction = "Raises";
                        else
                            direction = "Lowers";
                    }
                    //else if(direction == "")
                    //{
                    //    direction = directionPersist;
                    //}

                    #endregion

                    #region First Medal Addition (If Not Null)

                    if (PSMUR_1 != "" || StrDef_1 != "")
                    {
                        if (StrDef_1 == "")
                            StrDef_1 = StrDef_2;

                        if (PSMUR_1 == "")
                            PSMUR_1 = "Normal";
                        else if (PSMUR_1 == "DEF" || PSMUR_1 == "STR")
                        {
                            StrDef_1 = PSMUR_1;
                            PSMUR_1 = "Normal";
                        }

                        if (amount_1 == "")
                            if (amount_2 != "")
                                amount_1 = amount_2;
                            else
                                amount_1 = amountPersist;

                        var medalCombatAbility_1 = new MedalCombatAbility()
                        {
                            Direction = direction,
                            Attribute = PSMUR_1,
                            Tier = amount_1,
                            Duration = duration
                        };

                        // !! Medal Add Here !!
                        AddMedal(StrDef_1, medalCombatAbility_1, ability);
                    }

                    #endregion

                    #region Second Medal Addition

                    if (PSMUR_2 == "")
                        PSMUR_2 = "Normal";
                    else if (PSMUR_2 == "DEF" || PSMUR_2 == "STR")
                    {
                        StrDef_2 = PSMUR_2;
                        PSMUR_2 = "Normal";
                    }

                    if (amount_2 == "")
                        amount_2 = amountPersist;

                    var medalCombatAbility_2 = new MedalCombatAbility()
                    {
                        Direction = direction,
                        Attribute = PSMUR_2,
                        Tier = amount_2,
                        Duration = duration
                    };

                    // !! Medal Add Here !!
                    AddMedal(StrDef_2, medalCombatAbility_2, ability);

                    #endregion

                    directionPersist = direction;
                    amountPersist = amount_1;
                }

                else
                {
                    var spAtkBonusResult = SPAtkRegex.Match(section.Trim());

                    if (spAtkBonusResult.Success)
                    {
                        ability.SPBonus = spAtkBonusResult.Groups[1].Value;
                    }
                }
            }
        }
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
        else if (combatAbility.Attribute == "UR")
        {
            foreach (var s in "UR")
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
