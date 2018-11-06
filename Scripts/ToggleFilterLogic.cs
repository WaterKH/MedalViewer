using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleFilterLogic : MonoBehaviour {

	public Toggle ToggleParent;
	public List<Toggle> ToggleChildren;
	public List<bool> TempToggleBools;
    public List<int> ToggleChildrenActivated;

	public void AddToggles () 
	{
		TempToggleBools.Add(ToggleParent.isOn);

		foreach(var child in ToggleChildren)
		{
			TempToggleBools.Add(child.isOn);
		}
	}

	public void UpdateToggles()
	{
        ToggleChildrenActivated = new List<int>();

        TempToggleBools[0] = ToggleParent.isOn;

		for(int i = 1; i < TempToggleBools.Count; ++i)
		{
			TempToggleBools[i] = (ToggleChildren[i - 1].isOn);

            if(!ToggleChildren[i - 1].isOn)
            {
                ToggleChildrenActivated.Add(i);
            }
		}
	}

	public void SetTempValues()
	{
		UpdateToggles();
	}

	// NOTE!!!!! For some reason Unity doesn't allow you to flip this input in the inspector	
	// Thus, we are going to be reversing these booleans, so now isOn = isOff
	public void OnValueChanged()
	{
		if(ToggleParent.isOn)
		{
			foreach(var item in ToggleChildren)
			{
				item.isOn = true;
			}
		}
		else
		{
			foreach(var item in ToggleChildren)
			{
				item.isOn = false;
			}
		}
	}
}
