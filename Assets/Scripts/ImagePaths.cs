using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImagePaths {

	public static Dictionary<string, Dictionary<string, Dictionary<string, string>>> CombatPaths = 
										new Dictionary<string, Dictionary<string, Dictionary<string, string>>>()
	{
		{ 
			"STR", new Dictionary<string, Dictionary<string, string>> ()
			{
				{
					"Lowers", new Dictionary<string, string> ()
					{
						{ "Normal", "Saps/DEBUFFatk"}, { "STR", "Saps/DEBUFFatk"},
                        { "Power", "Saps/DEBUFFatkP"}, { "Speed", "Saps/DEBUFFatkS"}, { "Magic", "Saps/DEBUFFatkM"},

					    { "P", "Saps/DEBUFFatkP"}, { "S", "Saps/DEBUFFatkS"}, { "M", "Saps/DEBUFFatkM"}
                    }
				},
				{
					"Raises", new Dictionary<string, string> ()
					{
						{ "Normal", "Boosts/BUFFatk"},  { "STR", "Boosts/BUFFatk"}, { "Upright", "Boosts/BUFFatkU"}, { "Reversed", "Boosts/BUFFatkR" },
						{ "Power", "Boosts/BUFFatkP"}, { "Speed", "Boosts/BUFFatkS"}, { "Magic", "Boosts/BUFFatkM"},

					    { "U", "Boosts/BUFFatkU"}, { "R", "Boosts/BUFFatkR" },
					    { "P", "Boosts/BUFFatkP"}, { "S", "Boosts/BUFFatkS"}, { "M", "Boosts/BUFFatkM"}
                    }
				}

			}
		},
		{
			"DEF", new Dictionary<string, Dictionary<string, string>> ()
			{
				{
					"Lowers", new Dictionary<string, string> ()
					{
						{ "Normal", "Saps/DEBUFFdefense"}, { "DEF", "Saps/DEBUFFdefense"}, { "Upright", "Saps/DEBUFFdefenseU"}, { "Reversed", "Saps/DEBUFFdefenseR" },
						{ "Power", "Saps/DEBUFFdefenseP"}, { "Speed", "Saps/DEBUFFdefenseS"}, { "Magic", "Saps/DEBUFFdefenseM"},

					    { "U", "Saps/DEBUFFdefenseU"}, { "R", "Saps/DEBUFFdefenseR" },
					    { "P", "Saps/DEBUFFdefenseP"}, { "S", "Saps/DEBUFFdefenseS"}, { "M", "Saps/DEBUFFdefenseM"}
                    }
				},
				{
					"Raises", new Dictionary<string, string> ()
					{
						{ "Normal", "Boosts/BUFFdefense"}, { "DEF", "Boosts/BUFFdefense"},
                        { "Power", "Boosts/BUFFdefenseP"}, { "Speed", "Boosts/BUFFdefenseS"}, { "Magic", "Boosts/BUFFdefenseM"},

					    { "P", "Boosts/BUFFdefenseP"}, { "S", "Boosts/BUFFdefenseS"}, { "M", "Boosts/BUFFdefenseM"}
                    }
				}

			}
		}
	};

	public static Dictionary<string, Dictionary<string, string>> MiscPaths = new Dictionary<string, Dictionary<string, string>>()
	{
		{
			"INFL", new Dictionary<string, string>()
			{
                { "poisoned targets", "Inflict/ATKpoison" }, { "sleeping targets", "Inflict/ATKsleep" }, { "paralyzed targets", "Inflict/ATKparal" },
                { "target is paralyzed", "Inflict/ATKparal" },

                { "higher the slot number", "Inflict/plus" }, { "higher your HP", "Inflict/ATKhealthfull" },
                
                { "more enemies defeated in that stage", "Inflict/ATKkills" }, { "more gauges used", "Inflict/ATKspmore" },  { "more gauges used this turn", "Inflict/ATKspturn" },
                { "more gauges are full", "Inflict/ATKspmore" }, { "more gauges required for the next Medal", "Inflict/ATKspmore" }, { "more gauges required for the previous Medal", "Inflict/ATKspmore" },
                { "more Lux collected in that stage", "Inflict/ATKluxplus" }, { "more SP attacks used in succession", "Inflict/ATKspsuccALT" }, { "more special attacks used in succession", "Inflict/ATKspsuccALT" },
                { "more skills triggered in that stage", "Inflict/ATKskills2" }, { "more turns have passed", "Inflict/ATKturns" },

                { "bigger your party", "Inflict/ATKparty" }, { "bigger the slot number", "Inflict/plus" },

                { "lower your HP", "Inflict/ATKhealthlow" }, { "lower the slot number", "Inflict/minus" },

                { "less Lux collected in that stage", "Inflict/ATKluxminus" },
                
                { "1 enemy left (incl raid boss parts)", "Inflict/ATKsingleboss" }, { "1 enemy left (incl raid boss)", "Inflict/ATKsingleboss" }, { "1 enemy left (incl", "Inflict/ATKsingleboss" },

                { "slot 1", "Inflict/1" }, { "slot 2", "Inflict/2" }, { "slot 3", "Inflict/3" },
                { "slot 4", "Inflict/4" }, { "slot 5", "Inflict/5" }, { "slot 6", "Inflict/6" },

                { "exchange for defense", "Inflict/ATKexchangeDefense" }, { "exchange for HP", "Inflict/ATKexchangeHP" },

                { "fixed", "Inflict/ATKfixed" },

                { "50", "Inflict/50crit" }, { "30", "Inflict/30crit" }
            }
		},
        {
            "DAMAGE+", new Dictionary<string, string>()
            {
                { "the more gauges used", "Inflict/ATKspmore" },
                { "the more gauges are full", "Inflict/ATKspmore" },
                { "1 enemy or 0 parts left", "Inflict/ATKsingleboss" },
                { "the more SP attacks used in succession", "Inflict/ATKspsuccALT" },
                { "the bigger your party", "Inflict/ATKparty" },
                { "Higher HP", "Inflict/ATKhealthfull" },
                { "Higher slot number", "Inflict/plus" },
                { "Lower slot number", "Inflict/minus" },
            }
        },
        {
            "MIRROR", new Dictionary<string, string>()
            {
                { "target's", "StatusEffect/enemymirror" }
            }
        },
        {
			"HEAL", new Dictionary<string, string>()
			{
                { "slightly", "Heal/Heal1" }, { "moderately", "Heal/Heal2" }, { "greatly", "Heal/Heal3" }, { "significantly", "Heal/Heal4" },
                { "Slightly", "Heal/Heal1" }, { "Moderately", "Heal/Heal2" }, { "Greatly", "Heal/Heal3" }, { "Significantly", "Heal/Heal4" }, { "MAX", "Heal/HealMax" },
			    { "1", "Heal/Heal1" }, { "2", "Heal/Heal2" }, { "3", "Heal/Heal3" }, { "4", "Heal/Heal4" },
            }
		},
        {
            "ESUNA", new Dictionary<string, string>()
            {
                { "cures", "StatusEffect/esuna" }
            }
        },
        {
			"GAUGE", new Dictionary<string, string>()
			{
                { "0", "Gauges/Restore/GR0" }, { "1", "Gauges/Restore/GR1" }, { "2", "Gauges/Restore/GR2" }, { "3", "Gauges/Restore/GR3" },
                { "4", "Gauges/Restore/GR4" }, { "5", "Gauges/Restore/GR5" }, { "6", "Gauges/Restore/GR6" }, { "7", "Gauges/Restore/GR7" },
                { "8", "Gauges/Restore/GR8" }, { "9", "Gauges/Restore/GR9" }, { "10", "Gauges/Restore/GR10" }, { "15", "Gauges/Restore/GR15" },
                { "30", "Gauges/Restore/GR30"},
                
                { "Uses all gauges", "Gauges/Uses/GUall" }, { "use +2", "Gauges/Uses/GU2" }, { "use +30", "Gauges/Uses/GU30" }
            }
		},
        {
            "DISPEL", new Dictionary<string, string>()
            {
                { "targets'", "StatusEffect/enemydispel" }, { "all targets", "StatusEffect/enemydispel" }, { "target's", "StatusEffect/enemydispel" }, { "of all targets", "StatusEffect/enemydispel" },
                { "own", "StatusEffect/dispel" }, { "from self", "StatusEffect/dispel" }, { "all", "StatusEffect/dispel" },
            }
        },
		{
			"COUNT", new Dictionary<string, string>()
			{
				{ "0", "Count/count0" },
                { "1", "Count/count1" }, { "2", "Count/count2" }, { "3", "Count/count3" }, { "4", "Count/count4" }, { "5", "Count/count5" },
                { "6", "Count/count6" }, { "7", "Count/count7" }, { "8", "Count/count8" }, { "9", "Count/count9" }, { "10", "Count/count10"},
                { "Resets", "Count/COUNTreset" }, { "unaffected", "Count/count0" }
			}
		},
        {
            "COPY", new Dictionary<string, string>()
            {
                { "previous Medal", "Copy/before" }, { "next Medal", "Copy/after" }, 
                { "2 Medals before", "Copy/before" }, { "2 Medals after", "Copy/after" },
            }
        },
		{
			"NEXTMEDAL", new Dictionary<string, string>()
			{
				{ "Group Attack", "NextMedal/NEXTgroup" }, { "group attack", "NextMedal/NEXTgroup" },
                { "turns Magic", "NextMedal/NEXTm" }, { "turns Power", "NextMedal/NEXTp" }, { "turns Speed", "NextMedal/NEXTs" }
            }
		},
        {
            "SPBONUS", new Dictionary<string, string>()
            {
                { "15", "SPATKBonus/15" }, { "20", "SPATKBonus/20" },
                { "30", "SPATKBonus/30" }, { "40", "SPATKBonus/40" }, { "60", "SPATKBonus/60" },
                { "70", "SPATKBonus/70" }, { "80", "SPATKBonus/80" }, { "90", "SPATKBonus/90" },
                { "100", "SPATKBonus/100" }, { "110", "SPATKBonus/110" }, { "120", "SPATKBonus/120" },
                { "130", "SPATKBonus/130" }, { "140", "SPATKBonus/140" }, { "150", "SPATKBonus/150" },
                { "160", "SPATKBonus/160" }, { "170", "SPATKBonus/170" }, { "180", "SPATKBonus/180" },
                { "190", "SPATKBonus/190" }, { "200", "SPATKBonus/200" }, { "210", "SPATKBonus/210" },
                { "230", "SPATKBonus/230" }, { "250", "SPATKBonus/250" }, { "260", "SPATKBonus/260" },
                { "280", "SPATKBonus/280" },
            }
        },
        {
            "IGNORE", new Dictionary<string, string>()
            {
                { "ignore", "Inflict/NoAtt" }
            }
        },
        {
            "REFLECT", new Dictionary<string, string>()
            {
                { "100Power", "Reflect/100Power" }, { "100Speed", "Reflect/100Speed" }, { "100Magic", "Reflect/100Magic" },
                { "30Power", "Reflect/30Power" }, { "30Speed", "Reflect/30Speed" }, { "30Magic", "Reflect/30Magic" },
                { "10Power", "Reflect/10Power" }, { "10Speed", "Reflect/10Speed" }, { "10Magic", "Reflect/10Magic" },
            }
        },
        {
            "STR_DEF+", new Dictionary<string, string>()
            {
                { "DEF+500", "StrDefPlus/def500" },{ "DEF+1000", "StrDefPlus/def1000" },{ "DEF+1500", "StrDefPlus/def1500" },{ "DEF+2000", "StrDefPlus/def2000" },
                { "STR+500", "StrDefPlus/str500" },{ "STR+1000", "StrDefPlus/str1000" },{ "STR+1500", "StrDefPlus/str1500" },{ "STR+2000", "StrDefPlus/str2000" },{ "STR+4500", "StrDefPlus/str4500" },{ "STR+5000", "StrDefPlus/str4500" },

                { "STR+500U", "StrDefPlus/str500u" },{ "STR+1000U", "StrDefPlus/str1000u" },{ "STR+1500U", "StrDefPlus/str1500u" },{ "STR+2000U", "StrDefPlus/str2000u" },{ "STR+4000U", "StrDefPlus/str4500u" },{ "STR+4500U", "StrDefPlus/str4500u" },
                { "STR+500R", "StrDefPlus/str500r" },{ "STR+1000R", "StrDefPlus/str1000r" },{ "STR+1500R", "StrDefPlus/str1500r" },{ "STR+2000R", "StrDefPlus/str2000r" },{ "STR+4000R", "StrDefPlus/str4500r" },{ "STR+4500R", "StrDefPlus/str4500r" },

                { "STR+300P", "StrDefPlus/str500p" },{ "STR+500P", "StrDefPlus/str500p" },{ "STR+1000P", "StrDefPlus/str1000p" },{ "STR+1500P", "StrDefPlus/str1500p" },{ "STR+2000P", "StrDefPlus/str2000p" },{ "STR+4500P", "StrDefPlus/str4500p" },
                { "STR+300S", "StrDefPlus/str500s" },{ "STR+500S", "StrDefPlus/str500s" },{ "STR+1000S", "StrDefPlus/str1000s" },{ "STR+1500S", "StrDefPlus/str1500s" },{ "STR+2000S", "StrDefPlus/str2000s" },{ "STR+4500S", "StrDefPlus/str4500s" },
                { "STR+300M", "StrDefPlus/str500m" },{ "STR+500M", "StrDefPlus/str500m" },{ "STR+1000M", "StrDefPlus/str1000m" },{ "STR+1500M", "StrDefPlus/str1500m" },{ "STR+2000M", "StrDefPlus/str2000m" },{ "STR+4500M", "StrDefPlus/str4500m" },

                //{ "DEF-500", "StrDefPlus/def500" },{ "DEF-1500", "StrDefPlus/def1500" },{ "DEF-2000", "StrDefPlus/def2000" },
                //{ "STR-500", "StrDefPlus/str500" },{ "STR-1500", "StrDefPlus/str1500" },{ "STR-2000", "StrDefPlus/str2000" },

                { "STR-1000U", "StrDefPlus/str1000u" },
                

                //{ "STR-500P", "StrDefPlus/str500p" },{ "STR-1500P", "StrDefPlus/str1500p" },{ "STR-2000P", "StrDefPlus/str2000p" },
                //{ "STR-500S", "StrDefPlus/str500s" },{ "STR-1500S", "StrDefPlus/str1500s" },{ "STR-2000S", "StrDefPlus/str2000s" },
                //{ "STR-500M", "StrDefPlus/str500m" },{ "STR-1500M", "StrDefPlus/str1500m" },{ "STR-2000M", "StrDefPlus/str2000m" },

            }
        },
        {
            "GUARDBREAK", new Dictionary<string, string>()
            {
                { "GuardBreak", "GuardBreak/GuardBreak" }
            }
        },
        {
            "SETSTRDEF", new Dictionary<string, string>()
            {
                { "SetStrDef", "StatusEffect/BUFFreset" }
            }
        }
    };
}
