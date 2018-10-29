using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectionLogic : MonoBehaviour {

	public RectTransform SelectionBox;
	public ScrollRect Scroller;

	Vector3 startPos;
	Vector3 endPos;

	public GraphicRaycaster raycaster;
    PointerEventData pointer;
    public EventSystem eventSystem;

	// Use this for initialization
	void Start () 
	{
		SelectionBox.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKey(KeyCode.LeftControl))
		{
			if(Scroller.isActiveAndEnabled)
				Scroller.enabled = false;

			if(Input.GetMouseButtonDown(0))
			{
				startPos = Input.mousePosition;
				//startPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
				SelectionBox.gameObject.SetActive(true);

				Debug.Log("Start");
			}

			if(Input.GetMouseButtonUp(0))
			{
				SelectionBox.gameObject.SetActive(false);
				Debug.Log("Finished");
			}

			if(Input.GetMouseButton(0))
			{
				endPos = Input.mousePosition;

				var squareStart = startPos;

				var center = (squareStart + endPos) / 2f;

				var sizeX = Mathf.Abs(squareStart.x - endPos.x);
				var sizeY = Mathf.Abs(squareStart.y - endPos.y);

				SelectionBox.sizeDelta = new Vector2(sizeX, sizeY);


				SelectionBox.position = center;
			}
		}
		else
		{
			if(!Scroller.isActiveAndEnabled)
				Scroller.enabled = true;

			SelectionBox.gameObject.SetActive(false);
		}
	}
}
