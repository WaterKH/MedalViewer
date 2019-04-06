using MedalViewer.Medal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MedalViewer
{
    public class GetDraggedView : MonoBehaviour, IBeginDragHandler//, IDragHandler, IEndDragHandler
    {
        public MedalSpotlightDisplayManager MedalSpotlightDisplayManager;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (Globals.CurrSublistMedal != null)
                MedalSpotlightDisplayManager.HideSublistOfMedals(true);

            if (eventData.pointerDrag != null)
                Globals.PointerObjectName = eventData.pointerDrag.name;
        }
    }
}