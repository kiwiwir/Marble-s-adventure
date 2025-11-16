using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class QuestManager : MonoBehaviour
{
    private Dictionary<QuestSO, Dictionary<QuestObjective, int>> questProgress = new();

    private void OnEnable()
    {
        QuestEvents.IsQuestComplete += IsQuestComplete;
    }
    private void OnDisable()
    {
        QuestEvents.IsQuestComplete -= IsQuestComplete;
    }


    #region Quest Accept Logic
    public bool IsQuestAccepted(QuestSO questSO)
    {
        return questProgress.ContainsKey(questSO);
    }

    public List<QuestSO> GetActiveQuests()
    {
        return new List<QuestSO>(questProgress.Keys);
    }
    
    public void AcceptQuest(QuestSO questSO)
    {
        questProgress[questSO] = new Dictionary<QuestObjective, int>();

        foreach (var objective in questSO.objectives)
        {
            UpdateObjectiveProgress(questSO, objective);
        }
    }
    #endregion

    #region Quest Complete Logic
    public bool IsQuestComplete(QuestSO questSO)
    {
        if (!questProgress.TryGetValue(questSO, out var progressDict))
            return false;
        
        foreach (var objective in questSO.objectives)
        {
            UpdateObjectiveProgress(questSO, objective);
        }

        foreach (var objective in questSO.objectives)
        {
            if (progressDict[objective] < objective.requiredAmount)
                return false;
        }
        return true;
    }

    public void CompleteQuest(QuestSO questSO)
    {
        questProgress.Remove(questSO);
        //TODO Granting rewards
    }
    #endregion

    public void UpdateObjectiveProgress(QuestSO questSO, QuestObjective objective)
    {
        if (!questProgress.ContainsKey(questSO))
            return;

        var progressDictionary = questProgress[questSO];
        int newAmount = 0;

        if(objective.targetItem != null)
            newAmount = InventoryManager.Instance.GetItemQuantity(objective.targetItem);
        
        else if (objective.targetLocation != null && GameManager.Instance.LocationHistoryTracker.HasVisited(objective.targetLocation))
            newAmount = objective.requiredAmount;
        
        else if (objective.targetNPC != null && GameManager.Instance.DialogueHistoryTracker.HasSpokenWith(objective.targetNPC))
            newAmount = objective.requiredAmount;

        progressDictionary[objective] = newAmount;
    }

    public string GetProgressText(QuestSO questSO, QuestObjective objective)
    {
        int currentAmount = GetCurrentAmount(questSO, objective);

        // Pobierz aktualny język (locale)
        var locale = LocalizationSettings.SelectedLocale;
        string code = locale != null ? locale.Identifier.Code.ToLower() : "en";

        // Lokalizowane słowa
        string completed;
        string inProgress;

        switch (code)
        {
            case "pl":
                completed = "Ukończone";
                inProgress = "W trakcie";
                break;

            case "en":
            default:
                completed = "Completed";
                inProgress = "In Progress";
                break;
        }

        // Logika postępu
        if (currentAmount >= objective.requiredAmount)
        {
            return completed;
        }
        else if (objective.targetItem != null)
        {
            return $"{currentAmount}/{objective.requiredAmount}";
        }
        else
        {
            return inProgress;
        }
    }

    public int GetCurrentAmount(QuestSO questSO, QuestObjective objective)
    {
        if (questProgress.TryGetValue(questSO, out var objectiveDictionary))
            if (objectiveDictionary.TryGetValue(objective, out var amount))
                return amount;
        return 0;
    }
}
