using UnityEngine;
using TMPro;

public class QuestLogSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text questNameText;
    [SerializeField] private TMP_Text questLevelText;

    public QuestSO currentQuest;

    public QuestLogUI questLogUI;

    private void OnValidate()
    {
        if(currentQuest != null)
            SetQuest(currentQuest);
        else
            gameObject.SetActive(false);
    }

    public void SetQuest(QuestSO questSO)
    {
        currentQuest = questSO;
        questNameText.text = currentQuest.GetLocalizedQuestName();
        questLevelText.text = "Lvl: " + currentQuest.questLevel.ToString();

        gameObject.SetActive(true);
    }

    public void ClearSlot()
    {
        currentQuest = null;
        gameObject.SetActive(false);
    }

    public void OnSlotClicked()
    {
        questLogUI.HandleQuestClicked(currentQuest);
    }
}
