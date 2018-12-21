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
						{ "Normal", "Saps/S"}, { "STR", "Saps/S"},
                        { "Power", "Saps/S_P"}, { "Speed", "Saps/S_S"}, { "Magic", "Saps/S_M"},

					    { "P", "Saps/S_P"}, { "S", "Saps/S_S"}, { "M", "Saps/S_M"}
                    }
				},
				{
					"Raises", new Dictionary<string, string> ()
					{
						{ "Normal", "Boosts/S"},  { "STR", "Boosts/S"}, { "Upright", "Boosts/S_U"}, { "Reversed", "Boosts/S_R" },
						{ "Power", "Boosts/S_P"}, { "Speed", "Boosts/S_S"}, { "Magic", "Boosts/S_M"},

					    { "U", "Boosts/S_U"}, { "R", "Boosts/S_R" },
					    { "P", "Boosts/S_P"}, { "S", "Boosts/S_S"}, { "M", "Boosts/S_M"}
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
						{ "Normal", "Saps/D"}, { "DEF", "Saps/D"}, { "Upright", "Saps/D_U"}, { "Reversed", "Saps/D_R" },
						{ "Power", "Saps/D_P"}, { "Speed", "Saps/D_S"}, { "Magic", "Saps/D_M"},

					    { "U", "Saps/D_U"}, { "R", "Saps/D_R" },
					    { "P", "Saps/D_P"}, { "S", "Saps/D_S"}, { "M", "Saps/D_M"}
                    }
				},
				{
					"Raises", new Dictionary<string, string> ()
					{
						{ "Normal", "Boosts/D"}, { "DEF", "Boosts/D"},
                        { "Power", "Boosts/D_P"}, { "Speed", "Boosts/D_S"}, { "Magic", "Boosts/D_M"},

					    { "P", "Boosts/D_P"}, { "S", "Boosts/D_S"}, { "M", "Boosts/D_M"}
                    }
				}

			}
		}
	};

	public static Dictionary<string, Dictionary<string, string>> MiscPaths = new Dictionary<string, Dictionary<string, string>>()
	{
		{
			"INFL", new Dictionary<string, string>() // TODO Break this down into [more/less/higher/lower][attr]
			{
				{ "poisoned", "StatusEffect/Poison" }, { "sleeping", "StatusEffect/Sleep" }, { "paralyzed", "StatusEffect/Paralysis" }, { "no defense", "Gems/Upright_Gem" },

                { "more turns", "Gems/Upright_Gem" }, { "more SP", "Gems/Upright_Gem" }, { "more special", "Gems/Upright_Gem" }, { "more gauges used", "Gems/Upright_Gem" },
			    { "more skills", "Gems/Upright_Gem" },

			    { "lower the slot number", "Gems/Upright_Gem" },

			    { "higher the slot number", "Gems/Upright_Gem" },

                { "higher your HP", "Gems/Upright_Gem" }, 
				{ "defense sap", "Gems/Upright_Gem" }, { "lux", "Gems/Upright_Gem" }, { "enemy deaths", "Gems/Upright_Gem" }, 
				{ "full gauges", "Gems/Upright_Gem" }, { "previous gauge", "Gems/Upright_Gem" }, 
				{ "next gauge", "Gems/Upright_Gem" }, { "bigger party", "Gems/Upright_Gem" }, 
				{ "fixed", "Gems/Upright_Gem" }, { "turns", "Gems/Upright_Gem" }, { "1 enemy", "Gems/Upright_Gem" },
			    { "higher slot", "Gems/Upright_Gem" }, { "lower slot", "Gems/Upright_Gem" }, { "smaller slot", "Gems/Upright_Gem" },
			    { "slot 3", "Gems/Upright_Gem" }, { "slot 4", "Gems/Upright_Gem" }, { "slot 6", "Gems/Upright_Gem" },
            }
		},
		{
			"HEAL", new Dictionary<string, string>()
			{
				{ "Slightly", "Heal/Slightly" }, { "Moderately", "Heal/Moderately" }, { "Greatly", "Heal/Greatly" }, 
				{ "Significantly", "Heal/Significantly" }, { "MAX", "Heal/Max" },
			    { "1", "Heal/Slightly" }, { "2", "Heal/Moderately" },
			    { "3", "Heal/Heal/Greatly" }, { "4", "Heal/Significantly" },
            }
		},
		{
			"GAUGE", new Dictionary<string, string>()
			{
				{ "Use All", "Gems/Upright_Gem" }, { "2", "Gauges/2" }, { "3", "Gauges/3" }, { "4", "Gauges/4" }, 
				{ "5", "Gauges/5" }, { "6", "Gauges/6" }, { "7", "Gauges/7" },
				{ "8", "Gauges/8" }, { "10", "Gauges/10" }
			}
		},
		{
			"ESUNA", new Dictionary<string, string>()
			{
				{ "ESUNA", "Gems/Upright_Gem" }
			}
		},
		{
			"COUNT", new Dictionary<string, string>()
			{

				{ "0", "Count/0" }, { "1", "Count/1" }, { "2", "Count/2" }, { "3", "Count/3" }, 
				{ "5", "Count/5" }, { "6", "Count/6" }, { "10", "Count/10"},
			    { "Resets", "Gems/Upright_Gem" }, { "unaffected", "Count/0" }
			}
		},
	    {
	        "SPBONUS", new Dictionary<string, string>()
	        {
	            { "15", "Gems/Upright_Gem" }, { "20", "SPATKBonus/20" },
	            { "30", "SPATKBonus/30" }, { "40", "SPATKBonus/40" }, { "60", "SPATKBonus/60" },
	            { "70", "SPATKBonus/70" }, { "80", "SPATKBonus/80" }, { "90", "SPATKBonus/90" },
	            { "100", "SPATKBonus/100" }, { "110", "SPATKBonus/110" }, { "120", "SPATKBonus/120" }
            }
	    },
        {
			"DAMAGE+", new Dictionary<string, string>()
			{
				{ "1 enemy or 0 parts left", "Gems/Upright_Gem" }, { "Party", "Gems/Upright_Gem" },
			    { "Higher HP", "Gems/Upright_Gem" }
			}
		},
		{
			"NEXTMEDAL", new Dictionary<string, string>()
			{
				{ "Group Attack", "Gems/Upright_Gem" },
			    { "turns Magic", "Gems/Magic_Gem" }, { "turns Power", "Gems/Power_Gem" }, { "turns Speed", "Gems/Speed_Gem" }
            }
		}
	};


}
