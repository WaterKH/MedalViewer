using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public GameObject ContentTempParent;
    public GameObject[] ContentChildren;

    public Dictionary<int, RectTransform> vert_children = new Dictionary<int, RectTransform>();
	public Dictionary<int, RectTransform> hori_children = new Dictionary<int, RectTransform>();

    public List<Vector3> VerticalPositions = new List<Vector3>();
    public List<Vector3> HorizontalPositions = new List<Vector3>();

    private float yValueConstant = 500;
	private int xValueConstant = 2500;
    private Vector2 initialSizeDelta;

    public void Awake()
    {
        SetInitialPositions();

        initialSizeDelta = new Vector2(Content.sizeDelta.x, Content.sizeDelta.y);
    }
    
    public void SetInitialPositions()
    {
        foreach (var child in VerticalTempParent.GetComponentsInChildren<RectTransform>())
        {
            if (child.name != "Vertical_TEMP (Y)")
            {
                VerticalPositions.Add(child.position);
            }
        }

        foreach (var child in HorizontalTempParent.GetComponentsInChildren<RectTransform>())
        {
            if (child.name != "Horizontal_TEMP (X)")
            {
                HorizontalPositions.Add(child.position);
            }
        }
    }

	public void Initialize()
	{
        ResetContent();
        
	    //print("Vertical: " + VerticalParent.GetComponentsInChildren<Transform>().Length);
	    //print("Horizontal: " + HorizontalParent.GetComponentsInChildren<Transform>().Length);

        // Setting up the Y rows
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

        // Setting up the X columns
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

		//var initialPos = Placeholder.GetComponent<RectTransform>().position;
        //print("Initial Pos: " + initialPos);
		var yValue = 0.0f;
		var xValue = 0.0f;
		var tempHolder = VerticalParent.GetComponentsInChildren<RectTransform>();

		for(int i = tempHolder.Length - 1; i >= 0; --i)
		{
			if(tempHolder[i].name != "Vertical (Y)")
			{
				vert_children.Add(int.Parse(tempHolder[i].name), tempHolder[i]);
                //print(tempHolder[i].name + " " + tempHolder[i].position);
				tempHolder[i].position = new Vector2(0.0f, yValue + yValueConstant);
                //print(i + " " + tempHolder[i].position);
				tempHolder[i].GetComponent<Text>().enabled = true;
				yValue += yValueConstant;
			}
		}

		foreach(var x in HorizontalParent.GetComponentsInChildren<RectTransform>())
		{
			if(x.name != "Horizontal (X)")
			{
				hori_children.Add(int.Parse(x.name), x);
				x.position = new Vector2(xValue + xValueConstant, 0.0f);
				x.GetComponent<Text>().enabled = true;
				xValue += xValueConstant;
			}
		}

	    UpdateContent();
	}

	public void SetMedalHolderPosition(GameObject medalHolder, float guiltFloat, int tier)
	{
	    if (tier - 1 < 0)
	    {
            medalHolder.SetActive(false);
	        return;
	    }
	    var X_index = tier;
		var Y = guiltFloat;

		var Y_index = (int)Y;
		float Y_after_decimal = Y - Y_index;
	    
		// TODO Do we not already check for these? Why are they here?
		if(!Globals.TierFilter.ToggleChildren[X_index - 1].isOn)
		{
			if(Y_index <= Mathf.Abs(Globals.MultiplierFilter.Max.value) && Y_index >= Globals.MultiplierFilter.Min.value)
			{
				var Y_transform = vert_children[Y_index].position;
				var X_transform = hori_children[X_index].position;

                //print(Y_index + " " + vert_children[Y_index].transform.position);

				var new_transform = Vector3.zero;
			    var next_Y = yValueConstant;

                if (Y_index + 1 < vert_children.Count)
			    {
                    print("test");
			        next_Y = vert_children[Y_index + 1].position.y - Y_transform.y;
                    print(vert_children[Y_index + 1].name + " " + next_Y);
			    }
                //print(next_Y + " " + Y_after_decimal + " " +  Y_transform.y);
			    new_transform = new Vector3(X_transform.x, Y_transform.y + (next_Y * Y_after_decimal));
                
                medalHolder.GetComponent<RectTransform>().position = new_transform;
			    medalHolder.SetActive(true);
            }
			else
			{
				medalHolder.SetActive(false);
			}
		}
	}

    public void ResetContent()
    {
        Content.sizeDelta = initialSizeDelta;
        Content.localScale = new Vector3(1f, 1f, 1f);
        //print(Content.sizeDelta);
        vert_children.Clear();
        hori_children.Clear();
        foreach (var child in VerticalParent.GetComponentsInChildren<Transform>())
        {
            if (child.name != "Vertical (Y)")
            {
                child.SetParent(VerticalTempParent.transform);
                child.position = VerticalPositions[int.Parse(child.name) - 1];
            }
        }
        foreach (var child in HorizontalParent.GetComponentsInChildren<Transform>())
        {
            if (child.name != "Horizontal (X)")
            {
                child.SetParent(HorizontalTempParent.transform);
                child.position = HorizontalPositions[int.Parse(child.name) - 1];
            }
        }
    }

    // http://answers.unity.com/answers/1302142/view.html
    public void UpdateContent()
	{
	    
        //print("BEFORE: " + Content.sizeDelta);
        float yMin = 0.0f;
		float yMax = 0.0f;
		float xMin = 0.0f;
		float xMax = 0.0f;

		foreach (var kv in vert_children)
		{
		    var child = kv.Value;

		    if (child.name != "Vertical (Y)")
		    {

		        //if (yMin > child.offsetMin.y)
		        //{
		        //    print("Ymin: " + child.name + " " + child.offsetMin.y);
		        //}
		        //else if (yMax < child.offsetMax.y)
		        //{
		        //    print("Ymax: " + child.name + " " + child.offsetMax.y);
		        //}
                //foreach (var subchild in child.GetComponentsInChildren<RectTransform>())
                //{
                yMin = Mathf.Min(yMin, child.offsetMin.y);
		        yMax = Mathf.Max(yMax, child.offsetMax.y);
		        child.GetComponent<RowResizer>().SettingsChanged = true;
		    }
        }
		foreach (var kv in hori_children)
		{
		    var child = kv.Value;

		    if (child.name != "Horizontal (X)")
		    {
		        //if (xMin > child.offsetMin.x)
		        //{
		        //    print("Xmin: " + child.name + " " + child.offsetMin.x);
		        //}
		        //else if (xMax < child.offsetMax.x)
		        //{
		        //    print("Xmax: " + child.name + " " + child.offsetMax.x);
		        //}

		        //foreach (var subchild in child.GetComponentsInChildren<RectTransform>())
		        //{
		        xMin = Mathf.Min(xMin, child.offsetMin.x);
		        xMax = Mathf.Max(xMax, child.offsetMax.x);
		        //}
		    }
		}

		float finalSizeY = yMax - yMin;
		float finalSizeX = xMax - xMin;

	    //foreach (var child in ContentChildren)
	    //{
	    //    child.transform.SetParent(ContentTempParent.transform);
	    //}

        Content.sizeDelta = new Vector2 (finalSizeX, finalSizeY);
        Content.localScale = new Vector3(0.15f, 0.15f, 0.15f);
	    //print("AFTER: " + Content.sizeDelta);
	    //foreach (var child in ContentChildren)
	    //{
	    //    child.transform.SetParent(Content.transform);
	    //}
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
