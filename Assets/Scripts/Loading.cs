using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public CanvasGroup LoadingGroup;
    public Texture2D[] frames;
    public RawImage Image;
    public bool IsLoading;

    float framesPerSecond = 40.0f;

    void Update()
    {
        //int index = (int)(Time.time * framesPerSecond);
        //index = index % frames.Length;
        //Image.texture = frames[index];

    }

    IEnumerator InitiateLoading()
    {
        while (IsLoading)
        {
            //print(IsLoading);
            int index = (int)(Time.time * framesPerSecond);
            index %= frames.Length;
            Image.texture = frames[index];
            yield return null;
        }
    }

    public void StartLoading()
    {
        LoadingGroup.alpha = 1;
        LoadingGroup.blocksRaycasts = true;
        LoadingGroup.interactable = true;

        IsLoading = true;
        StartCoroutine(InitiateLoading());
    }

    public void FinishLoading()
    {
        LoadingGroup.alpha = 0;
        LoadingGroup.blocksRaycasts = false;
        LoadingGroup.interactable = false;

        IsLoading = false;
    }
}
