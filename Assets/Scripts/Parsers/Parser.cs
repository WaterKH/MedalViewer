using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using MedalViewer.Medal;

public class Parser {

    public static Dictionary<int, float> tierMultiplier = new Dictionary<int, float>()
    {
        { 1, 0.25f }, { 2, 0.50f }, { 3, 1.00f }, { 4, 1.30f },
        { 5, 1.50f }, { 6, 1.80f }, { 7, 2.00f }, { 8, 2.30f }
    };

    public float ParseGuilt(Medal medal)
	{
		float guiltFloat = 0.0f;

	    try
	    {   
	        if (medal.Star == 6 || medal.Star == 7)
	        {
                //if (medal.GuiltMultiplier.Length != 0)
                //{

                //    if (medal.GuiltMultiplier.Split('-').Length == 1)
                //    {
                //        if (medal.GuiltMultiplier.Substring(1).Length > 1)
                //        {
                //            guilt_float = float.Parse(float.Parse(medal.GuiltMultiplier.Substring(1)).ToString("0.00"));
                //        }
                //    }
                //    else
                //    {
                //        guilt_float = float.Parse(float.Parse(medal.GuiltMultiplier.Split('-')[1]).ToString("0.00"));
                //    }
                //}

                guiltFloat = medal.GuiltMultiplierHigh != "" ? float.Parse(medal.GuiltMultiplierHigh) : medal.GuiltMultiplierLow != "" ? float.Parse(medal.GuiltMultiplierLow) : 0.0f;
	        }
	    }
	    catch (Exception e)
	    {
            Console.WriteLine("Potentially no guilt");
	        Console.WriteLine(e);
	    }

	    return guiltFloat;
	}

