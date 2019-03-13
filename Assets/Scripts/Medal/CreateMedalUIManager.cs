using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CreateMedalUIManager : MonoBehaviour
{
    public CanvasGroup CreateMedalUIGroup;
    public MedalLogicManager MedalLogicManager;
    public MedalSortLogic Sorter;

    #region Fields

    public RawImage MedalUploader;

    public InputField BaseDefense;
    public InputField MaxDefense;
    public InputField BaseAttack;
    public InputField MaxAttack;
    
    public InputField Id;
    public InputField Name;
    
    public InputField Ability;
    public InputField AbilityDescription;

    public InputField SupernovaDescription;

    public InputField BaseMult;
    public InputField MaxMult;
    public InputField GuiltMult;
    public InputField SubSlotMult;

    public InputField BasePetPoints;
    public InputField MaxPetPoints;

    public Dropdown TraitSlots;
    public Dropdown Target;
    public Dropdown Gauge;
    public Dropdown Tier;
    public Dropdown AttrPSM;
    public Dropdown AttrUR;
    public Dropdown Star;
    
    
    #endregion

    private Dictionary<int, string> AttributePSM = new Dictionary<int, string>
    {
        {0, "Power"}, {1, "Speed"}, {2, "Magic"}
    };
    private Dictionary<int, string> AttributeUR = new Dictionary<int, string>
    {
        {0, "Upright"}, {1, "Reversed"}
    };
    private Dictionary<int, string> Targets = new Dictionary<int, string>
    {
        {0, "All"}, {1, "Random"}, {2, "Single"}
    };

    private readonly string customMedal = "CustomMedals";
    private string imageFilePath = "";
    //private string sourceName = "";
    //private string sourceNameExtension = "";
    private string customMedalFilePath = "";

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (CreateMedalUIGroup.alpha > 0)
	    {
	        if (Input.GetKeyDown(KeyCode.Escape))
	        {
	            CloseCreateMedalUI();
	        }
	    }
    }

    public void OpenCreateMedalUI()
    {
        CreateMedalUIGroup.alpha = 1;
        CreateMedalUIGroup.blocksRaycasts = true;
        CreateMedalUIGroup.interactable = true;
    }
    
    public void CloseCreateMedalUI()
    {
        CreateMedalUIGroup.alpha = 0;
        CreateMedalUIGroup.blocksRaycasts = false;
        CreateMedalUIGroup.interactable = false;
    }

//    public void OpenMedal()
//    {
////        string extensions = "png";

////        imageFilePath = FileBrowser.OpenSingleFile("Open Medal Image", "", extensions);

////        UpdateImage(imageFilePath);

////        Debug.Log("Selected Medal: " + imageFilePath);
//    }

    public void SaveMedal()
    {

#if UNITY_EDITOR
        customMedalFilePath = $@"Assets/StreamingAssets/{customMedal}";
#else
        customMedalFilePath = string.Format("{0}/{1}", Application.streamingAssetsPath, "/" + customMedal);
#endif

        if (Id.text == "")
        {
            Debug.Log("ID Cannot be null");
            return;
        }

        var medal = new Medal
        {
            Id = int.Parse(Id.text),
            Name = Name.text,
            Star = Star.value + 1,
            TraitSlots = TraitSlots.value + 1,
            Tier = Tier.value + 1,
            Gauge = Gauge.value,
            Attribute_PSM = AttributePSM[AttrPSM.value],
            Attribute_UR = AttributeUR[AttrUR.value],
            Target = Targets[Target.value],
            Type = "Attack",
            BaseDefense = BaseDefense.text != "" ? int.Parse(BaseDefense.text) : 0,
            MaxDefense = MaxDefense.text != "" ? int.Parse(MaxDefense.text) : 0,
            BaseAttack = BaseAttack.text != "" ? int.Parse(BaseAttack.text) : 0,
            MaxAttack = MaxAttack.text != "" ? int.Parse(MaxAttack.text) : 0,
            BasePetPoints = BasePetPoints.text != "" ? int.Parse(BasePetPoints.text) : 0,
            MaxPetPoints = MaxPetPoints.text != "" ? int.Parse(MaxPetPoints.text) : 0,
            BaseMultiplier = BaseMult.text,
            MaxMultiplier = MaxMult.text,
            GuiltMultiplier = GuiltMult.text,
            SubslotMultiplier = SubSlotMult.text,
            Ability = Ability.text,
            AbilityDescription = AbilityDescription.text,
            SupernovaDescription = SupernovaDescription.text,
            ImageURL = $"{customMedalFilePath}/{Name.text}/{Name.text}-{Star.value + 1}.png",
            Discriminator = "KHAttackMedal"
        };

        try
        {            
            Directory.CreateDirectory($"{customMedalFilePath}/{Name.text}/");
            File.Copy(imageFilePath, $"{customMedalFilePath}/{Name.text}/{Name.text}-{Star.value + 1}.png");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return;
        }
        
        //Globals.Connection.Insert(medal);

        Globals.Medals.Add(medal.Id, medal);
        MedalLogicManager.Add(medal);

        //Sorter.SortManager(Globals.Medals);
        MedalLogicManager.SetupMedalsByTierAndMult(Sorter.medals_by_tier);

        CloseCreateMedalUI();
    }

    public void UpdateImage(string imageFilePath)
    {
        if (imageFilePath == "")
            return;

        var bytes = File.ReadAllBytes(imageFilePath);
        Texture2D tex = new Texture2D(2, 2);

        tex.LoadImage(bytes);

        MedalUploader.texture = tex;
    }
}
