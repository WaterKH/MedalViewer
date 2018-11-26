using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMedalUIManager : MonoBehaviour{

    public CanvasGroup CreateMedalUIGroup;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (CreateMedalUIGroup.alpha > 0)
	    {
	        if (Input.GetKeyDown(KeyCode.Escape))
	        {
	            CloseCreateMedalUI();
	        }
	    }
    }

    public void OpenCreateMedalUI()
    {
        CreateMedalUIGroup.alpha = 1;
        CreateMedalUIGroup.blocksRaycasts = true;
        CreateMedalUIGroup.interactable = true;
    }

    public void CloseCreateMedalUI()
    {
        CreateMedalUIGroup.alpha = 0;
        CreateMedalUIGroup.blocksRaycasts = false;
        CreateMedalUIGroup.interactable = false;
    }
}
