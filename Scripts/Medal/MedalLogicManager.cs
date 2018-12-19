using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MedalLogicManager : MonoBehaviour {

	//public MedalCreator medalCreator;
	public MedalPositionLogic medalPositionLogic;
	public MedalSortLogic sortMedals;
    //public UIMovement UIMovement;
	//public SettingsManager settings;

    public GameObject parent;
	public List<GameObject> parents;

    // Use this to access all medals - Key: tier - Key: multiplier - Value: Parent Holder for Medals
    public Dictionary<int, Dictionary<float, GameObject>> AllMedalDisplayObjects; 
	//public Dictionary<GameObject, int> medalChildrenHolders = new Dictionary<GameObject, int>();

	private Parser parser = new Parser();

	private static bool stopped;
	private static bool firstPass;

    void Awake()
    {
        foreach (var child in parent.GetComponentsInChildren<Transform>().Where(x => x.name != "Vertical_TEMP (Y)").OrderBy(x => int.Parse(x.name)))
        {
            parents.Add(child.gameObject);
        }
    }

	void Start ()
	{
	    Initialize();

        //TODO We can just grab two from the vertical to get the difference and then just multiply by our guilt
        sortMedals.SortManager(Globals.Medals);

		// DEFAULT
		SetupMedalsByTierAndMult(sortMedals.medals_by_tier);
    }

    public void Add(Medal medal)
    {
        var medalGameObject = Instantiate(Resources.Load("Medal") as GameObject);
        var medalImage = medal.ImageURL;

        var guiltFloat = parser.ParseGuilt(medal); // TODO Do we need this parser anymore? Or just check for null/ 0.0f?       
        var guiltIndex = (int)guiltFloat - 1 < 0 ? 1 : (int)guiltFloat - 1;
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

                if (guiltIndex < parents.Count)
                {
                    tempObject.transform.SetParent(parents[guiltIndex].transform);
                }
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
    
    public void Initialize()
    {
        AllMedalDisplayObjects = new Dictionary<int, Dictionary<float, GameObject>>();

        foreach (var kv in Globals.Medals)
        {
            var medal = kv.Value;
            
            this.Add(medal);
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
	public void SetupMedalsByTierAndMult(Dictionary<int, List<Medal>> tier_medals)
	{
        medalPositionLogic.Initialize();
        
        foreach (var kv in AllMedalDisplayObjects)
        {
            foreach (var kv2 in kv.Value)
            {
                medalPositionLogic.SetMedalHolderPosition(kv2.Value, kv2.Key, kv.Key); // TODO Why is this out here instead of in the Init?
            }
        }
    }

	public void SetMedalImage(Medal medalItem, GameObject medalObject, string prevImg)
	{	
		var folderName = medalItem.Name;

		var fileName = medalItem.ImageURL.Split('/')[medalItem.ImageURL.Split('/').Length - 1];

		if(fileName == "NULL")
        {
        	print(prevImg);
            fileName = prevImg;
        }
        
	    if (prevImg.Contains("StreamingAssets"))
	    {
	        var bytes = File.ReadAllBytes(prevImg);
	        Texture2D tex = new Texture2D(2, 2);

	        tex.LoadImage(bytes);

	        medalObject.GetComponent<RawImage>().texture = tex;
        }
	    else
	    {
	        var path = "Medals/" + folderName + "/" + fileName.Replace(".png", "");

            medalObject.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(path);
	    }
	}
}
