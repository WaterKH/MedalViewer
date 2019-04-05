using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MedalSublistHandler : MonoBehaviour, IPointerClickHandler
{
    public GameObject ClickedMedal;
    public CanvasGroup SublistCanvasGroup;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(SublistCanvasGroup.alpha == 1)
        {
            if(ClickedMedal != eventData.pointerPress)
            {
                SublistCanvasGroup.SetCanvasGroupInactive();
            }
        }
        else
        {
            //ClickedMedal = eventData.lastPress;
        }
    }
}
