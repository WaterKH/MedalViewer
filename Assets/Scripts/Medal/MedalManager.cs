using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System;

public class MedalManager : MonoBehaviour {

    //public Dictionary<int, Medal> medals = new Dictionary<int, Medal>();

    private readonly string connectionString = "Server=tcp:medalviewer.database.windows.net,1433;Initial Catalog=medalviewer;Persist Security Info=False;User ID=MedalViewer;Password=Password1!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";//ConfigurationManager.ConnectionStrings["MedalViewerConnectionString"].ConnectionString;
                                                  //private SQLiteConnection connection;

    public MedalFilter MedalFilter;

    public async Task GetMedals(MedalFilter medalFilter)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            var commandString = medalFilter.GenerateFilterQuery();
            //Debug.Log(commandString);

            using (SqlCommand command = new SqlCommand(commandString, conn))
            {
                await conn.OpenAsync();
                using (SqlDataReader reader = (await command.ExecuteReaderAsync()))
                {
                    //Debug.Log("Test");
                    while (await reader.ReadAsync())
                    {
                        var medal = new Medal
                        {
                            Id = Convert.IsDBNull(reader[0]) ? -1 : reader.GetInt32(0),
                            Name = Convert.IsDBNull(reader[1]) ? "" : reader.GetString(1),
                            ImageURL = Convert.IsDBNull(reader[2]) ? "" : reader.GetString(2),
                            Star = Convert.IsDBNull(reader[3]) ? 0 : reader.GetInt32(3),
                            Class = Convert.IsDBNull(reader[4]) ? "" : reader.GetString(4),
                            Type = Convert.IsDBNull(reader[5]) ? "" : reader.GetString(5),
                            Attribute_PSM = Convert.IsDBNull(reader[6]) ? "" : reader.GetString(6),
                            Attribute_UR = Convert.IsDBNull(reader[7]) ? "" : reader.GetString(7),
                            Discriminator = Convert.IsDBNull(reader[8]) ? "" : reader.GetString(8),
                            BaseAttack = Convert.IsDBNull(reader[9]) ? 0 : reader.GetInt32(9),
                            MaxAttack = Convert.IsDBNull(reader[10]) ? 0 : reader.GetInt32(10),
                            BaseDefense = Convert.IsDBNull(reader[11]) ? 0 : reader.GetInt32(11),
                            MaxDefense = Convert.IsDBNull(reader[12]) ? 0 : reader.GetInt32(12),
                            TraitSlots = Convert.IsDBNull(reader[13]) ? 0 : reader.GetInt32(13),
                            BasePetPoints = Convert.IsDBNull(reader[14]) ? 0 : reader.GetInt32(14),
                            MaxPetPoints = Convert.IsDBNull(reader[15]) ? 0 : reader.GetInt32(15),
                            Ability = Convert.IsDBNull(reader[16]) ? "" : reader.GetString(16),
                            AbilityDescription = Convert.IsDBNull(reader[17]) ? "" : reader.GetString(17),
                            Target = Convert.IsDBNull(reader[18]) ? "" : reader.GetString(18),
                            Gauge = Convert.IsDBNull(reader[19]) ? 0 : reader.GetInt32(19),
                            BaseMultiplier = Convert.IsDBNull(reader[20]) ? "" : reader.GetString(20),
                            MaxMultiplier = Convert.IsDBNull(reader[21]) ? "" : reader.GetString(21),
                            GuiltMultiplier = Convert.IsDBNull(reader[22]) ? "" : reader.GetString(22),
                            SubslotMultiplier = Convert.IsDBNull(reader[23]) ? "" : reader.GetString(23),
                            Tier = Convert.IsDBNull(reader[24]) ? 0 : reader.GetInt32(24),
                            SupernovaName = Convert.IsDBNull(reader[25]) ? "" : reader.GetString(25),
                            SupernovaDescription = Convert.IsDBNull(reader[26]) ? "" : reader.GetString(26),
                            SupernovaDamage = Convert.IsDBNull(reader[27]) ? "" : reader.GetString(27),
                            SupernovaTarget = Convert.IsDBNull(reader[28]) ? "" : reader.GetString(28),
                            Effect = Convert.IsDBNull(reader[29]) ? "" : reader.GetString(29),
                            Effect_Description = Convert.IsDBNull(reader[30]) ? "" : reader.GetString(30)
                        };
                        //Debug.Log(medal.Id + " " + medal.Name);
                        Globals.Medals.Add(medal.Id, medal);
                    }
                }
            }
        }
    }

    public void DefaultFilters()
    {
        MedalFilter.Power = true;
        MedalFilter.Speed = true;
        MedalFilter.Magic = true;
        MedalFilter.Reversed = true;
        MedalFilter.Upright = true;

        MedalFilter.SixStar = true;
        MedalFilter.SevenStar = true;

        MedalFilter.Attack = true;

        MedalFilter.Tier5 = true;
        MedalFilter.Tier6 = true;
        MedalFilter.Tier7 = true;
        MedalFilter.Tier8 = true;
        MedalFilter.Tier9 = true;

        MedalFilter.Single = true;
        MedalFilter.All = true;
        MedalFilter.Random = true;
    }

    void Awake () 
	{
        DefaultFilters();

        Task.Run(() => GetMedals(MedalFilter));
    }
}
