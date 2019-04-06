using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System;
using UnityEngine.Networking;
using System.Text;

namespace MedalViewer.Medal
{
    public class MedalManager : MonoBehaviour
    {
        public MedalFilter MedalFilter;
        public Loading Loading;
        public MedalLogicManager MedalLogicManager;
        public MedalGraphViewLogic MedalGraphViewLogic;
        public bool IsRunningClient = false;

        private readonly string selectFilteredMedalsPHP = "https://mvphp.azurewebsites.net/selectFilteredMedals.php";
        private readonly string connectionString = "Server=tcp:medalviewer.database.windows.net,1433;Initial Catalog=medalviewer;Persist Security Info=False;User ID=MedalViewer;Password=Password1!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        void Awake()
        {
            // TODO Do we want this to be the only point of entry??
            Initialize();
        }

        public void Initialize()
        {
            #region Retrieve Medals from Database

            MedalFilter.DefaultFilters();

            HandleGetMedals(MedalFilter);

            #endregion
        }

        public void HandleGetMedals(MedalFilter medalFilter)
        {
            Loading.StartLoading();

            Globals.Medals.Clear();

            if (IsRunningClient)
                GetMedals(MedalFilter);
            else
                StartCoroutine(GetMedalsFromPHP(MedalFilter));

            #region Display Medals

            StartCoroutine(MedalGraphViewLogic.Display());

            #endregion
        }

        public void GetMedals(MedalFilter medalFilter)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                var commandString = medalFilter.GenerateFilterQuery();
                
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

