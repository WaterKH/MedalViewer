using MedalViewer.Medal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MedalViewer
{
    public class GetDraggedView : MonoBehaviour, IBeginDragHandler//, IDragHandler, IEndDragHandler
    {
        //public MedalSpotlightDisplayManager MedalSpotlightDisplayManager;
        public MedalGraphViewManager MedalGraphViewManager;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (MedalGraphViewManager.CurrSublistMedal != null)
                MedalGraphViewManager.HideSublistOfMedals(true);

            if (eventData.pointerDrag != null)
                MedalGraphViewManager.PointerObjectName = eventData.pointerDrag.name;
        }
    }
}