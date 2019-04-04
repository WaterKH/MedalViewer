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
                
                { "1 enemy left (incl raid boss parts)", "Inflict/ATKsingleboss" }, { "1 enemy left (incl raid boss)", "Inflict/ATKsingleboss" },

                { "slot 1", "Inflict/1" }, { "slot 2", "Inflict/2" }, { "slot 3", "Inflict/3" },
                { "slot 4", "Inflict/4" }, { "slot 5", "Inflict/5" }, { "slot 6", "Inflict/6" },

                { "exchange for defense", "Inflict/ATKexchangeDefense" }, { "exchange for HP", "Inflict/ATKexchangeHP" },

                { "fixed", "Inflict/ATKfixed" }, { "critical hit", "Inflict/50crit" }
            }
		},
        {
            "DAMAGE+", new Dictionary<string, string>()
            {
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
                
                { "Uses all gauges", "Gauges/Uses/GUall" }, { "use +2", "Gauges/Uses/GU2" }
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
				{ "Group Attack", "NextMedal/NEXTgroup" },
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
                { "250", "SPATKBonus/250" }, { "260", "SPATKBonus/260" },
            }
        },
        {
            "IGNORE", new Dictionary<string, string>()
            {
                { "ignore", "Inflict/NoAtt" }
            }
        }
    };
}
