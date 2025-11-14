using UnityEngine;

public class QuestLogUI : MonoBehaviour
{
    
    public void HandleQuestClicked(QuestSO questSO)
    {
        Debug.Log("Quest clicked: " + questSO.GetLocalizedName());

        foreach (var objective in questSO.objectives)
        {
            Debug.Log("Objective: " + objective.GetLocalizedQuestObjectiveDescription());
        }
    }
}