                            Globals.Medals.Add(medal.Id, medal);
                        }
                    }
                }
            }

            Loading.FinishLoading();
        }

        public IEnumerator GetMedalsFromPHP(MedalFilter medalFilter)
        {
            WWWForm form = new WWWForm();
            var query = medalFilter.GenerateFilterQuery();
            form.AddField("sqlQuery", query);

            using (UnityWebRequest www = UnityWebRequest.Post(selectFilteredMedalsPHP, form))
            {
                yield return www.SendWebRequest();
                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log("ERROR:: " + www.error);
                }
                else
                {
                    //Debug.Log(www.downloadHandler.text);
                    var rows = www.downloadHandler.text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var row in rows)
                    {
                        var splitRow = row.Split(new char[] { '|' }, StringSplitOptions.None);
                        
                        var medal = new Medal
                        {
                            Id = string.IsNullOrEmpty(splitRow[0]) ? -1 : int.Parse(splitRow[0]),
                            Name = string.IsNullOrEmpty(splitRow[1]) ? "" : splitRow[1],
                            ImageURL = string.IsNullOrEmpty(splitRow[2]) ? "" : splitRow[2],
                            Star = string.IsNullOrEmpty(splitRow[3]) ? 0 : int.Parse(splitRow[3]),
                            Class = string.IsNullOrEmpty(splitRow[4]) ? "" : splitRow[4],
                            Type = string.IsNullOrEmpty(splitRow[5]) ? "" : splitRow[5],
                            Attribute_PSM = string.IsNullOrEmpty(splitRow[6]) ? "" : splitRow[6],
                            Attribute_UR = string.IsNullOrEmpty(splitRow[7]) ? "" : splitRow[7],
                            Discriminator = string.IsNullOrEmpty(splitRow[8]) ? "" : splitRow[8],
                            BaseAttack = string.IsNullOrWhiteSpace(splitRow[9]) ? 0 : int.Parse(splitRow[9]),
                            MaxAttack = string.IsNullOrEmpty(splitRow[10]) ? 0 : int.Parse(splitRow[10]),
                            BaseDefense = string.IsNullOrEmpty(splitRow[11]) ? 0 : int.Parse(splitRow[11]),
                            MaxDefense = string.IsNullOrEmpty(splitRow[12]) ? 0 : int.Parse(splitRow[12]),
                            TraitSlots = string.IsNullOrEmpty(splitRow[13]) ? 0 : int.Parse(splitRow[13]),
                            BasePetPoints = string.IsNullOrEmpty(splitRow[14]) ? 0 : int.Parse(splitRow[14]),
                            MaxPetPoints = string.IsNullOrEmpty(splitRow[15]) ? 0 : int.Parse(splitRow[15]),
                            Ability = string.IsNullOrEmpty(splitRow[16]) ? "" : splitRow[16],
                            AbilityDescription = string.IsNullOrEmpty(splitRow[17]) ? "" : splitRow[17],
                            Target = string.IsNullOrEmpty(splitRow[18]) ? "" : splitRow[18],
                            Gauge = string.IsNullOrEmpty(splitRow[19]) ? 0 : int.Parse(splitRow[19]),
                            BaseMultiplierLow = string.IsNullOrEmpty(splitRow[20]) ? 0.0 : double.Parse(splitRow[20]),
                            BaseMultiplierHigh = string.IsNullOrEmpty(splitRow[21]) ? 0.0 : double.Parse(splitRow[21]),
                            MaxMultiplierLow = string.IsNullOrEmpty(splitRow[22]) ? 0.0 : double.Parse(splitRow[22]),
                            MaxMultiplierHigh = string.IsNullOrEmpty(splitRow[23]) ? 0.0 : double.Parse(splitRow[23]),
                            GuiltMultiplierLow = string.IsNullOrEmpty(splitRow[24]) ? 0.0 : double.Parse(splitRow[24]),
                            GuiltMultiplierHigh = string.IsNullOrEmpty(splitRow[25]) ? 0.0 : double.Parse(splitRow[25]),
                            SubslotMultiplier = string.IsNullOrEmpty(splitRow[26]) ? 0.0 : double.Parse(splitRow[26]),
                            Tier = string.IsNullOrEmpty(splitRow[27]) ? 0 : int.Parse(splitRow[27]),
                            SupernovaName = string.IsNullOrEmpty(splitRow[28]) ? "" : splitRow[28],
                            SupernovaDescription = string.IsNullOrEmpty(splitRow[29]) ? "" : splitRow[29],
                            SupernovaDamage = string.IsNullOrEmpty(splitRow[30]) ? "" : splitRow[30],
                            SupernovaTarget = string.IsNullOrEmpty(splitRow[31]) ? "" : splitRow[31],
                            Effect = string.IsNullOrEmpty(splitRow[32]) ? "" : splitRow[32],
                            Effect_Description = string.IsNullOrEmpty(splitRow[33]) ? "" : splitRow[33]
                        };
                        
                        Globals.Medals.Add(medal.Id, medal);
                    }
                }
            }

            Loading.FinishLoading();
        }

        public IEnumerator GetSearchMedalsFromPHP(string query)
        {
            Loading.StartLoading();

            WWWForm form = new WWWForm();
            form.AddField("sqlQuery", query);

            using (UnityWebRequest www = UnityWebRequest.Post(selectFilteredMedalsPHP, form))
            {
                yield return www.SendWebRequest();
                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log("ERROR:: " + www.error);
                }
                else
                {
                    //Debug.Log(www.downloadHandler.text);
                    var rows = www.downloadHandler.text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var row in rows)
                    {
                        var splitRow = row.Split(new char[] { '|' }, StringSplitOptions.None);

                        if (splitRow.Length == 34)
                        {
                            var medal = new Medal
                            {
                                Id = string.IsNullOrEmpty(splitRow[0]) ? -1 : int.Parse(splitRow[0]),
                                Name = string.IsNullOrEmpty(splitRow[1]) ? "" : splitRow[1],
                                ImageURL = string.IsNullOrEmpty(splitRow[2]) ? "" : splitRow[2],
                                Star = string.IsNullOrEmpty(splitRow[3]) ? 0 : int.Parse(splitRow[3]),
                                Class = string.IsNullOrEmpty(splitRow[4]) ? "" : splitRow[4],
                                Type = string.IsNullOrEmpty(splitRow[5]) ? "" : splitRow[5],
                                Attribute_PSM = string.IsNullOrEmpty(splitRow[6]) ? "" : splitRow[6],
                                Attribute_UR = string.IsNullOrEmpty(splitRow[7]) ? "" : splitRow[7],
                                Discriminator = string.IsNullOrEmpty(splitRow[8]) ? "" : splitRow[8],
                                BaseAttack = string.IsNullOrWhiteSpace(splitRow[9]) ? 0 : int.Parse(splitRow[9]),
                                MaxAttack = string.IsNullOrEmpty(splitRow[10]) ? 0 : int.Parse(splitRow[10]),
                                BaseDefense = string.IsNullOrEmpty(splitRow[11]) ? 0 : int.Parse(splitRow[11]),
                                MaxDefense = string.IsNullOrEmpty(splitRow[12]) ? 0 : int.Parse(splitRow[12]),
                                TraitSlots = string.IsNullOrEmpty(splitRow[13]) ? 0 : int.Parse(splitRow[13]),
                                BasePetPoints = string.IsNullOrEmpty(splitRow[14]) ? 0 : int.Parse(splitRow[14]),
                                MaxPetPoints = string.IsNullOrEmpty(splitRow[15]) ? 0 : int.Parse(splitRow[15]),
                                Ability = string.IsNullOrEmpty(splitRow[16]) ? "" : splitRow[16],
                                AbilityDescription = string.IsNullOrEmpty(splitRow[17]) ? "" : splitRow[17],
                                Target = string.IsNullOrEmpty(splitRow[18]) ? "" : splitRow[18],
                                Gauge = string.IsNullOrEmpty(splitRow[19]) ? 0 : int.Parse(splitRow[19]),
                                BaseMultiplierLow = string.IsNullOrEmpty(splitRow[20]) ? 0.0 : double.Parse(splitRow[20]),
                                BaseMultiplierHigh = string.IsNullOrEmpty(splitRow[21]) ? 0.0 : double.Parse(splitRow[21]),
                                MaxMultiplierLow = string.IsNullOrEmpty(splitRow[22]) ? 0.0 : double.Parse(splitRow[22]),
                                MaxMultiplierHigh = string.IsNullOrEmpty(splitRow[23]) ? 0.0 : double.Parse(splitRow[23]),
                                GuiltMultiplierLow = string.IsNullOrEmpty(splitRow[24]) ? 0.0 : double.Parse(splitRow[24]),
                                GuiltMultiplierHigh = string.IsNullOrEmpty(splitRow[25]) ? 0.0 : double.Parse(splitRow[25]),
                                SubslotMultiplier = string.IsNullOrEmpty(splitRow[26]) ? 0.0 : double.Parse(splitRow[26]),
                                Tier = string.IsNullOrEmpty(splitRow[27]) ? 0 : int.Parse(splitRow[27]),
                                SupernovaName = string.IsNullOrEmpty(splitRow[28]) ? "" : splitRow[28],
                                SupernovaDescription = string.IsNullOrEmpty(splitRow[29]) ? "" : splitRow[29],
                                SupernovaDamage = string.IsNullOrEmpty(splitRow[30]) ? "" : splitRow[30],
                                SupernovaTarget = string.IsNullOrEmpty(splitRow[31]) ? "" : splitRow[31],
                                Effect = string.IsNullOrEmpty(splitRow[32]) ? "" : splitRow[32],
                                Effect_Description = string.IsNullOrEmpty(splitRow[33]) ? "" : splitRow[33]
                            };

                            Globals.SearchMedals.Add(medal.Id, medal);
                        }
                    }
                }
            }

            Loading.FinishLoading();
        }
    }
}
