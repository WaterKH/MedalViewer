using UnityEngine;

public class CreditsDisplayManager : MonoBehaviour
{
    public CanvasGroup Credits;
    public CanvasGroup Extras;

    public void ShowCredits()
    {
        Credits.SetCanvasGroupActive();
    }

    public void HideCredits()
    {
        Credits.SetCanvasGroupInactive();
        HideExtras();
    }

    public void ShowExtras()
    {
        Extras.SetCanvasGroupActive();
    }

    public void HideExtras()
    {
        Extras.SetCanvasGroupInactive();
    }

    public void OpenUrl(string url)
    {
        Application.OpenURL(url);
    }
}
