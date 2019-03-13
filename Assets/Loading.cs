using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Texture2D[] frames;
    public RawImage Image;

    float framesPerSecond = 30.0f;

    void Update()
    {
        int index = (int)(Time.time * framesPerSecond);
        index = index % frames.Length;
        Image.texture = frames[index];
    }
}
