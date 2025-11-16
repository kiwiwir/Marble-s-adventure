using UnityEngine;

public class QuestLogUI : MonoBehaviour
{
    [SerializeField] private QuestManager questManager;
    public void HandleQuestClicked(QuestSO questSO)
    {
        Debug.Log("=== Quest clicked: " + questSO.GetLocalizedName() + " ===");

        foreach (var objective in questSO.objectives)
        {
            questManager.UpdateObjectiveProgress(questSO, objective);
            Debug.Log("Objective: " + objective.GetLocalizedQuestObjectiveDescription() + " => " + questManager.GetProgressText(questSO, objective));
        }
    }
}
