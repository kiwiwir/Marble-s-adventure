using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

[CreateAssetMenu(fileName = "QuestSO", menuName = "QuestSO")]
public class QuestSO : ScriptableObject
{
    [Header("Localized Quest Names")]
    public string questNamePL;
    public string questNameENG;

    [Header("Localized Quest Description")]
    [TextArea] public string questDescriptionPL;
    [TextArea] public string questDescriptionENG;

    public int questLevel;

    public List <QuestObjective> objectives;

    public string GetLocalizedName()
    {
        var locale = LocalizationSettings.SelectedLocale;
        if (locale == null)
        {
            Debug.LogWarning("No locale selected, falling back to English.");
            return questNameENG;
        }

        var code = locale.Identifier.Code.ToLower();

        switch (code)
        {
            case "pl":
                return questNamePL;
            case "en":
            default:
                return questNameENG;
        }
    }
    public string GetLocalizedDescription()
    {
        var locale = LocalizationSettings.SelectedLocale;
        if (locale == null)
        {
            Debug.LogWarning("No locale selected, falling back to English.");
            return questDescriptionENG;
        }

        var code = locale.Identifier.Code.ToLower();

        switch (code)
        {
            case "pl":
                return questDescriptionPL;
            case "en":
            default:
                return questDescriptionENG;
        }
    }
}

[System.Serializable]
public class QuestObjective
{
    public string descriptionPL;
    public string descriptionENG;
    public string GetLocalizedQuestObjectiveDescription()
    {
        var locale = LocalizationSettings.SelectedLocale;
        if (locale == null)
        {
            Debug.LogWarning("No locale selected, falling back to English.");
            return descriptionENG;
        }

        var code = locale.Identifier.Code.ToLower();

        switch (code)
        {
            case "pl":
                return descriptionPL;
            case "en":
            default:
                return descriptionENG;
        }
    }

    [SerializeField] private Object target;

    public ItemSO targetItem => target as ItemSO;
    public ActorSO targetNPC => target as ActorSO;
    public LocationSO targetLocation => target as LocationSO;

    public int requiredAmount;
    public int currentAmount;
}
