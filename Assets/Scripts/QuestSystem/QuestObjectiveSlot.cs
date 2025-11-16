using TMPro;
using UnityEngine;

public class QuestObjectiveSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text objectiveText;
    [SerializeField] private TMP_Text trackingText;

    public void RefreshPbjectives(string description, string progressText, bool isComplete)
    {
        objectiveText.text = description;
        trackingText.text = progressText;

        Color color;
        if (isComplete)
            ColorUtility.TryParseHtmlString("#2F6950", out color);
        else
            color = Color.white;
            
        objectiveText.color = color;
        trackingText.color = color;
    }
}
