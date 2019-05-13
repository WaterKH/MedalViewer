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

    public static string PointerObjectName;
    public static float OffsetY = 250;
    public static GameObject CurrSublistMedal;

    public static Dictionary<int, float> TierConversion = new Dictionary<int, float>
    {
        { 1, 1.25f }, { 2, 1.5f }, { 3, 2f }, { 4, 2.3f },
        { 5, 2.5f }, { 6, 2.8f }, { 7, 3f }, { 8, 3.3f },
        { 9, 3.8f }
    };
}
