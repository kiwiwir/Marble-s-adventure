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

    [Header("Stats")]
    public int currentHealth;
    public int maxHealth;
    public int speed;
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
