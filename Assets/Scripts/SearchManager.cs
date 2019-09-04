using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MedalViewer.Medal;
using System;
using UnityEngine.Networking;

namespace MedalViewer
{
    public class SearchManager : MonoBehaviour
    {
        public MedalManager MedalManager;
        public MedalFilterDisplayManager MedalFilterDisplayManager;
        public MedalLogicManager MedalLogicManager;
        public LoadManager LoadManager;

        public CanvasGroup Search;
        public InputField SearchBar;
        public Transform Content;

        public List<GameObject> SearchMedalObjects = new List<GameObject>();

        public bool IsDisplayingSearch;

        private readonly string selectFilteredMedalsPHP = "https://mvphp.azurewebsites.net/selectFilteredMedals.php";

        private string selections = "MU.Id MUId, MU.Name MUName, MU.Image, MU.Star, CTL.Class Class, CTL.Type Type, AL.PSM PSM, AL.UR UR, MU.BaseAttack, MU.MaxAttack, MU.BaseDefense, MU.MaxDefense, MU.TraitSlots, " +
                "PPL.BasePoints, PPL.MaxPoints, MU.Ability, MU.AbilityDescription, MU.Target MUTarget, MU.Gauge, MU.BaseMultiplierLow, MU.BaseMultiplierHigh, MU.MaxMultiplierLow, MU.MaxMultiplierHigh, " +
                "MU.SubslotMultiplier, MU.Tier, SL.Name SLName, SL.Description SLDescription, SL.Damage SLDamage, SL.Target SLTarget, EL.Description ELDescription";

        private string from = "MedalUpdated MU, AttributeLookup AL, ClassTypeLookup CTL, PetPointsLookup PPL, SupernovaLookup SL, EffectLookup EL, TierLookup TL";
        private string where = "(AL.Id = MU.AttributeId AND CTL.Id = MU.ClassTypeId AND PPL.Id = MU.PetPointsId AND SL.Id = MU.SupernovaId AND EL.Id = MU.EffectId AND TL.Id = MU.Tier)";
        // Start is called before the first frame update
        void Awake()
        {
            SearchBar.onEndEdit.AddListener(x => GetMedals(x));
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.C))
            {
                if (this.IsDisplayingSearch)
                {
                    HideSearch();
                }
            }
        }

        public void DisplaySearch()
        {
            IsDisplayingSearch = true;
            MedalFilterDisplayManager.HideFilterMenu();

            Search.SetCanvasGroupActive();

            if(SearchBar.text == "")
            {
                this.GetLatest();
            }

            MedalCycleLogic.Instance.StopCycleMedals();
        }

        public void HideSearch()
        {
            IsDisplayingSearch = false;

            Search.SetCanvasGroupInactive();

            MedalCycleLogic.Instance.StartCycleMedals();
        }

        public void GetLatest()
        {
            ClearSearch();

            var sqlStatement = $"Select TOP(10) {selections} From {from} WHERE {where} Order By MUId Desc";
            StartCoroutine(this.GetSearchMedalsFromPHP(sqlStatement, result => StartCoroutine(Display(result))));

            //StartCoroutine(Display());
        }

        public void GetMedals(string lookFor)
        {
            if (lookFor.Length < 3)
                return;

            ClearSearch();

            var sqlStatement = $"Select TOP(10) {selections} From {from} WHERE {where} AND MU.Name Like '%{lookFor}%' Order By MUId Desc";
            StartCoroutine(this.GetSearchMedalsFromPHP(sqlStatement, result => StartCoroutine(Display(result))));
            
            //StartCoroutine(Display());
        }

        public void ClearSearch()
        {
            //Globals.SearchMedals.Clear();

            SearchMedalObjects.ForEach(x => Destroy(x));
            SearchMedalObjects.Clear();
        }

        public IEnumerator Display(Dictionary<int, Medal.Medal> SearchMedals)
        {
            while (LoadManager.IsLoading)
            {
                yield return null;
            }

            LoadManager.StartLoading();

            SearchMedalObjects = this.GenerateSearchMedals(Content, SearchMedals);
        }

        // TODO Do a sub loading to generate the images and *THEN* display them
        public List<GameObject> GenerateSearchMedals(Transform MedalContentHolder, Dictionary<int, Medal.Medal> SearchMedals)
        {
            var medals = new List<GameObject>();

            foreach (var kv in SearchMedals)
            {
                var medal = kv.Value;

                var medalGameObject = MedalLogicManager.CreateMedal(medal, true);

                medalGameObject.transform.SetParent(MedalContentHolder, false);

                medalGameObject.GetComponent<CanvasGroup>().SetCanvasGroupActive();

                medals.Add(medalGameObject);
            }

            var maxY = (MedalContentHolder.childCount / 2) * (MedalContentHolder.GetComponent<GridLayoutGroup>().cellSize.y + MedalContentHolder.GetComponent<GridLayoutGroup>().spacing.y);
            // Resize
            MedalContentHolder.GetComponent<RectTransform>().offsetMin = new Vector2(MedalContentHolder.GetComponent<RectTransform>().offsetMax.x, -maxY);

            MedalContentHolder.GetComponent<GridLayoutGroup>().enabled = true;

            LoadManager.FinishLoading();

            return medals;
        }


        public IEnumerator GetSearchMedalsFromPHP(string query, Action<Dictionary<int, Medal.Medal>> result)
        {
            LoadManager.StartLoading();

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
                    var medals = new Dictionary<int, Medal.Medal>();
                    //Debug.Log(www.downloadHandler.text);
                    var rows = www.downloadHandler.text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var row in rows)
                    {
                        var splitRow = row.Split(new char[] { '|' }, StringSplitOptions.None);
                        //Debug.Log(splitRow.Length);
                        var medal = new Medal.Medal(splitRow);

                        //Debug.Log("Adding: " + medal.Name + " " + medal.Id);
                        //Globals.SearchMedals.Add(medal.Id, medal);
                        medals.Add(medal.Id, medal);
                    }

                    result(medals);
                }
            }

            LoadManager.FinishLoading();
        }
    }
}
