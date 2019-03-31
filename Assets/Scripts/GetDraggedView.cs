using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GetDraggedView : MonoBehaviour, IBeginDragHandler//, IDragHandler, IEndDragHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
            Globals.PointerObjectName = eventData.pointerDrag.name;
    }
}
