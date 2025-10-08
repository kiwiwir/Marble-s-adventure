/*using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    [Header("Panels")]
    public CanvasGroup infoPanelWithStats;
    public CanvasGroup infoPanelSimple;

    [Header("Texts")]
    public CanvasGroup infoPanel;
    public TMP_Text itemNameText;
    public TMP_Text itemDescriptionText;

    [Header("Stat Fields")]
    public TMP_Text[] statTexts;
    //private RectTransform infoPanelRect;
    private RectTransform currentPanelRect;

    private void Awake()
    {
        //infoPanelRect = GetComponent<RectTransform>();
        currentPanelRect = infoPanelWithStats.GetComponent<RectTransform>();
    }

    public void ShowItemInfo(ItemSO itemSO)
    {
        infoPanel.alpha = 1;

        itemNameText.text = itemSO.GetLocalizedName();
        itemDescriptionText.text = itemSO.GetLocalizedDescription();

        List<string> stats = new List<string>();
        if (itemSO.currentHealth > 0)
            stats.Add("Heal " + itemSO.currentHealth.ToString() + " Health.");
        if (itemSO.moveSpeed > 0 && itemSO.sprintSpeed > 0)
            stats.Add("Gain " + itemSO.moveSpeed.ToString() + " Speed.");
        if (itemSO.damage > 0)
            stats.Add("Damage: " + itemSO.damage.ToString());
        if (itemSO.duration > 0)
            stats.Add("Duration: " + itemSO.duration.ToString() + "s");

        if (stats.Count <= 0)
            return;

        for(int i = 0; i < statTexts.Length; i++)
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
    public void HideItemInfo()
    {
        infoPanel.alpha = 0;

        itemNameText.text = "";
        itemDescriptionText.text = "";
    }
    public void FollowMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 offset = new Vector3(15, 30, 0);

        infoPanelRect.position = mousePosition + offset;
    }
}
*/
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    [Header("Panels")]
    public CanvasGroup infoPanelWithStats;
    public CanvasGroup infoPanelSimple;

    [Header("Texts - With Stats")]
    public TMP_Text itemNameTextWithStats;
    public TMP_Text itemDescriptionTextWithStats;
    public TMP_Text[] statTexts;

    [Header("Texts - Simple")]
    public TMP_Text itemNameTextSimple;
    public TMP_Text itemDescriptionTextSimple;

    private RectTransform currentPanelRect;

    private void Awake()
    {
        // możesz przypisać dowolny na start, aktualizuje się dynamicznie
        currentPanelRect = infoPanelWithStats.GetComponent<RectTransform>();
    }

    public void ShowItemInfo(ItemSO itemSO)
    {
        bool hasStats = itemSO.currentHealth > 0 || 
                        itemSO.moveSpeed > 0 || 
                        itemSO.sprintSpeed > 0 || 
                        itemSO.damage > 0 || 
                        itemSO.duration > 0;

        // ukryj oba panele najpierw
        HideItemInfo();

        if (hasStats)
        {
            // pokaz panel z statami
            infoPanelWithStats.alpha = 1;
            infoPanelWithStats.blocksRaycasts = true;
            currentPanelRect = infoPanelWithStats.GetComponent<RectTransform>();

            itemNameTextWithStats.text = itemSO.GetLocalizedName();
            itemDescriptionTextWithStats.text = itemSO.GetLocalizedDescription();

            // dodaj staty
            List<string> stats = new List<string>();
            if (itemSO.currentHealth > 0)
                stats.Add("Heal " + itemSO.currentHealth.ToString() + " Health.");
            if (itemSO.moveSpeed > 0 && itemSO.sprintSpeed > 0)
                stats.Add("Gain " + itemSO.moveSpeed.ToString() + " Speed.");
            if (itemSO.damage > 0)
                stats.Add("Damage: " + itemSO.damage.ToString());
            if (itemSO.duration > 0)
                stats.Add("Duration: " + itemSO.duration.ToString() + "s");

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
            itemDescriptionTextSimple.text = itemSO.GetLocalizedDescription();
        }
    }

    public void HideItemInfo()
    {
        infoPanelWithStats.alpha = 0;
        infoPanelSimple.alpha = 0;
        infoPanelWithStats.blocksRaycasts = false;
        infoPanelSimple.blocksRaycasts = false;
    }

    public void FollowMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 offset = new Vector3(15, 30, 0);

        if (currentPanelRect != null)
            currentPanelRect.position = mousePosition + offset;
    }
}
