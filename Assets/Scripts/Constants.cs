using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Constants
{

    public static Dictionary<int, float> TierConversion = new Dictionary<int, float>
    {
        { 1, 1.25f }, { 2, 1.5f }, { 3, 2f }, { 4, 2.3f },
        { 5,  2.5f }, { 6, 2.8f }, { 7, 3f }, { 8, 3.3f },
        { 9,  3.8f }
    };
}
