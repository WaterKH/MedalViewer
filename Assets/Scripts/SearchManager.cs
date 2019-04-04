using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MedalViewer.Medal;

namespace MedalViewer
{
    public class SearchManager : MonoBehaviour
    {
        public MedalManager MedalManager;
        public FilterDisplayManager FilterDisplayManager;
        public MedalLogicManager MedalLogicManager;
        public Loading Loading;

        public CanvasGroup Search;
        public InputField SearchBar;
        public Transform Content;

        public List<GameObject> SearchMedalObjects = new List<GameObject>();

        public bool IsDisplayingSearch;

        // Start is called before the first frame update
        void Awake()
        {
            SearchBar.onEndEdit.AddListener(x => GetMedals(x));
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
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
            FilterDisplayManager.HideFilterMenu();

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

            var sqlStatement = "Select TOP(10) * From Medal Order By Id Desc";
            StartCoroutine(MedalManager.GetSearchMedalsFromPHP(sqlStatement));

            StartCoroutine(Display());
        }

        public void GetMedals(string lookFor)
        {
            ClearSearch();

            var sqlStatement = $"Select * From Medal Where Name Like '%{lookFor}%' Order By Id Desc";
            StartCoroutine(MedalManager.GetSearchMedalsFromPHP(sqlStatement));

            StartCoroutine(Display());
        }

        public void ClearSearch()
        {
            Globals.SearchMedals.Clear();

            SearchMedalObjects.ForEach(x => Destroy(x));
            SearchMedalObjects.Clear();
        }

        public IEnumerator Display()
        {
            while (Loading.IsLoading)
            {
                yield return null;
            }

            Loading.StartLoading();

            SearchMedalObjects = MedalLogicManager.GenerateSearchMedals(Content);
        }
    }
}
