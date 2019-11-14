using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MedalViewer.Medal;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolTipManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject Tip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        var parent = eventData.pointerEnter;

        var medals = parent.GetComponentInChildren<GridLayoutGroup>().GetComponentsInChildren<MedalDisplay>().ToList();
        if (parent.tag != "SubContent")
        {
            var high = this.GetHighestMultiplier(medals);
            var low = this.GetLowestMultiplier(medals);

            Tip = medals.Count == 1 || (high == low || high == 0 || low == 0) ? Instantiate(Resources.Load("MultTip") as GameObject) : Instantiate(Resources.Load("RangeMultTip") as GameObject);

            Tip.transform.SetParent(parent.transform, false);
            Tip.GetComponent<RectTransform>().position = new Vector3(parent.GetComponent<RectTransform>().position.x, parent.GetComponent<RectTransform>().position.y + 75);

            var texts = Tip.GetComponentsInChildren<TextMeshProUGUI>();

            texts[0].text = low > 0 ? low.ToString(CultureInfo.InvariantCulture) : high.ToString(CultureInfo.InvariantCulture);
            if (high != low && high != 0 && low != 0)
                texts[1].text = high.ToString(CultureInfo.InvariantCulture);
            
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DestroyImmediate(Tip);
    }

    private double GetHighestMultiplier(List<MedalDisplay> medals)
    {
        var currHigh = 0.0;

        foreach (var medal in medals)
        {
            var currMult = GetHighestMultiplier(medal);
            if (currMult > currHigh)
                currHigh = currMult;
        }

        return currHigh;
    }

    private double GetHighestMultiplier(MedalDisplay medal)
    {
        var runningValue = 0.0;
        double.TryParse(medal.GuiltMultiplierHigh, out runningValue);
        
        if(runningValue == 0.0)
            double.TryParse(medal.GuiltMultiplierLow, out runningValue);

        if (runningValue == 0.0)
            double.TryParse(medal.MaxMultiplierHigh, out runningValue);

        if (runningValue == 0.0)
            double.TryParse(medal.MaxMultiplierLow, out runningValue);

        if (runningValue == 0.0)
            double.TryParse(medal.BaseMultiplierHigh, out runningValue);

        if (runningValue == 0.0)
            double.TryParse(medal.BaseMultiplierLow, out runningValue);

        return runningValue;
    }

    private double GetLowestMultiplier(List<MedalDisplay> medals)
    {
        var currLow = 0.0;

        foreach (var medal in medals)
        {
            var currMult = GetLowestMultiplier(medal);
            if (currMult < currLow)
                currLow = currMult;
        }

        return currLow;
    }

    private double GetLowestMultiplier(MedalDisplay medal)
    {
        var runningValue = 0.0;
        double.TryParse(medal.BaseMultiplierLow, out runningValue);

        if (runningValue == 0.0)
            double.TryParse(medal.BaseMultiplierHigh, out runningValue);

        if (runningValue == 0.0)
            double.TryParse(medal.MaxMultiplierLow, out runningValue);

        if (runningValue == 0.0)
            double.TryParse(medal.MaxMultiplierHigh, out runningValue);

        if (runningValue == 0.0)
            double.TryParse(medal.GuiltMultiplierLow, out runningValue);

        if (runningValue == 0.0)
            double.TryParse(medal.GuiltMultiplierHigh, out runningValue);

        return runningValue;
    }
}
