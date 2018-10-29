using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour {

	public MedalCreator medalCreator;
	public MedalSortLogic Sorter;
	public CanvasGroup FilterGroup;

	public ToggleFilterLogic TierFilter;
	public ToggleFilterLogic PSM_URFilter;
	public ToggleFilterLogic StarFilter;
	public SliderFilterLogic MultiplierFilter;

	// Use this for initialization
	void Awake () 
	{
		this.SetupSettings();
		this.Default();
		this.CloseFilter();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(FilterGroup.alpha > 0)
		{
			if(Input.GetKeyDown(KeyCode.Escape))
			{
				CancelChanges();
				CloseFilter();
			}
		}
	}

	public void OpenFilter()
	{
		FilterGroup.alpha = 1;
		FilterGroup.blocksRaycasts = true;
		FilterGroup.interactable = true;

		TierFilter.SetTempValues();
		PSM_URFilter.SetTempValues();
		StarFilter.SetTempValues();
		MultiplierFilter.SetTempValues();
	}

	public void CloseFilter()
	{
		FilterGroup.alpha = 0;
		FilterGroup.blocksRaycasts = false;
		FilterGroup.interactable = false;
	}

	public void AcceptChanges()
	{
		this.UpdateSettings();
		// TODO Setup a class with all settings... May be easier to handle
		Sorter.SortManager(medalCreator.medals, this);

		CloseFilter();
	}

	public void CancelChanges()
	{
		PSM_URFilter.ToggleParent.isOn = PSM_URFilter.TempToggleBools[0];
		for(int i = 1; i < PSM_URFilter.TempToggleBools.Count; ++i)
			PSM_URFilter.ToggleChildren[i - 1].isOn = PSM_URFilter.TempToggleBools[i];

		TierFilter.ToggleParent.isOn = TierFilter.TempToggleBools[0];
		for(int i = 1; i < TierFilter.TempToggleBools.Count; ++i)
			TierFilter.ToggleChildren[i - 1].isOn = TierFilter.TempToggleBools[i];

		StarFilter.ToggleParent.isOn = StarFilter.TempToggleBools[0];
		for(int i = 1; i < StarFilter.TempToggleBools.Count; ++i)
			StarFilter.ToggleChildren[i - 1].isOn = StarFilter.TempToggleBools[i];


		MultiplierFilter.SliderParent.isOn = MultiplierFilter.TempIsOn[0];
		MultiplierFilter.Min.interactable = MultiplierFilter.TempIsOn[1];
		MultiplierFilter.Max.interactable = MultiplierFilter.TempIsOn[2];

		MultiplierFilter.Min.value = MultiplierFilter.TempMinMax[0];
		MultiplierFilter.Max.value = MultiplierFilter.TempMinMax[1];

		CloseFilter();
	}

	public void Default()
	{
		PSM_URFilter.ToggleParent.isOn = true;
		PSM_URFilter.OnValueChanged();

		TierFilter.ToggleParent.isOn = false;
		for(int i = 0; i < 4; ++i)
		{
			TierFilter.ToggleChildren[i].isOn = true;
		}
		for(int i = 4; i < TierFilter.ToggleChildren.Count; ++i)
		{
			TierFilter.ToggleChildren[i].isOn = false;
		}

		StarFilter.ToggleParent.isOn = false;
		for(int i = 0; i < 5; ++i)
		{
			StarFilter.ToggleChildren[i].isOn = true;
		}

		StarFilter.ToggleChildren[5].isOn = false;
		StarFilter.ToggleChildren[6].isOn = false;

		MultiplierFilter.SliderParent.isOn = false;
		MultiplierFilter.Min.value = 30;
		MultiplierFilter.Max.value = -77;
		MultiplierFilter.MinValueChanged();
		MultiplierFilter.MaxValueChanged();

		UpdateSettings();
	}

	public void SetupSettings()
	{
        Globals.TierFilter = TierFilter;
        Globals.PSM_URFilter = PSM_URFilter;
        Globals.StarFilter = StarFilter;
        Globals.MultiplierFilter = MultiplierFilter;

        TierFilter.AddToggles();
		PSM_URFilter.AddToggles();
		StarFilter.AddToggles();
		MultiplierFilter.AddSliders();
	}

	public void UpdateSettings()
	{
        TierFilter.UpdateToggles();
		StarFilter.UpdateToggles();
		PSM_URFilter.UpdateToggles();
		MultiplierFilter.UpdateSliders();
	}
}
