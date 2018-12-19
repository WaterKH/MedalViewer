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
						{ "Normal", "Saps/S"}, 
						{ "Power", "Saps/S_P"}, { "Speed", "Saps/S_S"}, { "Magic", "Saps/S_M"},

					    { "P", "Saps/S_P"}, { "S", "Saps/S_S"}, { "M", "Saps/S_M"}
                    }
				},
				{
					"Raises", new Dictionary<string, string> ()
					{
						{ "Normal", "Boosts/S"}, { "Upright", "Boosts/S_U"}, { "Reverse", "Boosts/S_R" },
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
						{ "Normal", "Saps/D"}, { "Upright", "Saps/D_U"}, { "Reverse", "Saps/D_R" },
						{ "Power", "Saps/D_P"}, { "Speed", "Saps/D_S"}, { "Magic", "Saps/D_M"},

					    { "U", "Saps/D_U"}, { "R", "Saps/D_R" },
					    { "P", "Saps/D_P"}, { "S", "Saps/D_S"}, { "M", "Saps/D_M"}
                    }
				},
				{
					"Raises", new Dictionary<string, string> ()
					{
						{ "Normal", "Boosts/D"}, 
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
			"INFL", new Dictionary<string, string>()
			{
				{ "poisoned", "StatusEffect/Poison" }, { "sleeping", "StatusEffect/Sleep" }, { "paralyzed", "StatusEffect/Paralysis" }, 
				{ "no defense", "Gems/Sun_Gem" }, { "more turns", "Gems/Sun_Gem" }, 
				{ "higher your HP", "Gems/Sun_Gem" }, { "sp attacks", "Gems/Sun_Gem" }, { "more gauges used", "Gems/Sun_Gem" }, 
				{ "defense sap", "Gems/Sun_Gem" }, { "lux", "Gems/Sun_Gem" }, { "enemy deaths", "Gems/Sun_Gem" }, 
				{ "full gauges", "Gems/Sun_Gem" }, { "previous gauge", "Gems/Sun_Gem" }, 
				{ "next gauge", "Gems/Sun_Gem" }, { "bigger party", "Gems/Sun_Gem" }, { "more skills", "Gems/Sun_Gem" }, 
				{ "fixed", "Gems/Sun_Gem" }, { "turns", "Gems/Sun_Gem" }, { "1 enemy", "Gems/Sun_Gem" },
			    { "higher slot", "Gems/Sun_Gem" }, { "lower slot", "Gems/Sun_Gem" }, { "smaller slot", "Gems/Sun_Gem" },
			    { "slot 3", "Gems/Sun_Gem" }, { "slot 4", "Gems/Sun_Gem" }, { "slot 6", "Gems/Sun_Gem" },
            }
		},
		{
			"HEAL", new Dictionary<string, string>()
			{
				{ "Slightly", "Gems/Sun_Gem" }, { "Moderately", "Gems/Sun_Gem" }, { "Greatly", "Gems/Sun_Gem" }, 
				{ "Significantly", "Gems/Sun_Gem" }, { "MAX", "Gems/Sun_Gem" },
			    { "1", "Gems/Sun_Gem" }, { "2", "Gems/Sun_Gem" },
			    { "3", "Gems/Sun_Gem" }, { "4", "Gems/Sun_Gem" },
            }
		},
		{
			"GAUGE", new Dictionary<string, string>()
			{
				{ "Use All", "Gems/Sun_Gem" }, { "2", "Gems/Sun_Gem" }, { "3", "Gems/Sun_Gem" }, { "4", "Gems/Sun_Gem" }, 
				{ "5", "Gems/Sun_Gem" }, { "6", "Gems/Sun_Gem" }, { "7", "Gems/Sun_Gem" },
				{ "8", "Gems/Sun_Gem" }, { "10", "Gems/Sun_Gem" }
			}
		},
		{
			"ESUNA", new Dictionary<string, string>()
			{
				{ "ESUNA", "Gems/Sun_Gem" }
			}
		},
		{
			"COUNT", new Dictionary<string, string>()
			{

				{ "0", "Count/0" }, { "1", "Count/1" }, { "2", "Count/2" }, { "3", "Count/3" }, 
				{ "5", "Count/5" }, { "6", "Count/6" }, { "Reset", "Gems/Sun_Gem" },
			    { "unaffected", "Count/0" }
			}
		},
		{
			"DAMAGE+", new Dictionary<string, string>()
			{
				{ "1 enemy or 0 parts left", "Gems/Sun_Gem" }, { "Party", "Gems/Sun_Gem" },
			    { "Higher HP", "Gems/Sun_Gem" }
			}
		},
		{
			"NEXTMEDAL", new Dictionary<string, string>()
			{
				{ "Group Attack", "Gems/Sun_Gem" },
			    { "turns Magic", "Gems/Magic_Gem" }, { "turns Power", "Gems/Power_Gem" }, { "turns Speed", "Gems/Speed_Gem" }
            }
		}
	};


}
