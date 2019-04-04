using MedalViewer.Medal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMovement : MonoBehaviour {

    public GameObject MedalContent;

	#region public vars
	public Camera MainCamera;
    public MedalPositionLogic MedalPositionLogic;

	public GameObject YRows;
	#endregion

	#region private vars
	private float max;
	private float min;

    private float zoomValue;

	private bool isFilterDisplay;
	private bool isSearchDisplay;
	#endregion

	void Awake()
	{
        //MainCamera.transform.position = new Vector3(MainCamera.transform.position.x, MainCamera.transform.position.y, -2500);

        min = 50;
        max = 110;
        zoomValue = 500;

        //MainCamera.fieldOfView = 60;
	}

	// Update is called once per frame
	void Update () 
	{
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            var offsetMax = MedalContent.GetComponent<RectTransform>().offsetMax;
            MedalContent.GetComponent<RectTransform>().offsetMax = new Vector2(offsetMax.x + zoomValue, offsetMax.y + zoomValue);
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            var offsetMax = MedalContent.GetComponent<RectTransform>().offsetMax;
            MedalContent.GetComponent<RectTransform>().offsetMax = new Vector2(offsetMax.x - zoomValue, offsetMax.y - zoomValue);
        }

        //if(Input.GetAxis("Mouse ScrollWheel") > 0)
        //{
        //    if(MainCamera.fieldOfView > min)
        //    {
        //        MainCamera.fieldOfView -= zoomValue;
        //    }
        //    else
        //    {
        //        MainCamera.fieldOfView = min;
        //    }
        //}
        //else if(Input.GetAxis("Mouse ScrollWheel") < 0)
        //{
        //    if (MainCamera.fieldOfView < max)
        //    {
        //        MainCamera.fieldOfView += zoomValue;
        //    }
        //    else
        //    {
        //        MainCamera.fieldOfView = max;
        //    }
        //}
	}

	public void ChangeYRowSize(Scrollbar scroller)
	{
		var localScale = YRows.transform.localScale;
		YRows.transform.localScale = new Vector2(1, scroller.value);
		var children = YRows.GetComponentsInChildren<Text>();
		foreach(var child in children)
		{
			var childScale = child.transform.localScale;
			child.transform.localScale = new Vector2(1, float.Parse("1" + (1 - scroller.value).ToString().Substring(1)));
		}
	}
}
