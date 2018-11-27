using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedalAbilityParser : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public MedalAbility Parser(string abilityDescription)
    {
        var returnMedal = new MedalAbility();

        var parts = abilityDescription.Split('.');

        foreach (var item in parts)
        {

        }

        return returnMedal;
    }
}
