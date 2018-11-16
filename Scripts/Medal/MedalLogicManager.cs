using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MedalLogicManager : MonoBehaviour {

	public MedalCreator medalCreator;
	public MedalPositionLogic medalPositionLogic;
	public MedalSortLogic sortMedals;
	//public SettingsManager settings;

	public List<GameObject> parents;

    // Use this to access all medals - Key: tier - Key: multiplier - Value: Parent Holder for Medals
    public Dictionary<int, Dictionary<float, GameObject>> AllMedalDisplayObjects; 
	//public Dictionary<GameObject, int> medalChildrenHolders = new Dictionary<GameObject, int>();

	private Parser parser = new Parser();

	private static bool stopped;
	private static bool firstPass;

	void Start ()
	{
	    Initialize();

        //TODO We can just grab two from the vertical to get the difference and then just multiply by our guilt
        sortMedals.SortManager(medalCreator.medals);

		// DEFAULT
		SetupMedalsByTierAndMult(sortMedals.medals_by_tier);
    }

    public void Initialize()
    {
        AllMedalDisplayObjects = new Dictionary<int, Dictionary<float, GameObject>>();

        foreach (var kv in medalCreator.medals)
        {
            var medal = kv.Value;
            
            var medalGameObject = Instantiate(Resources.Load("Medal") as GameObject);
            var medalImage = medal.ImageURL;
            
            var guiltFloat = parser.ParseGuilt(medal); // TODO Do we need this parser anymore? Or just check for null/ 0.0f?
            var tier = medal.Tier;
            
            //NAME
            medalGameObject.name = medal.Name;
            //IMAGE
            SetMedalImage(medal, medalGameObject, medalImage);
            //MedalDisplay.cs Updating
            medalGameObject.GetComponent<MedalDisplay>().MapVariables(medal);

            if (!AllMedalDisplayObjects.ContainsKey(tier))
            {
                AllMedalDisplayObjects.Add(tier, new Dictionary<float, GameObject>());
            }

            if (!AllMedalDisplayObjects[tier].ContainsKey(guiltFloat))
            {
                var tempObject = Instantiate(Resources.Load("MedalDisplay") as GameObject);
                if (tier - 1 >= 0)
                {
                    tempObject.transform.SetParent(parents[tier - 1].transform);
                }
                else
                {
                    // TODO Add a non tier-based system in future update
                }
                tempObject.name = guiltFloat.ToString();

                AllMedalDisplayObjects[tier].Add(guiltFloat, tempObject);
            }
            
            medalGameObject.transform.SetParent(AllMedalDisplayObjects[tier][guiltFloat].transform);
        }

        foreach (var kv in AllMedalDisplayObjects)
        {
            foreach (var kv2 in kv.Value)
            {
                kv2.Value.SetActive(false);
            }
        }
    }

	// TODO Allow this to be for every thing, not just tier/ mult
	// Tiers as X and Y as Mults
	public void SetupMedalsByTierAndMult(Dictionary<int, List<Medal>> tier_medals)
	{
	    medalPositionLogic.Initialize();
        //print("Initialized".ToUpper());
        foreach (var kv in AllMedalDisplayObjects)
        {
            //print("TIER: " + kv.Key);
            foreach (var kv2 in kv.Value)
            {
                //print("GUILT: " + kv2.Key);
                //print("NAME: " + kv2.Value.name);
                //print("POSITION (BEFORE): " + kv2.Value.transform.position);
                medalPositionLogic.SetMedalHolderPosition(kv2.Value, kv2.Key, kv.Key);
                //print("POSITION (AFTER): " + kv2.Value.transform.position);
            }
        }
    }

	public void SetMedalImage(Medal medal_item, GameObject medal_object, string prevImg)
	{	
		var folderName = medal_item.Name;

		var fileName = medal_item.ImageURL.Split('/')[medal_item.ImageURL.Split('/').Length - 1];
				//.Replace("%E2%98%85_", "").Replace("&amp;", "_").Replace("%26", "").Replace("%28", "").Replace("%29", "")
				//.Replace("%C3%A9", "").Replace("%C3%AF", "");

		if(fileName == "NULL")
        {
        	print(prevImg);
            fileName = prevImg;
        }

		var path = "Medals/" + folderName + "/" + fileName.Replace(".png", "");
		
		medal_object.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(path);
	}
}
