using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace MedalViewer.Medal
{
    public class MedalCycleLogic : MonoBehaviour
    {
        MedalLogicManager MedalLogicManager;

        // This field can be accesed through our singleton instance,
        // but it can't be set in the inspector, because we use lazy instantiation
        public bool stopped;
        public bool firstPass = true;
    
        // Static singleton instance
        private static MedalCycleLogic instance;

        // Static singleton property
        public static MedalCycleLogic Instance
        {
            // Here we use the ?? operator, to return 'instance' if 'instance' does not equal null
            // otherwise we assign instance to a new component and return that
            get { return instance ?? (instance = new GameObject("MedalCycleLogic").AddComponent<MedalCycleLogic>()); }
        }

        void Start()
        {
            MedalLogicManager = GameObject.FindGameObjectWithTag("ScriptHolder").GetComponent<MedalLogicManager>();

            StartCoroutine(CycleMedals(Globals.CycleMedals));
        }

        // Instance method, this method can be accesed through the singleton instance
        public IEnumerator CycleMedals(Dictionary<GameObject, int> medals)
        {
            while (!stopped)
            {
                foreach (var m in medals)
                {
                    var currMedal = m.Key.transform.GetChild(m.Value);

                    m.Key.GetComponent<RawImage>().texture = currMedal.GetComponent<RawImage>().texture;
                }

                List<GameObject> keys = new List<GameObject>(medals.Keys);
                foreach (var key in keys)
                {
                    medals[key] = (medals[key] + 1) % key.transform.childCount;
                }

                yield return new WaitForSeconds(2.0f);
            }
        }

        public void StartCycleMedals()
        {
            stopped = false;
            firstPass = true;
            StartCoroutine(CycleMedals(Globals.CycleMedals));
        }

        public void StopCycleMedals()
        {
            stopped = true;
            firstPass = false;
        }
    }
}