using MedalViewer.Medal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMovement : MonoBehaviour {

	#region public vars
	public Camera MainCamera;
    public MedalPositionLogic MedalPositionLogic;
    //public GameObject Background;

	public GameObject YRows;
	#endregion

	#region private vars
	private float max;
	private float min;

	private bool isFilterDisplay;
	private bool isSearchDisplay;
	#endregion

	void Awake()
	{
		//max = 1.0f;
		//min = 0.08f;

	    min = -2500;
	    max = -5000;

	    MainCamera.transform.position = new Vector3(MainCamera.transform.position.x, MainCamera.transform.position.y, -2500);
	    //Content.transform.localScale = Vector3.one * min;
	    //Content.transform.position = new Vector2(-406.9875f, -57241.6f);
	    //Background.transform.position = Content.transform.position;
	}

	// Update is called once per frame
	void Update () 
	{
		//Content.transform.position = Background.transform.position;
		//Background.transform.localScale = Content.transform.localScale;
        
		float zoomValue = Input.GetAxis("Mouse ScrollWheel") * 20000;
 
		if (zoomValue != 0)
		{
            if (Mathf.Abs(MainCamera.transform.position.z) >= Mathf.Abs(min) && Mathf.Abs(MainCamera.transform.position.z) <= Mathf.Abs(max))
            {
                //Content.transform.localScale += Vector3.one * zoomValue * Time.deltaTime / 2;
                //Content.transform.position -= Vector3.one * zoomValue * Time.deltaTime / 2;
                MainCamera.transform.position += new Vector3(0, 0, zoomValue * Time.deltaTime);


                if (Mathf.Abs(MainCamera.transform.position.z) <= Mathf.Abs(min))
                {
                    MainCamera.transform.position = new Vector3(MainCamera.transform.position.x, MainCamera.transform.position.y, min);
                }
                else if (Mathf.Abs(MainCamera.transform.position.z) >= Mathf.Abs(max))
                {
                    MainCamera.transform.position = new Vector3(MainCamera.transform.position.x, MainCamera.transform.position.y, max);
                }

                //MedalPositionLogic.UpdateContent();
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
