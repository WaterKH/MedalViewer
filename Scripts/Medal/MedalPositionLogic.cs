﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedalPositionLogic : MonoBehaviour {

	public GameObject main_parent;
	public GameObject VerticalParent;
	public GameObject HorizontalParent;
	public GameObject VerticalTempParent;
	public GameObject HorizontalTempParent;

	public GameObject Placeholder;
	public RectTransform Content;

	private Dictionary<int, RectTransform> vert_children = new Dictionary<int, RectTransform>();
	private Dictionary<int, RectTransform> hori_children = new Dictionary<int, RectTransform>();

	private int yValueConstant = 500;
	private int xValueConstant = 2500;

	public void Initialize()
	{
		vert_children.Clear();
		hori_children.Clear();

		foreach(var child in VerticalTempParent.GetComponentsInChildren<RectTransform>())
		{
			
			if(child.name != "Vertical_TEMP (Y)")
			{
				if(int.Parse(child.name) >= Globals.MultiplierFilter.Min.value &&
					int.Parse(child.name) <= Mathf.Abs(Globals.MultiplierFilter.Max.value))
				{
					child.SetParent(VerticalParent.transform);
				}
				
			}
		}

		foreach(var child in HorizontalTempParent.GetComponentsInChildren<RectTransform>())
		{
			if(child.name != "Horizontal_TEMP (X)")
			{
				if(!Globals.TierFilter.ToggleChildren[int.Parse(child.name) - 1].isOn)
				{
					child.SetParent(HorizontalParent.transform);
				}
			}
		}

		var initialPos = Placeholder.GetComponent<RectTransform>().anchoredPosition;
		var yValue = initialPos.y;
		var xValue = initialPos.x;
		var tempHolder = VerticalParent.GetComponentsInChildren<RectTransform>();

		for(int i = tempHolder.Length - 1; i >= 0; --i)
		{
			if(tempHolder[i].name != "Vertical (Y)")
			{
				vert_children.Add(int.Parse(tempHolder[i].name), tempHolder[i]);
				tempHolder[i].position = new Vector2(initialPos.x, yValue + yValueConstant);
				tempHolder[i].GetComponent<Text>().enabled = true;
				yValue += yValueConstant;
			}
		}

		foreach(var x in HorizontalParent.GetComponentsInChildren<RectTransform>())
		{
			if(x.name != "Horizontal (X)")
			{
				hori_children.Add(int.Parse(x.name), x);
				x.position = new Vector2(xValue + xValueConstant, initialPos.y);
				x.GetComponent<Text>().enabled = true;
				xValue += xValueConstant;
			}
		}

		UpdateContent();
	}

	public void SetMedalHolderPosition(GameObject medalHolder, float guilt_float, int tier)
	{
		var X_index = tier;
		var Y = guilt_float;

		var Y_index = (int)Y;
		float Y_after_decimal = Y - Y_index;

		// TODO Do we not already check for these? Why are they here?
		if(!Globals.TierFilter.ToggleChildren[X_index - 1].isOn)
		{
			if(Y_index <= Mathf.Abs(Globals.MultiplierFilter.Max.value) && Y_index >= Globals.MultiplierFilter.Min.value)
			{
				var Y_transform = vert_children[Y_index + 1].transform.position;
				var X_transform = hori_children[X_index].transform.position;
//				print(vert_children[Y_index].name + " " + medalHolder.name);

				var new_transform = Vector3.zero;
				var next_Y = yValueConstant;

				new_transform = new Vector3(X_transform.x, Y_transform.y + (next_Y * Y_after_decimal));

				medalHolder.transform.position = new_transform;
			}
			else
			{
				medalHolder.SetActive(false);
			}
		}
	}

	// http://answers.unity.com/answers/1302142/view.html
	public void UpdateContent()
	{
		float yMin = 0.0f;
		float yMax = 0.0f;
		float xMin = 0.0f;
		float xMax = 0.0f;

		foreach (var child in VerticalParent.GetComponentsInChildren<RectTransform>())
		{
			yMin = Mathf.Min (yMin, child.offsetMin.y);
			yMax = Mathf.Max (yMax, child.offsetMax.y);
		}
		foreach (var child in HorizontalParent.GetComponentsInChildren<RectTransform>()) 
		{
			xMin = Mathf.Min (xMin, child.offsetMin.x);
			xMax = Mathf.Max (xMax, child.offsetMax.x);
		}

		float finalSizeY = yMax - yMin;
		float finalSizeX = xMax - xMin;

		Content.sizeDelta = new Vector2 (finalSizeX, finalSizeY);
	}
}
