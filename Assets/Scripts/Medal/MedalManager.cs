using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System;

namespace MedalViewer.Medal
{
    public class MedalManager : MonoBehaviour
    {
        public MedalFilter MedalFilter;
        public Loading Loading;
        public MedalLogicManager MedalLogicManager;

        //public Dictionary<int, Medal> medals = new Dictionary<int, Medal>();
        private readonly string connectionString = "Server=tcp:medalviewer.database.windows.net,1433;Initial Catalog=medalviewer;Persist Security Info=False;User ID=MedalViewer;Password=Password1!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        
        void Awake()
        {
            // TODO Do we want this to be the only point of entry??
            Initialize();
        }

        public void Initialize()
        {
            Loading.StartLoading();

            #region Retrieve Medals from Database

            MedalFilter.DefaultFilters();

            GetMedals(MedalFilter);

            #endregion

            #region Display Medals

            //MedalLogicManager.Initialize();

            #endregion

            Loading.FinishLoading();
        }
        
        public void GetMedals(MedalFilter medalFilter)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                var commandString = medalFilter.GenerateFilterQuery();
                //print(commandString);
                //Debug.Log(commandString);
                using (SqlCommand command = new SqlCommand(commandString, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = (command.ExecuteReader()))
                    {
                        while (reader.Read())
                        {
                            var medal = new Medal
                            {
                                Id = reader[0] == DBNull.Value ? -1 : reader.GetInt32(0),
                                Name = reader[1] == DBNull.Value ? "" : reader.GetString(1),
                                ImageURL = reader[2] == DBNull.Value ? "" : reader.GetString(2),
                                Star = reader[3] == DBNull.Value ? 0 : reader.GetInt32(3),
                                Class = reader[4] == DBNull.Value ? "" : reader.GetString(4),
                                Type = reader[5] == DBNull.Value ? "" : reader.GetString(5),
                                Attribute_PSM = reader[6] == DBNull.Value ? "" : reader.GetString(6),
                                Attribute_UR = reader[7] == DBNull.Value ? "" : reader.GetString(7),
                                Discriminator = reader[8] == DBNull.Value ? "" : reader.GetString(8),
                                BaseAttack = reader[9] == DBNull.Value ? 0 : reader.GetInt32(9),
                                MaxAttack = reader[10] == DBNull.Value ? 0 : reader.GetInt32(10),
                                BaseDefense = reader[11] == DBNull.Value ? 0 : reader.GetInt32(11),
                                MaxDefense = reader[12] == DBNull.Value ? 0 : reader.GetInt32(12),
                                TraitSlots = reader[13] == DBNull.Value ? 0 : reader.GetInt32(13),
                                BasePetPoints = reader[14] == DBNull.Value ? 0 : reader.GetInt32(14),
                                MaxPetPoints = reader[15] == DBNull.Value ? 0 : reader.GetInt32(15),
                                Ability = reader[16] == DBNull.Value ? "" : reader.GetString(16),
                                AbilityDescription = reader[17] == DBNull.Value ? "" : reader.GetString(17),
                                Target = reader[18] == DBNull.Value ? "" : reader.GetString(18),
                                Gauge = reader[19] == DBNull.Value ? 0 : reader.GetInt32(19),
                                BaseMultiplierLow = reader[20] == DBNull.Value ? 0.0 : reader.GetDouble(20),
                                BaseMultiplierHigh = reader[21] == DBNull.Value ? 0.0 : reader.GetDouble(21),
                                MaxMultiplierLow = reader[22] == DBNull.Value ? 0.0 : reader.GetDouble(22),
                                MaxMultiplierHigh = reader[23] == DBNull.Value ? 0.0 : reader.GetDouble(23),
                                GuiltMultiplierLow = reader[24] == DBNull.Value ? 0.0 : reader.GetDouble(24),
                                GuiltMultiplierHigh = reader[25] == DBNull.Value ? 0.0 : reader.GetDouble(25),
                                SubslotMultiplier = reader[26] == DBNull.Value ? 0.0 : reader.GetDouble(26),
                                Tier = reader[27] == DBNull.Value ? 0 : reader.GetInt32(27),
                                SupernovaName = reader[28] == DBNull.Value ? "" : reader.GetString(28),
                                SupernovaDescription = reader[29] == DBNull.Value ? "" : reader.GetString(29),
                                SupernovaDamage = reader[30] == DBNull.Value ? "" : reader.GetString(30),
                                SupernovaTarget = reader[31] == DBNull.Value ? "" : reader.GetString(31),
                                Effect = reader[32] == DBNull.Value ? "" : reader.GetString(32),
                                Effect_Description = reader[33] == DBNull.Value ? "" : reader.GetString(33)
                            };
                            //Debug.Log(medal.Id + " " + medal.Name);
                            Globals.Medals.Add(medal.Id, medal);
                        }
                    }
                }
            }
        }
    }
}
