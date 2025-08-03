using UnityEngine;
using UnityEngine.UI;

public class CanvasImageSwitch : MonoBehaviour
{
    public Image imageToHide;
    public Image imageToShow;
    public float delay = 16.5f;

    void Start()
    {
        Invoke("SwitchImages", delay);
    }

    void SwitchImages()
    {
        if (imageToHide != null)
            imageToHide.enabled = false;

        if (imageToShow != null)
            imageToShow.enabled = true;
    }
}
