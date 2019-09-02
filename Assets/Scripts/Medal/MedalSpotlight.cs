using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace MedalViewer.Medal
{
    public class MedalSpotlight : MonoBehaviour
    {

        public MedalGraphViewManager MedalGraphViewManager;
        //MedalLogic medalLogic;

        void Start()
        {
            MedalGraphViewManager = GameObject.FindGameObjectWithTag("ScriptHolder").GetComponent<MedalGraphViewManager>();
        }

        public void HandleDisplay()
        {
            MedalGraphViewManager.HandleDisplay(gameObject);
        }

        //public void DisplayCurrentMedal()
        //{
        //    StartCoroutine(MedalSpotlightDisplayManager.Display(gameObject));
        //}
    }
}
