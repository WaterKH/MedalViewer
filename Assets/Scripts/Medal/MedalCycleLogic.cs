using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System;

namespace MedalViewer.Medal
{
    public class MedalCycleLogic : MonoBehaviour
    {
        LoadManager LoadManager;

        // This field can be accesed through our singleton instance,
        // but it can't be set in the inspector, because we use lazy instantiation
        public bool stopped;
        public bool loadInitial = true;
        public bool firstPass = true;
        
        public Dictionary<GameObject, int> CycleMedalsList = new Dictionary<GameObject, int>();

        // Static singleton instance
        private static MedalCycleLogic instance;
        private Coroutine lastRoutine = null;

        // Static singleton property
        public static MedalCycleLogic Instance => instance ?? (instance = new GameObject("MedalCycleLogic").AddComponent<MedalCycleLogic>());

        private void Awake()
        {
            LoadManager = GameObject.FindGameObjectWithTag("Loading").GetComponent<LoadManager>();
        }

        // Instance method, this method can be accessed through the singleton instance
        public IEnumerator CycleMedals(Dictionary<GameObject, int> medals)
        {
            int count = 0;
            while (!stopped)
            {
                // TODO Do fades for medals
                foreach (var m in medals)
                {
                    if (m.Key.GetComponentsInChildren<RectTransform>().First(x => x.name == "SubContent").childCount == 1)
                        continue;

                    var texture1 = m.Key.GetComponentsInChildren<CanvasGroup>().First(x => x.name == "MedalImage");
                    var texture2 = m.Key.GetComponentsInChildren<CanvasGroup>().First(x => x.name == "AltMedalImage");

                    var currMedal = m.Key.GetComponentsInChildren<RectTransform>().First(x => x.name == "SubContent").GetChild(m.Value);

                    // Display MedalImage
                    if (count == 1)
                    {
                        StartCoroutine(SwapImageCanvasGroup(texture1, texture2));
                        //m.Key.GetComponent<RawImage>().texture = currMedal.GetComponent<RawImage>().texture;
                    }
                    // Display AltMedalImage
                    else
                    {
                        StartCoroutine(SwapImageCanvasGroup(texture2, texture1));
                    }
                }

                List<GameObject> keys = new List<GameObject>(medals.Keys);
                foreach (var key in keys)
                {
                    var mod = key.GetComponentsInChildren<RectTransform>().First(x => x.name == "SubContent").childCount;
                    medals[key] = (medals[key] + 1) % mod;

                    var nextMedal = key.GetComponentsInChildren<RectTransform>().First(x => x.name == "SubContent").GetChild((medals[key] + 1) % mod);

                    // MedalImage is displaying
                    if (count == 0)
                    {
                        key.GetComponentsInChildren<RawImage>().First(x => x.name == "AltMedalImage").texture = nextMedal.GetComponent<RawImage>().texture;
                    }
                    // AltMedalImage is displaying
                    else
                    {
                        key.GetComponentsInChildren<RawImage>().First(x => x.name == "MedalImage").texture = nextMedal.GetComponent<RawImage>().texture;
                    }
                }
                
                count++;
                if (count % 2 == 0)
                    count = 0;

                if (loadInitial)
                {
                    loadInitial = false;
                    LoadManager.FinishLoading();
                }

                yield return new WaitForSeconds(1.5f);
            }
        }

        public IEnumerator SwapImageCanvasGroup(CanvasGroup texture1, CanvasGroup texture2)
        {
            StartCoroutine(ShowDisplay(texture1));
            StartCoroutine(HideDisplay(texture2));
            yield return null;
        }

        private IEnumerator ShowDisplay(CanvasGroup canvasGroup)
        {
            var isTransition = true;
            var elapsedTime = 0.0f;

            while (isTransition)
            {
                elapsedTime += Time.deltaTime / 2;

                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1, elapsedTime);
                
                if (canvasGroup.alpha >= 0.999f)
                {
                    canvasGroup.alpha = 1;

                    isTransition = false;
                }
                
                yield return null;
            }
        }

        private IEnumerator HideDisplay(CanvasGroup canvasGroup)
        {
            var isTransition = true;
            var elapsedTime = 0.0f;

            while (isTransition)
            {
                elapsedTime += Time.deltaTime / 2;
                
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0, elapsedTime);

                if (canvasGroup.alpha <= 0.001f)
                {
                    canvasGroup.alpha = 0;

                    isTransition = false;
                }

                yield return null;
            }
        }

        public IEnumerator PopulateCycleMedals(Dictionary<int, Dictionary<double, GameObject>> MedalGameObjects)
        {
            while (LoadManager.IsLoading)
            {
                yield return null;
            }

            LoadManager.StartLoading();

            foreach (var tier in MedalGameObjects)
            {
                foreach (var mult in tier.Value)
                {
                    var medal = mult.Value;
                    var subContent = medal.GetComponentsInChildren<RectTransform>().First(x => x.name == "SubContent");
                    var subTexture = subContent.GetComponentInChildren<RawImage>().texture;
                    var medalTexture = medal.GetComponentsInChildren<RawImage>().First(x => x.name == "MedalImage").texture;

                    //Globals.CycleMedals.Add(medal, 0);
                    CycleMedalsList.Add(medal, 0);
                }
            }

            MedalCycleLogic.Instance.StartCycleMedals();
            //Loading.FinishLoading();
        }

        public void StartCycleMedals()
        {
            if (stopped || firstPass)
            {
                stopped = false;
                firstPass = false;
                lastRoutine = StartCoroutine(CycleMedals(this.CycleMedalsList));
            }
        }

        public void StopCycleMedals()
        {
            if (lastRoutine == null)
                return;

            this.CycleMedalsList.Clear();
            stopped = true;
            //firstPass = false;
            StopCoroutine(lastRoutine);
            StopAllCoroutines();
        }
    }
}