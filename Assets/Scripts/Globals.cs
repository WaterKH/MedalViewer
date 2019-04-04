using System.Collections.Generic;
using MedalViewer.Medal;
using UnityEngine.EventSystems;
using UnityEngine;

public static class Globals
{
    #region Medals

    public static Dictionary<int, Medal> Medals = new Dictionary<int, Medal>();
    public static Dictionary<GameObject, int> CycleMedals = new Dictionary<GameObject, int>();
    public static Dictionary<int, Medal> SearchMedals = new Dictionary<int, Medal>();

    #endregion


    #region Settings

    public static ToggleFilterLogic TierFilter;
    public static ToggleFilterLogic PSM_URFilter;
    public static ToggleFilterLogic StarFilter;
    public static SliderFilterLogic MultiplierFilter;

    #endregion

    public static string PointerObjectName;
}
