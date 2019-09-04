using MedalViewer.Medal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    #region public vars
    public MedalGraphViewManager MedalGraphViewManager;
    //public GameObject MedalContent;
    public RectTransform MedalDisplay;
    public RectTransform OverlayY;
    public RectTransform OverlayX;

    public float OffsetY = 250;

    //public Camera MainCamera;
    public MedalPositionLogic MedalPositionLogic;
    public Vector3 InitialZoomScale;
    public Vector3 InitialOverlayX;
    public Vector3 InitialOverlayY;
    ///public GameObject YRows;
    #endregion

    private float min = 0.2f;
    private float max = 1.25f;
    private float zoomValue;
    private float Test = 0.0f;

	void Awake()
    {
        zoomValue = 0.1f;
        //min = 2530;
        //max = 11500;
        //InitialZoomScale = MedalDisplay.localScale;
        //InitialOverlayX = OverlayX.localScale;
        //InitialOverlayY = OverlayY.localScale;
    }

    // TODO Rework UIMovement
	// Update is called once per frame
	void Update () 
	{
        var input = Input.GetAxis("Mouse ScrollWheel");

        if (input > 0 || input < 0)
        {
            if (MedalDisplay.localScale.y <= max && MedalDisplay.localScale.y >= min)
            {
                var inputZoomCalculation = (zoomValue * input);
                var y = MedalDisplay.localScale.y + inputZoomCalculation;
                var x = MedalDisplay.localScale.x + inputZoomCalculation;

                this.UpdateViewWindow(x, y);
            }
            else
            {
                if (MedalDisplay.localScale.y > max)
                    this.UpdateViewWindow(max, max);
                else if (MedalDisplay.localScale.y < min)
                    this.UpdateViewWindow(min, min);
            }
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if(Input.GetKeyDown(KeyCode.Equals))
            {
                print("Pressed +");
                if (OffsetY + min > max)
                    OffsetY = max;
                else
                    OffsetY += min;

                ChangeYRowSize(OffsetY);
            }
            else if(Input.GetKeyDown(KeyCode.Minus))
            {
                print("Pressed -");
                if (OffsetY - min < min)
                    OffsetY = min;
                else
                    OffsetY -= min;

                ChangeYRowSize(OffsetY);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha0))
            {
                print("Pressed R");
                OffsetY = 250;
                ChangeYRowSize(OffsetY);
            }
        }
	}

    //public void UpdateViewWindow(int value = 3680)
    //{
    //    //MedalDisplayCanvasScaler.referenceResolution = new Vector2(MedalDisplayCanvasScaler.referenceResolution.x, value);
    //    //OverlayCanvasScaler.referenceResolution = new Vector2(OverlayCanvasScaler.referenceResolution.x, value);
    //}

    public void ResetViewWindow()
    {
        MedalDisplay.localScale = InitialZoomScale;

        OverlayX.localScale = InitialOverlayX;
        OverlayY.localScale = InitialOverlayY;

        OffsetY = 250;
    }

    public void UpdateViewWindow(float x, float y)
    {
        MedalDisplay.localScale = new Vector2(x, y);

        OverlayY.localScale = new Vector2(x, y);
        OverlayX.localScale = new Vector2(x, y);
    }
    
	public void ChangeYRowSize(float changeValue)
	{
        MedalGraphViewManager.RowsY.ForEach(x => Destroy(x));
        MedalGraphViewManager.RowsY.Clear();

        MedalGraphViewManager.UpdateYRows(changeValue);

        MedalGraphViewManager.PopulateMedals(false);
	}
}
