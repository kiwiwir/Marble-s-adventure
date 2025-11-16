using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<QuestSO, Dictionary<QuestObjective, int>> questProgress = new();


    public void UpdateObjectiveProgress(QuestSO questSO, QuestObjective objective)
    {
        if (!questProgress.ContainsKey(questSO))
        {
            questProgress[questSO] = new Dictionary<QuestObjective, int>();
        }

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

        if(currentAmount >= objective.requiredAmount)
            return "Completed";
        else if (objective.targetItem != null)
            return $"{currentAmount}/{objective.requiredAmount}";
        else
            return "In Progress";

    }

    public int GetCurrentAmount(QuestSO questSO, QuestObjective objective)
    {
        if (questProgress.TryGetValue(questSO, out var objectiveDictionary))
            if (objectiveDictionary.TryGetValue(objective, out var amount))
                return amount;
        return 0;
    }
}