    public float ParseGuilt(string guiltFloat)
    {
        float guilt_float = 0.0f;

        try
        {
            if (guiltFloat.Length != 0)
            {

                if (guiltFloat.Split('-').Length == 1)
                {
                    if (guiltFloat.Substring(1).Length > 1)
                    {
                        guilt_float = float.Parse(float.Parse(guiltFloat.Substring(1)).ToString("0.00"));
                    }
                }
                else
                {
                    guilt_float = float.Parse(float.Parse(guiltFloat.Split('-')[1]).ToString("0.00"));
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Potentially no guilt");
            Console.WriteLine(e);
        }

        return guilt_float;
    }

    public Dictionary<int, MedalAbility> ParseAbilityDescription(TextAsset MedalData)
	{
	    return null;
        //var holder = new Dictionary<int, MedalAbility>();

        //var lines = MedalData.text.Split('\n');//File.ReadAllLines("KHUx_Truncated.txt");
        //var splitAnd = new string[] { " & " };
        //var splitColon = new string[] { ": " };

        //foreach (var line in lines)
        //{
        //    var parts = line.Split(',');
        //    var id = int.Parse(parts[0].Split(':')[0]);

        //    var subParts = parts[1].Split(splitAnd, StringSplitOptions.None);
        //    var medalAbility = new MedalAbility();
        //    //			Debug.Log(id);
        //    medalAbility.ID = id;
        //    //medalAbility.Name = parts[0].Split(':')[1];
        //    //Debug.Log("NAME: " + parts[0]);
        //    foreach (var subPart in subParts)
        //    {
        //        var keyValues = subPart.Split(splitColon, StringSplitOptions.None);
        //        var key = keyValues[0];
        //        switch (key)
        //        {
        //            case "STR":
        //                var str_values = keyValues[1].Split(';');

        //                if (str_values[1].Contains("-"))
        //                {
        //                    var attrs = str_values[1].Split('-');
        //                    var tiers = str_values[2].Split('-');

        //                    for (int i = 0; i < attrs.Length; ++i)
        //                    {
        //                        var attr = attrs[i];
        //                        var tier = tiers[i % tiers.Length];

        //                        medalAbility.STR.Add(new MedalCombatAbility(str_values[0], attr, tier, str_values[3]));
        //                    }
        //                }
        //                else
        //                {
        //                    medalAbility.STR.Add(new MedalCombatAbility(keyValues[1].Split(';')));
        //                }
        //                break;
        //            case "DEF":
        //                var def_values = keyValues[1].Split(';');

        //                if (def_values[1].Contains("-"))
        //                {
        //                    var attrs = def_values[1].Split('-');
        //                    var tiers = def_values[2].Split('-');

        //                    for (int i = 0; i < attrs.Length; ++i)
        //                    {
        //                        var attr = attrs[i];
        //                        var tier = tiers[i % tiers.Length];

        //                        medalAbility.DEF.Add(new MedalCombatAbility(def_values[0], attr, tier, def_values[3]));
        //                    }
        //                }
        //                else
        //                {
        //                    medalAbility.DEF.Add(new MedalCombatAbility(keyValues[1].Split(';')));
        //                }
        //                break;
        //            case "INFL":
        //                medalAbility.INFL = keyValues[1];
        //                break;
        //            case "HEAL":
        //                medalAbility.HEAL = keyValues[1];
        //                break;
        //            case "GAUGE":
        //                medalAbility.GAUGE = keyValues[1];
        //                break;
        //            case "ESUNA":
        //                medalAbility.ESUNA = true;
        //                break;
        //            case "COUNT":
        //                medalAbility.COUNT = keyValues[1];
        //                break;
        //            case "SPBONUS":
        //                medalAbility.SPBONUS = keyValues[1];
        //                break;
        //            case "DAMAGE+":
        //                medalAbility.DAMAGE = keyValues[1];
        //                break;
        //            case "NEXTMEDAL":
        //                medalAbility.NEXTMEDAL = keyValues[1];
        //                break;
        //            default:
        //                Debug.Log("NAME: " + parts[0]);
        //                Debug.Log("NOT LISTED: " + subPart);
        //                break;
        //        }
        //    }
        //    if (!holder.ContainsKey(id))
        //        holder.Add(id, medalAbility);
        //}

        //return holder;
    }

    public static float[] ParseMultiplier(string baseMultiplier)
    {
        var mult = baseMultiplier.Split('-');
        var parsedMult = new float[2];

        for (int i = 0; i < mult.Length; ++i)
        {
            float.TryParse(mult[i].Replace("x", ""), out parsedMult[i]);
        }

        return parsedMult;
    }

    public static string ParseMultiplier(string multiplier, float spATKBonus, int tier)
    {
        var mult = multiplier.Split('-');
        var parsedMult = new float[2];

        for (int i = 0; i < mult.Length; ++i)
        {
            float.TryParse(mult[i].Replace("x", ""), out parsedMult[i]);
        }

        //Debug.Log(parsedMult[0] + " " + tierMultiplier[tier] + " " + (spATKBonus / 100.0f) + " " + parsedMult[0] * (tierMultiplier[tier] + (spATKBonus / 100.0f)));

        return $"x{Math.Round(parsedMult[0] * (1.0f + tierMultiplier[tier] + (spATKBonus / 100.0f)), 2)}" +
            $"{(parsedMult[1] != 0.0f ? $" ~ {Math.Round(parsedMult[1] * (1.0f + tierMultiplier[tier] + (spATKBonus / 100)), 2)}" : string.Empty)}";
    }

    public static string ParseMultiplierWithStrength(string baseMultiplier, int maxStrength)
    {
        var mult = baseMultiplier.Split('-');

        if(mult.Length == 1)
        {
            mult = baseMultiplier.Split('~');
        }

        var parsedMult = new float[2];

        for (int i = 0; i < mult.Length; ++i)
        {
            float.TryParse(mult[i].Replace("x", ""), out parsedMult[i]);
        }

        return $"{Math.Round(parsedMult[0] * maxStrength)}{(parsedMult[1] != 0.0f ? $" ~ {Math.Round(parsedMult[1] * maxStrength)}" : string.Empty)}";;
    }
}
