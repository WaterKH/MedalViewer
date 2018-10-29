using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMovement : MonoBehaviour {

	#region public vars
	public GameObject Content;
	//public GameObject Background;
	public Camera uiCamera;

	public GameObject YRows;
	#endregion

	#region private vars
	private float max;
	private float min;

	private bool isFilterDisplay;
	private bool isSearchDisplay;
	#endregion

	void Start()
	{
		max = 0.75243f;
		min = 0.1675405f;

		Content.transform.localScale = Vector3.one * min;
		//Content.transform.position = new Vector2(-406.9875f, -57241.6f);
		//Background.transform.position = Content.transform.position;
	}

	// Update is called once per frame
	void Update () 
	{
		//Content.transform.position = Background.transform.position;
		//Background.transform.localScale = Content.transform.localScale;


		float zoomValue = Input.GetAxis("Mouse ScrollWheel");
 
		if (zoomValue != 0)
		{
			if(Content.transform.localScale.x >= min && Content.transform.localScale.x <= max)
			{
				Content.transform.localScale += Vector3.one * zoomValue * Time.deltaTime / 2;
				Content.transform.position -= Vector3.one * zoomValue * Time.deltaTime / 2;

				if(Content.transform.localScale.x >= max)
				{
					Content.transform.localScale = Vector3.one * max;
				}
				else if(Content.transform.localScale.x <= min)
				{
					Content.transform.localScale = Vector3.one * min;
				}
			}
		}
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
