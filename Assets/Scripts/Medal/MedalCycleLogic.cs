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
        MedalLogicManager MedalLogicManager;
        Loading Loading;

        // This field can be accesed through our singleton instance,
        // but it can't be set in the inspector, because we use lazy instantiation
        public bool stopped;
        public bool loadInitial = true;
        public bool firstPass = true;
        private Coroutine lastRoutine = null;

        // Static singleton instance
        private static MedalCycleLogic instance;
        
        // Static singleton property
        public static MedalCycleLogic Instance
        {
            get { return instance ?? (instance = new GameObject("MedalCycleLogic").AddComponent<MedalCycleLogic>()); }
        }

        void Start()
        {
            MedalLogicManager = GameObject.FindGameObjectWithTag("ScriptHolder").GetComponent<MedalLogicManager>();
            Loading = GameObject.FindGameObjectWithTag("Loading").GetComponent<Loading>();
        }

        // Instance method, this method can be accesed through the singleton instance
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
                    Loading.FinishLoading();
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

        public void StartCycleMedals()
        {
            if (stopped || firstPass)
            {
                stopped = false;
                firstPass = false;
                lastRoutine = StartCoroutine(CycleMedals(Globals.CycleMedals));
            }
        }

        public void StopCycleMedals()
        {
            if (lastRoutine == null)
                return;

            stopped = true;
            //firstPass = false;
            StopCoroutine(lastRoutine);
        }
    }
}