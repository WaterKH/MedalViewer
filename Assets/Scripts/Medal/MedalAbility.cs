using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedalAbility {

	//public int ID = 0;
	public List<MedalCombatAbility> STR = new List<MedalCombatAbility>();
	public List<MedalCombatAbility> DEF = new List<MedalCombatAbility>();
    public string DEAL = "";
	public string INFL = "";
	public string HEAL = "";
	public string GAUGE = "";
	public bool ESUNA = false;
    public bool ENEMYESUNA = false;
	public string COUNT = "";
	public string SPBONUS = "";
	public string DAMAGE = "";
	public string NEXTMEDAL = "";
    public string COPYDIRECTION = "";

	// Key: STR/DEF - Key: Raise/Lower/LowerPlayer - Value: Images
	public Dictionary<string, Dictionary<string, List<Texture2D>>> CombatImages;
    public Dictionary<string, Texture2D> MiscImages;

	public void SetUpDisplayAbility()
	{
		InitImages();

		foreach(var str in STR)
		{
			var str_image = Resources.Load(ImagePaths.CombatPaths["STR"][str.Direction][str.Attribute]) as Texture2D;
			CombatImages["STR"][str.Direction].Add(str_image);
		}

		foreach(var def in DEF)
		{
			var def_image = Resources.Load(ImagePaths.CombatPaths["DEF"][def.Direction][def.Attribute]) as Texture2D;
			CombatImages["DEF"][def.Direction].Add(def_image);
		}

		if(INFL != "")
		{
			var infl_image = Resources.Load(ImagePaths.MiscPaths["INFL"][INFL]) as Texture2D;
			MiscImages["INFL"] = infl_image;
		}

		if(HEAL != "")
		{
			var heal_image = Resources.Load(ImagePaths.MiscPaths["HEAL"][HEAL]) as Texture2D;
			MiscImages["HEAL"] = heal_image;
		}

		if(GAUGE != "")
		{
			var gauge_image = Resources.Load(ImagePaths.MiscPaths["GAUGE"][GAUGE]) as Texture2D;
			MiscImages["GAUGE"] = gauge_image;
		}

		if(ESUNA)
		{
			var esuna_image = Resources.Load(ImagePaths.MiscPaths["ESUNA"]["ESUNA"]) as Texture2D;
			MiscImages["ESUNA"] = esuna_image;
		}

		if(COUNT != "")
		{
			var count_image = Resources.Load(ImagePaths.MiscPaths["COUNT"][COUNT]) as Texture2D;
			MiscImages["COUNT"] = count_image;
		}

	    if (SPBONUS != "")
	    {
	        var spbonus_image = Resources.Load(ImagePaths.MiscPaths["SPBONUS"][SPBONUS]) as Texture2D;
	        MiscImages["SPBONUS"] = spbonus_image;
	    }

        if (DAMAGE != "")
		{
			var dam_image = Resources.Load(ImagePaths.MiscPaths["DAMAGE+"][DAMAGE]) as Texture2D;
			MiscImages["DAMAGE+"] = dam_image;
		}

		if(NEXTMEDAL != "")
		{
			var next_image = Resources.Load(ImagePaths.MiscPaths["NEXTMEDAL"][NEXTMEDAL]) as Texture2D;
			MiscImages["NEXTMEDAL"] = next_image;
		}
	}

	public void InitImages()
	{
        CombatImages = new Dictionary<string, Dictionary<string, List<Texture2D>>>
        {
            { "STR", new Dictionary<string, List<Texture2D>>() }
        };
        CombatImages["STR"].Add("Raises", new List<Texture2D>());
		CombatImages["STR"].Add("Lowers", new List<Texture2D>());

		CombatImages.Add("DEF", new Dictionary<string, List<Texture2D>>());
		CombatImages["DEF"].Add("Raises", new List<Texture2D>());
		CombatImages["DEF"].Add("Lowers", new List<Texture2D>());
		CombatImages["DEF"].Add("PlayerLowers", new List<Texture2D>()); // TODO Redesign backend to account for player
        
        MiscImages = new Dictionary<string, Texture2D>
        {
            { "INFL", null },
            { "HEAL", null },
            { "GAUGE", null },
            { "ESUNA", null },
            { "COUNT", null },
            { "DAMAGE+", null },
            { "NEXTMEDAL", null },
            { "SPBONUS", null }
        };
    }
}
