using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedalCombatAbility {

	public string Direction = "";
	public string Attribute = "";
	public string Tier = "";
	public string Amount = "";

	public MedalCombatAbility(string[] ability)
	{
		Direction = ability[0];
		Attribute = ability[1];
		Tier = ability[2];
		Amount = ability[3];
	}

	public MedalCombatAbility(string dir, string attr, string tier, string amt)
	{
		Direction = dir;
		Attribute = attr;
		Tier = tier;
		Amount = amt;
	}
}
