using UnityEngine;
using UnityEngine.Localization.Settings;

[CreateAssetMenu(fileName = "New Item")]
public class ItemSO : ScriptableObject
{
    public string itemNamePL;
    public string itemNameENG;
    [TextArea] public string itemDescriptionPL;
    [TextArea] public string itemDescriptionENG;
    public Sprite icon;

    public bool isGold;
    public bool isUsable;

    public int stackSize = 99;
    public int basePrice = 0;

    [Header("Stats")]
    public int currentHealth;
    public int moveSpeed;
    public int sprintSpeed;
    public int damage;

    [Header("For Temporary Items")]
    public float duration;

    public string GetLocalizedName()
    {
        var locale = LocalizationSettings.SelectedLocale;
        if (locale == null)
        {
            Debug.LogWarning("No locale selected, falling back to English.");
            return itemNameENG;
        }

        var code = locale.Identifier.Code.ToLower();

        switch (code)
        {
            case "pl":
                return itemNamePL;
            case "en":
            default:
                return itemNameENG;
        }
    }

    public string GetLocalizedDescription()
    {
        var locale = LocalizationSettings.SelectedLocale;
        if (locale == null)
        {
            Debug.LogWarning("No locale selected, falling back to English.");
            return itemDescriptionENG;
        }

        var code = locale.Identifier.Code.ToLower();

        switch (code)
        {
            case "pl":
                return itemDescriptionPL;
            case "en":
            default:
                return itemDescriptionENG;
        }
    }
}
