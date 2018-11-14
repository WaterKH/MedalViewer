using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
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

	public Dictionary<int, RectTransform> vert_children = new Dictionary<int, RectTransform>();
	public Dictionary<int, RectTransform> hori_children = new Dictionary<int, RectTransform>();

	private float yValueConstant = 500;
	private int xValueConstant = 2500;

	public void Initialize()
	{
		vert_children.Clear();
		hori_children.Clear();
	    foreach (var child in VerticalParent.GetComponentsInChildren<Transform>())
	    {
	        if (child.name != "Vertical (Y)")
	        {
	            child.SetParent(VerticalTempParent.transform);
	        }
	    }
	    foreach (var child in HorizontalParent.GetComponentsInChildren<Transform>())
	    {
	        if (child.name != "Horizontal (X)")
	        {
	            child.SetParent(HorizontalTempParent.transform);
	        }
	    }

	    Console.WriteLine("Vertical: " + VerticalParent.GetComponentsInChildren<Transform>().Length);
	    Console.WriteLine("Horizontal: " + HorizontalParent.GetComponentsInChildren<Transform>().Length);

        foreach (var child in VerticalTempParent.GetComponentsInChildren<RectTransform>())
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
                //print(tempHolder[i].transform.position);
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

	public void SetMedalHolderPosition(GameObject medalHolder, float guiltFloat, int tier)
	{
		var X_index = tier;
		var Y = guiltFloat;

		var Y_index = (int)Y;
		float Y_after_decimal = Y - Y_index;
	    
		// TODO Do we not already check for these? Why are they here?
		if(!Globals.TierFilter.ToggleChildren[X_index - 1].isOn)
		{
			if(Y_index <= Mathf.Abs(Globals.MultiplierFilter.Max.value) && Y_index >= Globals.MultiplierFilter.Min.value)
			{
				var Y_transform = vert_children[Y_index].transform.position;
				var X_transform = hori_children[X_index].transform.position;

                //print(Y_index + " " + vert_children[Y_index].transform.position);

				var new_transform = Vector3.zero;
			    var next_Y = yValueConstant;

                if (Y_index + 1 < vert_children.Count)
			    {
			        next_Y = vert_children[Y_index + 1].transform.position.y - Y_transform.y;
			    }
                //print(next_Y + " " + Y_after_decimal + " " +  Y_transform.y);
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

    public void UpateMedalHolderPosition(GameObject medalHolder)
    {
        var multiplier = float.Parse(medalHolder.name);
        var lower = GameObject.Find(((int) multiplier).ToString()).transform.position.y;
        var upper = GameObject.Find((((int) multiplier) + 1).ToString()).transform.position.y;

        var percentage = multiplier - (int) multiplier;
        var position = (upper - lower) * percentage;

        medalHolder.transform.position = new Vector3(medalHolder.transform.position.x, lower + position);
    }
}
