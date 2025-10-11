using UnityEngine;
using TMPro;

public class ItemDescription : MonoBehaviour
{
    [SerializeField] private GameObject background; // tło obrazka
    [SerializeField] private TMP_Text descriptionText; // pole tekstowe TMP

    private void Awake()
    {
        HideItemInfo(); // ukryj na start
    }

    public void ShowDescription(string description)
    {
        descriptionText.text = description;
        background.SetActive(true);
        descriptionText.gameObject.SetActive(true);
    }

    public void HideItemInfo()
    {
        background.SetActive(false);
        descriptionText.gameObject.SetActive(false);
    }
}
