using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace MedalViewer.Medal
{
    public class DamoEasterEgg : MonoBehaviour
    {
        private readonly string selectFilteredMedalsPHP = "https://mvphp.azurewebsites.net/selectFilteredMedals.php";
        private Medal DamoMedal;
        private MedalDisplay MedalDisplay;

        public Loading Loading;
        public MedalSpotlightDisplayManager MedalSpotlightDisplayManager;
        public bool Clicked;

        private int framesPerSecond = 30;
        public Texture2D[] IdleFrames;
        public RawImage Image;

        public void SummonDamo()
        {
            StartCoroutine(Idle());
            //var sql = "Select * From Medal Where Id = 0";
            //StartCoroutine(GetMedal(sql));
        }
        
        public IEnumerator GetMedal(string query)
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
                    var row = www.downloadHandler.text.Replace("\n", "");
                    
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

                    DamoMedal = medal;
                    MedalDisplay = gameObject.GetComponent<MedalDisplay>();
                    MedalDisplay.MapVariables(DamoMedal);
                }
            }

            Loading.FinishLoading();

            StartCoroutine(MedalSpotlightDisplayManager.Display(null, MedalDisplay));
        }

        IEnumerator Idle()
        {
            while (!Clicked)
            {
                //print(IsLoading);
                int index = (int)(Time.time * framesPerSecond);
                index %= IdleFrames.Length;
                Image.texture = IdleFrames[index];

                if (index == 4 || index == 16)
                    yield return new WaitForSeconds(1f);
                else if(index == 7 || index == 12)
                    yield return new WaitForSeconds(0.5f);
                else
                    yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
