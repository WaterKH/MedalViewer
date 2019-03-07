using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class KHUtilitiesx
{
	public static List<T> Splice<T>(this List<T> list, int index, int count)
	{
		List<T> range = list.GetRange(index, count);
		list.RemoveRange(index, count);
		return range;
	}

	public static string[] Splice(this string[] arr, int index, int count)
	{
		var list = arr.ToList();
	  	var range = list.GetRange(index, count);
	  	list.RemoveRange(index, count);
	  	return range.ToArray();
	}

    public static bool IsIn<T>(this T i, IEnumerable<T> ts)
    {
        foreach(var t in ts)
        {
            if(i.Equals(ts))
            {
                return true;
            }
        }

        return false;
    }

    public static void SetCanvasGroupActive(this CanvasGroup group)
    {
        group.interactable = true;
        group.blocksRaycasts = true;
        group.alpha = 1;
    }

    public static void SetCanvasGroupInactive(this CanvasGroup group)
    {
        group.interactable = false;
        group.blocksRaycasts = false;
        group.alpha = 0;
    }
}


