using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace MedalViewer.Medal
{
    public class MedalSpotlight : MonoBehaviour
    {

        public MedalSpotlightDisplayManager medalSpotlightDisplayManager;
        //MedalLogic medalLogic;

        // Use this for initialization
        void Start()
        {
            medalSpotlightDisplayManager = GameObject.FindGameObjectWithTag("MedalHighlight").GetComponent<MedalSpotlightDisplayManager>();
            //medalLogic = GameObject.FindGameObjectWithTag("ScriptHolder").GetComponent<MedalLogic>();
        }

        public void HandleDisplay()
        {
            medalSpotlightDisplayManager.HandleDisplay(gameObject);
        }

        public void DisplayCurrentMedal()
        {
            medalSpotlightDisplayManager.Display(gameObject);
        }
    }
}
