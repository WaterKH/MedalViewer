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
					"Lower", new Dictionary<string, string> ()
					{
						{ "Normal", "Saps/S"}, 
						{ "Power", "Saps/S_P"}, { "Speed", "Saps/S_S"}, { "Magic", "Saps/S_M"}
					}
				},
				{
					"Raise", new Dictionary<string, string> ()
					{
						{ "Normal", "Boosts/S"}, { "Upright", "Boosts/S_U"}, { "Reverse", "Boosts/S_R" },
						{ "Power", "Boosts/S_P"}, { "Speed", "Boosts/S_S"}, { "Magic", "Boosts/S_M"}
					}
				}

			}
		},
		{
			"DEF", new Dictionary<string, Dictionary<string, string>> ()
			{
				{
					"Lower", new Dictionary<string, string> ()
					{
						{ "Normal", "Saps/D"}, { "Upright", "Saps/D_U"}, { "Reverse", "Saps/D_R" },
						{ "Power", "Saps/D_P"}, { "Speed", "Saps/D_S"}, { "Magic", "Saps/D_M"}
					}
				},
				{
					"Raise", new Dictionary<string, string> ()
					{
						{ "Normal", "Boosts/D"}, 
						{ "Power", "Boosts/D_P"}, { "Speed", "Boosts/D_S"}, { "Magic", "Boosts/D_M"}
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
				{ "Poison", "StatusEffect/Poison" }, { "Sleep", "StatusEffect/Sleep" }, { "Paralysis", "StatusEffect/Paralysis" }, 
				{ "No Defense", "Gems/Sun_Gem" }, { "More Turns", "Gems/Sun_Gem" }, 
				{ "Higher HP", "Gems/Sun_Gem" }, { "SPAttacks", "Gems/Sun_Gem" }, { "Gauge Used", "Gems/Sun_Gem" }, 
				{ "Defense Sap", "Gems/Sun_Gem" }, { "Lux", "Gems/Sun_Gem" }, { "Enemy Deaths", "Gems/Sun_Gem" }, 
				{ "Full Gauges", "Gems/Sun_Gem" }, { "Previous Gauge", "Gems/Sun_Gem" }, 
				{ "Next Gauge", "Gems/Sun_Gem" }, { "Bigger Party", "Gems/Sun_Gem" }, { "More Skills", "Gems/Sun_Gem" }, 
				{ "Higher Slot", "Gems/Sun_Gem" }, { "Lower Slot", "Gems/Sun_Gem" }, { "Fixed", "Gems/Sun_Gem" },  
				{ "Turns", "Gems/Sun_Gem" }, { "Slot 3", "Gems/Sun_Gem" }, { "Slot 4", "Gems/Sun_Gem" }, 
				{ "1 Enemy", "Gems/Sun_Gem" }, { "Smaller Slot", "Gems/Sun_Gem" }, { "Slot 6", "Gems/Sun_Gem" }, // TODO Make custom thingy
			}
		},
		{
			"HEAL", new Dictionary<string, string>()
			{
				{ "Slightly", "Gems/Sun_Gem" }, { "Moderately", "Gems/Sun_Gem" }, { "Greatly", "Gems/Sun_Gem" }, 
				{ "Significantly", "Gems/Sun_Gem" }, { "Max", "Gems/Sun_Gem" }
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
				{ "ESUNA", "Gems/Sun_Gem" }// TODO Custom make thingy
			}
		},
		{
			"COUNT", new Dictionary<string, string>()
			{

				{ "0", "Gems/Sun_Gem" }, { "+1", "Gems/Sun_Gem" }, { "+2", "Gems/Sun_Gem" }, { "+3", "Gems/Sun_Gem" }, 
				{ "+5", "Gems/Sun_Gem" }, { "+6", "Gems/Sun_Gem" }, { "Reset", "Gems/Sun_Gem" },

			}
		},
		{
			"DAMAGE+", new Dictionary<string, string>()
			{
				{ "1 enemy or 0 parts", "Gems/Sun_Gem" }, { "Party", "Gems/Sun_Gem" }
			}
		},
		{
			"NEXTMEDAL", new Dictionary<string, string>()
			{
				{ "Group Attack", "Gems/Sun_Gem" },
			}
		}
	};


}
