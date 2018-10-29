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
	public SettingsManager settings;

	public List<GameObject> parents;
	//public Dictionary<GameObject, int> medalChildrenHolders = new Dictionary<GameObject, int>();

	private Parser parser = new Parser();

	private static bool stopped;
	private static bool firstPass;

	void Start () 
	{
		//TODO We can just grab two from the vertical to get the difference and then just multiply by our guilt
		sortMedals.SortManager(medalCreator.medals, settings);

		// DEFAULT
		SetupMedalsByTierAndMult(sortMedals.medals_by_tier);
	}

	// TODO Allow this to be for every thing, not just tier/ mult
	// Tiers as X and Y as Mults
	public void SetupMedalsByTierAndMult(Dictionary<int, List<Medal>> tier_medals)
	{
		medalPositionLogic.Initialize();

        //var previous_medal_img = "";
        //var previousMedalName = "";

        foreach (var tiers in tier_medals)
		{
			var medalsToQuery = tiers.Value;
			if(tiers.Key == 0)
				continue;

            var medal_dict = new Dictionary<float, List<GameObject>>();
			foreach(var medal in medalsToQuery)
			{
				var medal_object = Instantiate(Resources.Load("Medal") as GameObject);
				var medal_img = medal.ImageURL;

                //if(medal_img == "NULL" && medal.Name == previousMedalName)
				//{
				//	print(medal_img + " " + previous_medal_img);
				//	medal_img = previous_medal_img;
                //}

				var guilt_float = parser.ParseGuilt(medal);

				if(!medal_dict.ContainsKey(guilt_float))
				{
					medal_dict.Add(guilt_float, new List<GameObject>());
				}
				medal_dict[guilt_float].Add(medal_object);

				//NAME
				medal_object.name = medal.Name;

				//IMAGE
				SetMedalImage(medal, medal_object, medal_img);

				//MedalDisplay.cs Updating
				medal_object.GetComponent<MedalDisplay>().MapVariables(medal);//, MedalData);

				//previous_medal_img = medal_img;
                //previousMedalName = medal.Name;
     		}

//			foreach(var kv in medal_dict)
//				foreach(var kv2 in kv.Value)
//					print(kv.Key + " " + kv2.name);

			//DEFAULT POSITION (By tier (X - [4 - 8]) and multiplier (Y - [> 30]))
			int medal_position_counter = 0;
			foreach(var medals in medal_dict.OrderByDescending(x => x.Key))
			{
				var tempObject = Instantiate(Resources.Load("MedalDisplay") as GameObject);
				tempObject.transform.SetParent(parents[tiers.Key - 1].transform);
				tempObject.name = medals.Key.ToString();// + "_" + tiers.Key + "_" + medal_position_counter;

				foreach(var medal_object in medals.Value)
				{
					medal_object.transform.SetParent(tempObject.transform);
					//medalHolders.Add(tempObject);
				}
				++medal_position_counter;

				medalPositionLogic.SetMedalHolderPosition(tempObject, medals.Key, tiers.Key);

				MedalCycleLogic.Instance.medalChildrenHolders.Add(tempObject, 0);
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
