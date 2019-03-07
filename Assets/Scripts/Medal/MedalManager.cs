using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
//using SQLite4Unity3d;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System;

public class MedalManager : MonoBehaviour {

    //public Dictionary<int, Medal> medals = new Dictionary<int, Medal>();

    private readonly string connectionString = "";//ConfigurationManager.ConnectionStrings["MedalViewerConnectionString"].ConnectionString;
    //private SQLiteConnection connection;
    
    public async Task GetMedals(MedalFilter medalFilter)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            var commandString = "Select * From Medal";

            using (SqlCommand command = new SqlCommand(commandString, conn))
            {
                //using (SqlDataReader reader = (await command.ExecuteReaderAsync()))
                //{
                //    while (reader.Read())
                //    {
                //        var medal = new Medal
                //        {
                //            Id = Convert.IsDBNull(reader[0]) ? "" : reader.GetString(0),
                //            Name = Convert.IsDBNull(reader[1]) ? "" : reader.GetString(1),
                //            ImageURL = Convert.IsDBNull(reader[2]) ? "" : reader.GetString(2),
                //            Star = Convert.IsDBNull(reader[3]) ? 0 : reader.GetInt32(3),
                //            Class = Convert.IsDBNull(reader[4]) ? "" : reader.GetString(4),
                //            Type = Convert.IsDBNull(reader[5]) ? "" : reader.GetString(5),
                //            Attribute_PSM = Convert.IsDBNull(reader[6]) ? "" : reader.GetString(6),
                //            Attribute_UR = Convert.IsDBNull(reader[7]) ? "" : reader.GetString(7),
                //            Discriminator = Convert.IsDBNull(reader[8]) ? "" : reader.GetString(8),
                //            BaseAttack = Convert.IsDBNull(reader[9]) ? 0 : reader.GetInt32(9),
                //            MaxAttack = Convert.IsDBNull(reader[10]) ? 0 : reader.GetInt32(10),
                //            BaseDefense = Convert.IsDBNull(reader[11]) ? 0 : reader.GetInt32(11),
                //            MaxDefense = Convert.IsDBNull(reader[12]) ? 0 : reader.GetInt32(12),
                //            TraitSlots = Convert.IsDBNull(reader[13]) ? 0 : reader.GetInt32(13),
                //            BasePetPoints = Convert.IsDBNull(reader[14]) ? 0 : reader.GetInt32(14),
                //            MaxPetPoints = Convert.IsDBNull(reader[15]) ? 0 : reader.GetInt32(15),
                //            Ability = Convert.IsDBNull(reader[16]) ? "" : reader.GetString(16),
                //            AbilityDescription = Convert.IsDBNull(reader[17]) ? "" : reader.GetString(17),
                //            Target = Convert.IsDBNull(reader[18]) ? "" : reader.GetString(18),
                //            Gauge = Convert.IsDBNull(reader[19]) ? 0 : reader.GetInt32(19),
                //            BaseMultiplier = Convert.IsDBNull(reader[20]) ? "" : reader.GetString(20),
                //            MaxMultiplier = Convert.IsDBNull(reader[21]) ? "" : reader.GetString(21),
                //            GuiltMultiplier = Convert.IsDBNull(reader[22]) ? "" : reader.GetString(22),
                //            SubslotMultiplier = Convert.IsDBNull(reader[23]) ? "" : reader.GetString(23),
                //            Tier = Convert.IsDBNull(reader[24]) ? 0 : reader.GetInt32(24),
                //            SupernovaName = Convert.IsDBNull(reader[25]) ? "" : reader.GetString(25),
                //            SupernovaDescription = Convert.IsDBNull(reader[26]) ? "" : reader.GetString(26),
                //            SupernovaDamage = Convert.IsDBNull(reader[27]) ? "" : reader.GetString(27),
                //            SupernovaTarget = Convert.IsDBNull(reader[28]) ? "" : reader.GetString(28),
                //            Effect = Convert.IsDBNull(reader[29]) ? "" : reader.GetString(29),
                //            Effect_Description = Convert.IsDBNull(reader[30]) ? "" : reader.GetString(30)
                //        };
                //    }
                //}
            }
        }
    }

    void Awake () 
	{
        using (SqlConnection conn = new SqlConnection(connectionString))
        {

        }
//#if UNITY_EDITOR
//            var dbPath = string.Format(@"Assets/StreamingAssets/{0}", databaseName);
//	    connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite); // TODO In commercial usage, make this only ReadOnly
//#else
//        // check if file exists in Application.persistentDataPath
//        var filepath = string.Format("{0}/{1}", Application.streamingAssetsPath, "/" + databaseName);
//        connection = new SQLiteConnection(filepath, SQLiteOpenFlags.ReadWrite);
//#endif

//        Globals.MedalsTable = connection.Table<Medal>();
//	    Globals.Connection = connection;

        foreach(var medal in Globals.MedalsTable)
        {
            Globals.Medals.Add(medal.Id, medal);
        }
    }
}
