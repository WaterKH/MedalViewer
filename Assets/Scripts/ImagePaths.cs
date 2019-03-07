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
            // TODO Add in eveeeeerything that Mattie did
			"INFL", new Dictionary<string, string>() // TODO Break this down into [more/less/higher/lower][attr]
			{
                { "higher your HP", "Inflict/HP/ATKhealthfull" }, { "lower your HP", "Inflict/HP/ATKhealthlow" },

                { "more lux", "Inflict/Lux/ATKluxplus" }, { "less lux", "Inflict/Lux/ATKluxminus" },

                { "slot 1", "Inflict/Slots/1" }, { "slot 2", "Inflict/Slots/2" }, { "slot 3", "Inflict/Slots/3" },
                { "slot 4", "Inflict/Slots/4" }, { "slot 5", "Inflict/Slots/5" }, { "slot 6", "Inflict/Slots/6" },
                { "lower the slot number", "Inflict/Slots/minus" }, { "lower slot", "Inflict/Slots/minus" }, { "smaller slot", "Inflict/Slots/minus" },
                { "higher the slot number", "Inflict/Slots/plus" }, { "higher slot", "Inflict/Slots/plus" },
                
                { "more SP attacks used this turn", "Inflict/SP/ATKspmore" }, { "more special attacks used this turn", "Inflict/SP/ATKspatk" },
                { "more special attacks used in succession", "Inflict/SP/ATKspsuccALT" }, { "more gauges are full", "Inflict/SP/ATKspmore" },
                { "more gauges used", "Inflict/SP/ATKspmore" }, { "Uses all gauges", "Inflict/SP/ATKspuseall" },

                { "poisoned", "Inflict/StatusEffect/ATKpoison" }, { "sleeping", "Inflict/StatusEffect/ATKsleep" }, { "paralyzed", "Inflict/StatusEffect/ATKparal" },

                { "fixed amount of damage", "Inflict/ATKfixed" },
                { "enemy deaths", "Inflict/ATKkills" },
                { "bigger your party", "Inflict/ATKparty" },
                { "1 enemy", "Inflict/ATKsingleboss" },
                { "more skills", "Inflict/ATKskills2" },
                { "more turns", "Inflict/ATKturns" },
                { "no attributes", "Inflict/NoAtt" },
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
			"GAUGE", new Dictionary<string, string>()
			{
				{ "Uses all", "Inflict/SP/ATKspuseall" },
                { "1", "Gauges/1" }, { "2", "Gauges/2" }, { "3", "Gauges/3" }, { "4", "Gauges/4" }, { "5", "Gauges/5" },
                { "6", "Gauges/6" }, { "7", "Gauges/7" }, { "8", "Gauges/8" }, { "9", "Gauges/9" }, { "10", "Gauges/10" }
			}
		},
		{
			"ESUNA", new Dictionary<string, string>()
			{
				{ "Cures own status ailments", "Heal/HealEsuna" }
			}
		},
        {
            "DISPEL", new Dictionary<string, string>()
            {
                { "Removes status effects", "Heal/HealDispel" }
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
	        "SPBONUS", new Dictionary<string, string>()
	        {
	            { "15", "Gems/Upright_Gem" }, { "20", "SPATKBonus/20" },
	            { "30", "SPATKBonus/30" }, { "40", "SPATKBonus/40" }, { "60", "SPATKBonus/60" },
	            { "70", "SPATKBonus/70" }, { "80", "SPATKBonus/80" }, { "90", "SPATKBonus/90" },
	            { "100", "SPATKBonus/100" }, { "110", "SPATKBonus/110" }, { "120", "SPATKBonus/120" },
                { "130", "SPATKBonus/120" }, { "140", "SPATKBonus/120" }, { "150", "SPATKBonus/120" },
                { "160", "SPATKBonus/120" },
            }
	    },
        {
			"DAMAGE+", new Dictionary<string, string>()
			{
				{ "1 enemy or 0 parts left", "Inflict/atksinglepart" }, { "Party", "Inflict/atkparty" },
			    { "Higher HP", "Gems/Upright_Gem" },
                { "Lower slot number", "Inflict/slotlow" }, { "The more gauges are full", "Inflict/spmore" },
                { "the more SP attacks used in succession", "Inflict/spmore" }
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
