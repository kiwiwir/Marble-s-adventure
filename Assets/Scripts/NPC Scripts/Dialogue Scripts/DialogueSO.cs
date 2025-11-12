using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "DialogueSO", menuName = "Dialogue/DialogueNode")]
public class DialogueSO : ScriptableObject
{
    public DialogueLine[] lines;
    public DialogueOption[] options;

    [Header("Conditional Requirements (Optional)")]
    public ActorSO[] requiredNPCs;
    
    //Items
    //Locations





    public bool IsConditionMet()
    {

        if (requiredNPCs.Length > 0)
        {
            foreach (var npc in requiredNPCs)
            {
                if (!DialogueHistoryTracker.Instance.HasSpokenWith(npc))
                    return false;
            }
        }
        //Check for Items
        //Check for Locations
        
        return true;
    }
}

[System.Serializable]
public class DialogueLine
{
    public ActorSO speaker;
    [Header("Localized Text")]
    [TextArea(3, 5)] public string textENG;
    [TextArea(3, 5)] public string textPL;

    [Tooltip("Name of the expression to display (optional)")]
    public string expressionName;
    public string GetLocalizedText()
    {
        var locale = LocalizationSettings.SelectedLocale;
        if (locale == null)
        {
            Debug.LogWarning("No locale selected, falling back to English.");
            return textENG;
        }

        var code = locale.Identifier.Code.ToLower();

        switch (code)
        {
            case "pl":
                return textPL;
            case "en":
            default:
                return textENG;
        }
    }
}

[System.Serializable]
public class DialogueOption
{
    [TextArea(2, 4)] public string optionTextENG;
    [TextArea(2, 4)] public string optionTextPL;
    public DialogueSO nextDialogue;

    public string GetLocalizedText()
    {
        var locale = LocalizationSettings.SelectedLocale;
        if (locale == null)
        {
            Debug.LogWarning("No locale selected, falling back to English.");
            return optionTextENG;
        }

        var code = locale.Identifier.Code.ToLower();

        switch (code)
        {
            case "pl":
                return optionTextPL;
            case "en":
            default:
                return optionTextENG;
        }
    }
}
