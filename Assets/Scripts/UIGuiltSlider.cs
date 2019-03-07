using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuiltSlider : MonoBehaviour {

    public Image Slider;
    public RectTransform Button;

    private bool isClicking;
    
	// Update is called once per frame
	void Update () {
		if(isClicking)
        {
            MoveGuiltSlider();
        }
	}

    void MoveGuiltSlider()
    {
        //var dir = (Input.mousePosition - Slider.)
    }

    void OnMouseDown()
    {
        isClicking = true;
    }

    void OnMouseUp()
    {
        isClicking = false;
    }
}
