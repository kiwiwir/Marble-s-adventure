using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class ItemInfo : MonoBehaviour
{
    [Header("Panels")]
    public CanvasGroup infoPanelWithStats;
    public CanvasGroup infoPanelSimple;
    public CanvasGroup infoPanelDescription;

    [Header("Texts - With Stats")]
    public TMP_Text itemNameTextWithStats;
    public TMP_Text[] statTexts;

    [Header("Texts - Simple")]
    public TMP_Text itemNameTextSimple;

    [Header("Texts - Description")]
    public TMP_Text descriptionText;
    
    private RectTransform currentPanelRect;

    private void Awake()
    {
        currentPanelRect = infoPanelWithStats.GetComponent<RectTransform>();
    }

    public void ShowItemInfo(ItemSO itemSO)
    {
        bool hasStats = itemSO.currentHealth > 0 || 
                        itemSO.moveSpeed > 0 || 
                        itemSO.sprintSpeed > 0 || 
                        itemSO.damage > 0 || 
                        itemSO.duration > 0;

        HideItemInfo();

        if (hasStats)
        {
            infoPanelWithStats.alpha = 1;
            infoPanelWithStats.blocksRaycasts = true;
            currentPanelRect = infoPanelWithStats.GetComponent<RectTransform>();

            itemNameTextWithStats.text = itemSO.GetLocalizedName();

            List<string> stats = new List<string>();
            string langCode = LocalizationSettings.SelectedLocale.Identifier.Code; // np. "en" albo "pl"

            if (itemSO.currentHealth > 0)
            {
                stats.Add(langCode == "pl"
                    ? $"Leczy o {itemSO.currentHealth}."
                    : $"Heal {itemSO.currentHealth} Health.");
            }

            if (itemSO.moveSpeed > 0 && itemSO.sprintSpeed > 0)
            {
                stats.Add(langCode == "pl"
                    ? $"Zwiększa prędkość o {itemSO.moveSpeed}."
                    : $"Gain {itemSO.moveSpeed} Speed.");
            }

            if (itemSO.damage > 0)
            {
                stats.Add(langCode == "pl"
                    ? $"Obrażenia: {itemSO.damage}"
                    : $"Damage: {itemSO.damage}");
            }

            if (itemSO.duration > 0)
            {
                stats.Add(langCode == "pl"
                    ? $"Czas trwania: {itemSO.duration}s"
                    : $"Duration: {itemSO.duration}s");
            }

            for (int i = 0; i < statTexts.Length; i++)
            {
                if (i < stats.Count)
                {
                    statTexts[i].text = stats[i];
                    statTexts[i].gameObject.SetActive(true);
                }
                else
                {
                    statTexts[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            // pokaz prosty panel
            infoPanelSimple.alpha = 1;
            infoPanelSimple.blocksRaycasts = true;
            currentPanelRect = infoPanelSimple.GetComponent<RectTransform>();

            itemNameTextSimple.text = itemSO.GetLocalizedName();
        }
    }
    public void ShowDescription(string description)
    {
        HideItemInfo(); // ukryj inne panele

        if (descriptionText != null && infoPanelDescription != null)
        {
            descriptionText.text = description;
            infoPanelDescription.alpha = 1;
            infoPanelDescription.blocksRaycasts = true;
            currentPanelRect = infoPanelDescription.GetComponent<RectTransform>();
        }
    }
    public void HideItemInfo()
    {
        infoPanelWithStats.alpha = 0;
        infoPanelSimple.alpha = 0;
        infoPanelDescription.alpha = 0;

        infoPanelWithStats.blocksRaycasts = false;
        infoPanelSimple.blocksRaycasts = false;
        infoPanelDescription.blocksRaycasts = false;
    }

    public void FollowMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 offset = new Vector3(15, 30, 0);

        if (currentPanelRect != null)
            currentPanelRect.position = mousePosition + offset;
    }
}
