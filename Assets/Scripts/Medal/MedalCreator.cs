using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using SQLite4Unity3d;

public class MedalCreator : MonoBehaviour {

	public Dictionary<int, Medal> medals = new Dictionary<int, Medal>();
	public Dictionary<int, MedalAbility> medalAbilityImages = new Dictionary<int, MedalAbility>();

	//public TextAsset medal_data;
	public TextAsset MedalTruncatedData;

	//private int atkMedal1_2VarCount = 19;
	//private int atkMedal3_4_5VarCount = 20;
	//private int atkMedal6VarCount = 22;
	//private int atkMedal7VarCount = 22;
	//private int misc_medal_var_count = 9;

	public Parser parser = new Parser();

    private readonly string databaseName = "medals.sqlite";
    private SQLiteConnection connection;

    // Use this for initialization
    void Awake () 
	{
		medalAbilityImages = parser.ParseAbilityDescription(MedalTruncatedData);

#if UNITY_EDITOR
        print(Application.streamingAssetsPath);
        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", databaseName);
	    connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadOnly);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.streamingAssetsPath, "/" + databaseName);
        connection = new SQLiteConnection(filepath, SQLiteOpenFlags.ReadOnly);
#endif


        Globals.MedalsTable = connection.Table<Medal>();

        foreach(var medal in Globals.MedalsTable)
        {
            medals.Add(medal.Id, medal);
        }

        /*var prevImage = "";
		//print(medalAbilityImages.Count);
		foreach(var line in medal_data.text.Split('\n'))
		{
			var elements = line.Split(';');
			var medal = new Medal();

			var image = elements[1];
			if(image == "NULL")
			{
				elements[1] = prevImage;
			}
			prevImage = image;

			if(elements.Length == atkMedal1_2VarCount || elements.Length == atkMedal3_4_5VarCount || 
				elements.Length == atkMedal6VarCount || elements.Length == atkMedal7VarCount)
			{
				switch(medal.Star)
				{
				case 1:
				case 2:
					((AttackMedal)medal).BaseMultiplier = elements[16];
					((AttackMedal)medal).Target = elements[17];
					((AttackMedal)medal).Gauge = elements[18];
					break;
				case 3:
				case 4:
				case 5:
					((AttackMedal)medal).BaseMultiplier = elements[16];
					((AttackMedal)medal).Target = elements[17];
					((AttackMedal)medal).MaxMultiplier = elements[18];
					((AttackMedal)medal).Gauge = elements[19];
					break;
				case "6":
					((AttackMedal)medal).BaseMultiplier = elements[16];
					((AttackMedal)medal).Target = elements[17];
					((AttackMedal)medal).MaxMultiplier = elements[18];
					((AttackMedal)medal).Gauge = elements[19];
					((AttackMedal)medal).GuiltMultiplier = elements[20];
					((AttackMedal)medal).Tier = elements[21];
					break;
				case "7":
					((AttackMedal)medal).MaxMultiplier = elements[16];
					((AttackMedal)medal).Target = elements[17];
					((AttackMedal)medal).GuiltMultiplier = elements[18];
					((AttackMedal)medal).Gauge = elements[19];
					((AttackMedal)medal).SubslotMultiplier = elements[20];
					((AttackMedal)medal).Tier = elements[21];
					break;
				default:
					print("Something is wrong");
					break;
				}

			}
			else if(elements.Length == misc_medal_var_count)
			{
				medal = new MiscMedal(elements);
			}
			else if(line == "")
			{
				break;
			}

			// Non sorted, all medals
			if(!medals.ContainsKey(medal.Name))
			{
				medals.Add(medal.Name, new List<Medal>() { medal });
			}
			else
			{
				medals[medal.Name].Add(medal);
			}
		}*/
    }
}
