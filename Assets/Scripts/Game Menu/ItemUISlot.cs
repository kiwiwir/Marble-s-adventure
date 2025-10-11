using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class ItemUISlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Item Data")]
    public ItemSO itemSO; // przypisany przedmiot

    [Header("UI References")]
    public Image itemImage;
    [SerializeField] private ItemDescription itemDescription; // panel z opisem

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (itemSO != null)
        {
            itemImage.sprite = itemSO.icon;
            itemImage.gameObject.SetActive(true);
        }
        else
        {
            itemImage.gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemSO != null)
        {
            string description = itemSO.GetLocalizedDescription();
            itemDescription.ShowDescription(description);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemDescription.HideItemInfo();
    }
}
