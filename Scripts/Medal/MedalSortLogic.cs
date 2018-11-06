using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class MedalSortLogic : MonoBehaviour {

	public Dictionary<int, List<Medal>> medals_by_tier = new Dictionary<int, List<Medal>>();

	// TODO Call all the combination of settings on each separate coroutine and generate those lists
	public void SortManager(Dictionary<int, Medal> medals, SettingsManager settings)
	{
		if(!Globals.MultiplierFilter.SliderParent.isOn && !Globals.TierFilter.ToggleParent.isOn)
		{
            var medalByTier = Globals.MedalsTable.Where(x => Globals.TierFilter.ToggleChildrenActivated.Contains(x.Tier));
            var medalByMultAndTier = medalByTier.ToList().Where(x => CompareGuilt(x));

            foreach (var medal in medalByMultAndTier)
            {
                if (!medals_by_tier.ContainsKey(medal.Tier))
                {
                    medals_by_tier.Add(medal.Tier, new List<Medal>());
                }
                medals_by_tier[medal.Tier].Add(medal);
            }
		}
	}

    public bool CompareGuilt(Medal medal)
    {
        var newGuiltRange = medal.GuiltMultiplier.Replace("x", "").Split(new string[] { " - " }, StringSplitOptions.None);
        var highestGuilt = float.Parse(newGuiltRange[newGuiltRange.Length - 1]);

        return (highestGuilt >= Globals.MultiplierFilter.TempMinMax[0] &&
                highestGuilt <= Math.Abs(Globals.MultiplierFilter.TempMinMax[1]));
    }
}
