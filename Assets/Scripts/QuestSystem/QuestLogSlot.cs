using UnityEngine;
using TMPro;

public class QuestLogSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text questNameText;
    [SerializeField] private TMP_Text questLevelText;

    public QuestSO currentQuest;

    private void OnValidate()
    {
        if(currentQuest != null)
            SetQuest(currentQuest);
    }

    public void SetQuest(QuestSO questSO)
    {
        currentQuest = questSO;
        questNameText.text = currentQuest.GetLocalizedName();
        questLevelText.text = "Lvl: " + currentQuest.questLevel.ToString();
    }
}
