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
                HideFilterMenu();
            }
        }
    }

    public void DisplayFilterMenu()
    {
        isDisplayingFilters = true;

        FilterMenu.alpha = 1;
        FilterMenu.interactable = true;
        FilterMenu.blocksRaycasts = true;
    }

    public void HideFilterMenu()
    {
        isDisplayingFilters = false;

        FilterMenu.alpha = 0;
        FilterMenu.interactable = false;
        FilterMenu.blocksRaycasts = false;
    }
}
