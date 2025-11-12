using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "DialogueSO", menuName = "Dialogue/DialogueNode")]
public class DialogueSO : ScriptableObject
{
    public DialogueLine[] lines;
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
