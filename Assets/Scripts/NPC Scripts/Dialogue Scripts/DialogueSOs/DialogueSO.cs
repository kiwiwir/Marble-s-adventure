using UnityEngine;
using UnityEngine.Localization.Settings;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DialogueSO", menuName = "Dialogue/DialogueNode")]
public class DialogueSO : ScriptableObject
{
    public string dialogueID;

    public DialogueLine[] lines;
    public DialogueOption[] options;

    [Header("Quest Offer (Optional)")]
    public QuestSO offerQuestOnEnd;

    [Header("Completed Quest Requirement (Optional)")]
    public QuestSO[] requiredCompletedQuests;

    [Header("Quest Turn-In (Optional)")]
    public QuestSO turnInQuestOnEnd;

    [Header("Conditional Requirements (Optional)")]
    public ActorSO[] requiredNPCs;
    public LocationSO[] requiredLocations;
    public ItemSO[] requiredItems;

    [Header("Control Flags")]
    public bool removeAfterPlay;
    public List<DialogueSO> removeTheseOnPlay;

    private void OnValidate() {
        if (string.IsNullOrEmpty(dialogueID))
            dialogueID = System.Guid.NewGuid().ToString();
    }

    public bool IsConditionMet()
    {

        if (requiredNPCs.Length > 0)
        {
            foreach (var npc in requiredNPCs)
            {
                if (!GameManager.Instance.DialogueHistoryTracker.HasSpokenWith(npc))
                    return false;
            }
        }

        if (requiredLocations.Length > 0)
        {
            foreach (var location in requiredLocations)
            {
                if (!GameManager.Instance.LocationHistoryTracker.HasVisited(location))
                    return false;
            }
        }
        
        if( requiredItems.Length > 0)
        {
            foreach (var item in requiredItems)
            {
                if (!InventoryManager.Instance.HasItem(item))
                    return false;
            }
        }
        
        if(requiredCompletedQuests != null && requiredCompletedQuests.Length > 0)
        {
            foreach(var quest in requiredCompletedQuests)
            {
                if (!GameManager.Instance.QuestManager.IsQuestComplete(quest))
                    return false;
            }
        }
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

    public QuestSO offerQuest;

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
