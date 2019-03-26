using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterDisplayManager : MonoBehaviour
{
    public CanvasGroup FilterMenu;

    private bool isDisplayingFilters;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (this.isDisplayingFilters)
            {
                
            }
        }
    }

    public void DisplayFilterMenu()
    {

    }

    public void HideFilterMenu()
    {

    }
}
