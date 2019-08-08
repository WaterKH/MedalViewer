using MedalViewer.Medal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMovement : MonoBehaviour {

    public MedalGraphViewLogic MedalGraphViewLogic;
    //public GameObject MedalContent;
    public RectTransform MedalDisplay;
    public RectTransform OverlayY;
    public RectTransform OverlayX;


    #region public vars
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

	void Awake()
	{
        //min = 2530;
        //max = 11500;
        zoomValue = 0.1f;
        //InitialZoomScale = MedalDisplay.localScale;
        //InitialOverlayX = OverlayX.localScale;
        //InitialOverlayY = OverlayY.localScale;
    }

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
                MedalDisplay.localScale = new Vector2(x, y);

                //var overlayY1 = OverlayY.localScale.y + inputZoomCalculation;
                //var overlayX1 = OverlayY.localScale.x + inputZoomCalculation;
                OverlayY.localScale = new Vector2(x, y);

                //var overlayY2 = OverlayX.localScale.y + inputZoomCalculation;
                //var overlayX2 = OverlayX.localScale.x + inputZoomCalculation;
                OverlayX.localScale = new Vector2(x, y);
            }
            else
            {
                if (MedalDisplay.localScale.y > max)
                {
                    MedalDisplay.localScale = new Vector2(max, max);
                    OverlayY.localScale = new Vector2(max, max);
                    OverlayX.localScale = new Vector2(max, max);
                }
                else if (MedalDisplay.localScale.y < min)
                {
                    MedalDisplay.localScale = new Vector2(min, min);
                    OverlayY.localScale = new Vector2(min, min);
                    OverlayX.localScale = new Vector2(min, min);
                }
            }
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if(Input.GetKeyDown(KeyCode.Equals))
            {
                print("Pressed +");
                if (Globals.OffsetY + min > max)
                    Globals.OffsetY = max;
                else
                    Globals.OffsetY += min;

                ChangeYRowSize(Globals.OffsetY);
            }
            else if(Input.GetKeyDown(KeyCode.Minus))
            {
                print("Pressed -");
                if (Globals.OffsetY - min < min)
                    Globals.OffsetY = min;
                else
                    Globals.OffsetY -= min;

                ChangeYRowSize(Globals.OffsetY);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha0))
            {
                print("Pressed R");
                Globals.OffsetY = 250;
                ChangeYRowSize(Globals.OffsetY);
            }
        }
	}

    public void UpdateViewWindow(int value = 3680)
    {
        //MedalDisplayCanvasScaler.referenceResolution = new Vector2(MedalDisplayCanvasScaler.referenceResolution.x, value);
        //OverlayCanvasScaler.referenceResolution = new Vector2(OverlayCanvasScaler.referenceResolution.x, value);
    }

    public void ResetViewWindow()
    {
        MedalDisplay.localScale = InitialZoomScale;
        OverlayX.localScale = InitialOverlayX;
        OverlayY.localScale = InitialOverlayY;
    }

	public void ChangeYRowSize(float changeValue)
	{
        MedalGraphViewLogic.RowsY.ForEach(x => Destroy(x));
        MedalGraphViewLogic.RowsY.Clear();

        MedalGraphViewLogic.UpdateYRows(changeValue);

        MedalGraphViewLogic.PopulateMedals(false);
	}
}
