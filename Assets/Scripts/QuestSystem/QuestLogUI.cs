using TMPro;
using UnityEngine;

public class QuestLogUI : MonoBehaviour
{
    [SerializeField] private QuestManager questManager;

    [SerializeField] private TMP_Text questNameText;
    [SerializeField] private TMP_Text questDescriptionText;
    [SerializeField] private QuestObjectiveSlot[] objectiveSlots;
    [SerializeField] private QuestRewardSlot[] rewardSlots;

    private QuestSO questSO;

    [SerializeField] private CanvasGroup questCanvas;
    [SerializeField] private CanvasGroup acceptCanvasGroup;
    [SerializeField] private CanvasGroup declineCanvasGroup;
    [SerializeField] private CanvasGroup completeCanvasGroup;


    private void OnEnable()
    {
        QuestEvents.OnQuestOfferRequested += ShowQuestOffer;
    }
    private void OnDisable()
    {
        QuestEvents.OnQuestOfferRequested -= ShowQuestOffer;
    }

    public void ShowQuestOffer(QuestSO incomingQuestSO)
    {
        HandleQuestClicked(incomingQuestSO);
        SetCanvasState(questCanvas, true);

        SetCanvasState(acceptCanvasGroup, true);
        SetCanvasState(declineCanvasGroup, true);
        SetCanvasState(completeCanvasGroup, false);
    }

    private void SetCanvasState(CanvasGroup group, bool activate)
    {
        group.alpha = activate ? 1 : 0;
        group.blocksRaycasts = activate;
        group.interactable = activate;
    }

    public void HandleQuestClicked(QuestSO questSO)
    {
        this.questSO = questSO;

        questNameText.text = questSO.GetLocalizedQuestName();
        questDescriptionText.text = questSO.GetLocalizedQuestDescription();

        DisplayObjective();
        DisplayRewards();
    }
    private void DisplayObjective()
    {
        for (int i = 0; i < objectiveSlots.Length; i++)
        {
            if (i < questSO.objectives.Count)
            {
                var objective = questSO.objectives[i];
                questManager.UpdateObjectiveProgress(questSO, objective);

                int currentAmount = questManager.GetCurrentAmount(questSO, objective);
                string progress = questManager.GetProgressText(questSO, objective);
                bool isComplete = currentAmount >= objective.requiredAmount;

                objectiveSlots[i].gameObject.SetActive(true);
                objectiveSlots[i].RefreshPbjectives(objective.GetLocalizedQuestObjectiveDescription(), progress, isComplete);
            }
            else
            {
                objectiveSlots[i].gameObject.SetActive(false);
            }
        }
    }


    private void DisplayRewards()
    {
        for (int i = 0; i < rewardSlots.Length; i++)
        {
            if (i < questSO.rewards.Count)
            {
                var reward = questSO.rewards[i];
                rewardSlots[i].DisplayReward(reward.itemSO.icon, reward.quantity);
                rewardSlots[i].gameObject.SetActive(true);
            }
            else
            {
                rewardSlots[i].gameObject.SetActive(false);
            }
        }
    }
}
