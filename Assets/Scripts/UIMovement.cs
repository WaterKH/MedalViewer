using MedalViewer.Medal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMovement : MonoBehaviour {

    public MedalGraphViewLogic MedalGraphViewLogic;
    //public GameObject MedalContent;
    public CanvasScaler MedalDisplayCanvasScaler;
    public CanvasScaler OverlayCanvasScaler;


    #region public vars
    //public Camera MainCamera;
    public MedalPositionLogic MedalPositionLogic;

	///public GameObject YRows;
    #endregion

    private int min = 50;
    private int max = 600;

	void Awake()
	{
        //min = 2530;
        //max = 11500;
        //zoomValue = 2000;
	}

	// Update is called once per frame
	void Update () 
	{
        //var input = Input.GetAxis("Mouse ScrollWheel");

        //if (input > 0 || input < 0)
        //{
        //    MedalDisplayCanvasScaler.referenceResolution = new Vector2(MedalDisplayCanvasScaler.referenceResolution.x,
        //                                                               MedalDisplayCanvasScaler.referenceResolution.y + (zoomValue * -input));

        //    OverlayCanvasScaler.referenceResolution = new Vector2(OverlayCanvasScaler.referenceResolution.x,
        //                                                          OverlayCanvasScaler.referenceResolution.y + (zoomValue * -input));
        //}

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

	public void ChangeYRowSize(int changeValue)
	{
        MedalGraphViewLogic.RowsY.ForEach(x => Destroy(x));
        MedalGraphViewLogic.RowsY.Clear();

        MedalGraphViewLogic.UpdateYRows(changeValue);

        MedalGraphViewLogic.PopulateMedals(false);
	}
}
