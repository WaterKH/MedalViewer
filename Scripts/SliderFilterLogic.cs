using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderFilterLogic : MonoBehaviour {

	public Toggle SliderParent;
	public Slider Min;
	public Slider Max;
	public Text MinText;
	public Text MaxText;
	public List<float> TempMinMax;
	public List<bool> TempIsOn;

	void Start()
	{
		Min.onValueChanged.AddListener(delegate { MinValueChanged(); });
		Max.onValueChanged.AddListener(delegate { MaxValueChanged(); });
	}

	public void AddSliders () 
	{
		TempIsOn.Add(SliderParent.isOn);

		TempIsOn.Add(Min.IsActive());
		TempIsOn.Add(Max.IsActive());

		TempMinMax.Add(Min.value);
		TempMinMax.Add(Max.value);
	}

	public void UpdateSliders()
	{
		TempIsOn[0] = SliderParent.isOn;
		TempIsOn[1] = Min.IsActive();
		TempIsOn[2] = Max.IsActive();

		TempMinMax[0] = Min.value;
		TempMinMax[1] = Max.value;
	}

	public void SetTempValues()
	{
		UpdateSliders();
	}

	public void OnValueChanged()
	{
		if(SliderParent.isOn)
		{
			TempIsOn[1] = true;
			TempIsOn[2] = true;
		}
		else
		{
			TempIsOn[1] = false;
			TempIsOn[2] = false;
		}
	}

	public void MinValueChanged()
	{
		if(Min.value > Mathf.Abs(Max.value))
		{
			Min.value = Mathf.Abs(Max.value);
		}

		MinText.text = Min.value.ToString();
	}

	public void MaxValueChanged()
	{
		if(Mathf.Abs(Max.value) < Min.value)
		{
			Max.value = -Min.value;
		}

		MaxText.text = Mathf.Abs(Max.value).ToString();
	}
}
