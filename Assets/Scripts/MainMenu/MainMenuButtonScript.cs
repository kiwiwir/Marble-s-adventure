using UnityEngine;
using System.Collections;  
using UnityEngine.EventSystems;  
using UnityEngine.UI;
using TMPro;

public class MenuButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text buttonText;
    public Image hoverImage;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Color hexColor;
        if (ColorUtility.TryParseHtmlString("#dff8d0", out hexColor))
        {
            buttonText.color = hexColor;
        }
        if (hoverImage != null)
            hoverImage.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Color hexColor;
        if (ColorUtility.TryParseHtmlString("#87bd76", out hexColor))
        {
            buttonText.color = hexColor;
        }
        if (hoverImage != null)
            hoverImage.enabled = false;
    }
}
