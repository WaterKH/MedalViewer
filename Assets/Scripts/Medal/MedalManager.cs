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
        public MedalFilterManager MedalFilterManager = MedalFilterManager.Instance;
        public LoadManager LoadManager;
        public MedalLogicManager MedalLogicManager;
        public MedalGraphViewManager MedalGraphViewManager;
        public UIController UIController;

        public bool IsRunningClient = false;

        public Dictionary<int, Medal> Medals = new Dictionary<int, Medal>();
        
        private readonly string selectFilteredMedalsPHP = "https://mvphp.azurewebsites.net/selectFilteredMedals.php";
        
        void Awake()
        {
            // TODO Do we want this to be the only point of entry??
            Initialize();
        }

        public void Initialize()
        {
            #region Retrieve Medals from Database

            MedalFilterManager.DefaultFilters();

            HandleGetMedals(MedalFilterManager);

            #endregion

            #region Display Medals

            MedalGraphViewManager.StillRunning = StartCoroutine(MedalGraphViewManager.Display());
            
            #endregion
        }

        public void HandleGetMedals(MedalFilterManager medalFilter)
        {
            LoadManager.StartLoading();

            Medals.Clear();

            UIController.ResetViewWindow();//.OffsetY = 250;

            StartCoroutine(GetMedalsFromPHP(medalFilter));
        }

        public IEnumerator GetMedalsFromPHP(MedalFilterManager medalFilter)
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

                        var medal = new Medal(splitRow);

                        Medals.Add(medal.Id, medal);
                    }
                }
            }

            LoadManager.FinishLoading();
        }
    }
}
