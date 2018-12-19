using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using SQLite4Unity3d;

public class MedalCreator : MonoBehaviour {

	//public Dictionary<int, Medal> medals = new Dictionary<int, Medal>();

    private readonly string databaseName = "medals.sqlite";
    private SQLiteConnection connection;
    
    void Awake () 
	{
#if UNITY_EDITOR
        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", databaseName);
	    connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite); // TODO In commercial usage, make this only ReadOnly
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.streamingAssetsPath, "/" + databaseName);
        connection = new SQLiteConnection(filepath, SQLiteOpenFlags.ReadWrite);
#endif

        Globals.MedalsTable = connection.Table<Medal>();
	    Globals.Connection = connection;

        foreach(var medal in Globals.MedalsTable)
        {
            Globals.Medals.Add(medal.Id, medal);
        }
    }
}
