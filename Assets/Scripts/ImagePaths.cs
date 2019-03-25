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
                { "poisoned targets", "Inflict/StatusEffect/ATKpoison" }, { "sleeping targets", "Inflict/StatusEffect/ATKsleep" }, { "paralyzed targets", "Inflict/StatusEffect/ATKparal" },
                { "target is paralyzed", "Inflict/StatusEffect/ATKparal" },

                { "higher the slot number", "Inflict/Slots/plus" }, { "higher your HP", "Inflict/HP/ATKhealthfull" },
                
                { "more enemies defeated in that stage", "Inflict/ATKkills" }, { "more gauges used", "Inflict/SP/ATKspmore" },  { "more gauges used this turn", "Inflict/SP/ATKspturn" },
                { "more gauges are full", "Inflict/SP/ATKspmore" }, { "more gauges required for the next Medal", "Inflict/SP/ATKspmore" }, { "more gauges required for the previous Medal", "Inflict/SP/ATKspmore" },
                { "more Lux collected in that stage", "Inflict/Lux/ATKluxplus" }, { "more SP attacks used in succession", "Inflict/SP/ATKspsuccALT" }, { "more special attacks used in succession", "Inflict/SP/ATKspsuccALT" },
                { "more skills triggered in that stage", "Inflict/ATKskills2" }, { "more turns have passed", "Inflict/ATKturns" },

                { "bigger your party", "Inflict/ATKparty" }, { "bigger the slot number", "Inflict/Slots/plus" },

                { "lower your HP", "Inflict/HP/ATKhealthlow" }, { "lower the slot number", "Inflict/Slots/minus" },

                { "less Lux collected in that stage", "Inflict/Lux/ATKluxminus" },
                
                { "1 enemy left (incl raid boss parts)", "Inflict/ATKsingleboss" }, { "1 enemy left (incl raid boss)", "Inflict/ATKsingleboss" },

                { "slot 1", "Inflict/Slots/1" }, { "slot 2", "Inflict/Slots/2" }, { "slot 3", "Inflict/Slots/3" },
                { "slot 4", "Inflict/Slots/4" }, { "slot 5", "Inflict/Slots/5" }, { "slot 6", "Inflict/Slots/6" },

                { "exchange for defense", "Inflict/ATKexchangeDefense" }, { "exchange for HP", "Inflict/ATKexchangeHP" },

                { "fixed", "Inflict/ATKfixed" }, { "critical hit", "Inflict/50crit" }
            }
		},
        {
            "DAMAGE+", new Dictionary<string, string>()
            {
                { "the more gauges are full", "Inflict/SP/ATKspmore" },
                { "1 enemy or 0 parts left", "Inflict/ATKsingleboss" },
                { "the more SP attacks used in succession", "Inflict/SP/ATKspsuccALT" },
                { "the bigger your party", "Inflict/ATKparty" },
                { "Higher HP", "Inflict/HP/ATKhealthfull" },
                { "Higher slot number", "Inflict/Slots/plus" },
                { "Lower slot number", "Inflict/Slots/minus" },
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
				{ "Slightly", "Heal/Heal1" }, { "Moderately", "Heal/Heal2" }, { "Greatly", "Heal/Heal3" }, { "Significantly", "Heal/Heal4" }, { "MAX", "Heal/HealMax" },
			    { "1", "Heal/Heal1" }, { "2", "Heal/Heal2" }, { "3", "Heal/Heal3" }, { "4", "Heal/Heal4" },
            }
		},
        {
            "ESUNA", new Dictionary<string, string>()
            {
                { "cures", "Heal/HealEsuna" }
            }
        },
        {
			"GAUGE", new Dictionary<string, string>()
			{
                { "Uses all gauges", "Inflict/SP/ATKspuseall" },
                { "1", "Gauges/1" }, { "2", "Gauges/2" }, { "3", "Gauges/3" }, { "4", "Gauges/4" }, { "5", "Gauges/5" },
                { "6", "Gauges/6" }, { "7", "Gauges/7" }, { "8", "Gauges/8" }, { "9", "Gauges/9" }, { "10", "Gauges/10" },
                { "30", "Gauges/10"}
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

				{ "0", "Count/COUNTunnaf" },
                { "1", "Count/1" }, { "2", "Count/2" }, { "3", "Count/3" }, { "4", "Count/4" }, { "5", "Count/5" },
                { "6", "Count/6" }, { "7", "Count/7" }, { "8", "Count/8" }, { "9", "Count/9" }, { "10", "Count/10"},
                { "Resets", "Count/COUNTreset" }, { "unaffected", "Count/COUNTunnaf" }
			}
		},
        {
            "COPY", new Dictionary<string, string>()
            {
                { "next Medal", "Copy/before" }, { "previous Medal", "Copy/after" },
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
                { "15", "Gems/Upright_Gem" }, { "20", "SPATKBonus/20" },
                { "30", "SPATKBonus/30" }, { "40", "SPATKBonus/40" }, { "60", "SPATKBonus/60" },
                { "70", "SPATKBonus/70" }, { "80", "SPATKBonus/80" }, { "90", "SPATKBonus/90" },
                { "100", "SPATKBonus/100" }, { "110", "SPATKBonus/110" }, { "120", "SPATKBonus/120" },
                { "130", "SPATKBonus/120" }, { "140", "SPATKBonus/120" }, { "150", "SPATKBonus/120" },
                { "160", "SPATKBonus/120" }, { "170", "SPATKBonus/120" }, { "180", "SPATKBonus/120" },
                { "190", "SPATKBonus/120" }, { "200", "SPATKBonus/120" }, { "210", "SPATKBonus/120" },
                { "250", "SPATKBonus/120" }, { "260", "SPATKBonus/120" },
            }
        },
    };
}
