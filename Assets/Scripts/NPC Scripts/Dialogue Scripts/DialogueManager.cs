using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public CanvasGroup canvasGroup;
    public Image portrait;
    public TMP_Text actorName;
    public TMP_Text dialogueText;
    public Image dialogueArrow;
    public Image dialogueChoicePanel;
    public Button[] choiceButtons;


    private bool isTyping;
    public bool isDialogueActive;

    private DialogueSO currentDialogue;
    private int dialogueIndex;

    private Coroutine typingCoroutine;

    private void Awake()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        foreach (var button in choiceButtons)
        {
            button.gameObject.SetActive(false);
            dialogueChoicePanel.gameObject.SetActive(false);
        }
    }

    public void StartDialogue(DialogueSO dialogueSO)
    {
        currentDialogue = dialogueSO;
        dialogueIndex = 0;
        isDialogueActive = true;
        Time.timeScale = 0f;
        ShowDialogue();
    }

    public void AdvanceDialogue()
    {
        if (isTyping)
        {
            // Skip typing and show full line immediately
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentDialogue.lines[dialogueIndex].GetLocalizedText();

            isTyping = false;

            if (dialogueArrow != null)
                dialogueArrow.enabled = true;

            return;
        }

        dialogueIndex++;

        if (dialogueIndex < currentDialogue.lines.Length)
            ShowDialogue();
        else
        {
            if (currentDialogue.options.Length > 0)
                ShowChoices();
            else
                EndDialogue();
        }
    }

    private void ShowDialogue()
    {
        DialogueLine line = currentDialogue.lines[dialogueIndex];

        GameManager.Instance.DialogueHistoryTracker.RecordNPC(line.speaker);

        Sprite chosenPortrait = line.speaker.GetExpressionPortrait(line.expressionName);
        portrait.sprite = chosenPortrait;
        actorName.text = line.speaker.GetLocalizedName();

        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        if (dialogueArrow != null)
            dialogueArrow.enabled = false;


        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(line));
    }
    IEnumerator TypeText(DialogueLine line)
    {
        isTyping = true;
        dialogueText.text = "";
        float timeSinceLastVoice = 0f;

        string localizedText = line.GetLocalizedText();
        foreach (char letter in localizedText)
        {
            dialogueText.text += letter;

            timeSinceLastVoice += line.speaker.typingSpeed;

            if (line.speaker.voiceSound && timeSinceLastVoice >= line.speaker.voiceInterval)
            {
                float basePitch = line.speaker.basePitch;
                float pitchVariation = line.speaker.pitchVariation;
                float finalPitch = basePitch + Random.Range(-pitchVariation, pitchVariation);

                AudioManager.PlayVoice(line.speaker.voiceSound, finalPitch);
                timeSinceLastVoice = 0f;
            }

            // <-- Użycie WaitForSecondsRealtime zamiast WaitForSeconds
            yield return new WaitForSecondsRealtime(line.speaker.typingSpeed);
        }

        isTyping = false;
        if (dialogueArrow != null)
            dialogueArrow.enabled = true;
    }


    private void ShowChoices()
    {
        ClearChoices();
        if (currentDialogue.options.Length > 0)
        {
            for (int i = 0; i < currentDialogue.options.Length; i++)
            {
                var option = currentDialogue.options[i];

                choiceButtons[i].GetComponentInChildren<TMP_Text>().text = option.GetLocalizedText();
                choiceButtons[i].gameObject.SetActive(true);
                dialogueChoicePanel.gameObject.SetActive(true);

                choiceButtons[i].onClick.AddListener(() => ChooseOption(option.nextDialogue));
            }
        }
        else
        /*{
            if(currentDialogue.offerQuestOnEnd != null)
            {
                EndDialogue();
                QuestEvents.OnQuestOfferRequested?.Invoke(currentDialogue.offerQuestOnEnd);
            }
            else*/
            {
                choiceButtons[0].GetComponentInChildren<TMP_Text>().text = "X";
                choiceButtons[0].onClick.AddListener(EndDialogue);
                choiceButtons[0].gameObject.SetActive(true);
            }
        //}
    }

    
    private void ChooseOption(DialogueSO dialogueSO)
    {
        if (dialogueSO == null)
        {
            EndDialogue();
        }
        else
        {
            ClearChoices();
            StartDialogue(dialogueSO);
        }
    }

    private void EndDialogue()
    {
        // --- ZAPISYWANIE STANU USUNIĘTYCH DIALOGÓW ---
        if (currentDialogue != null)
        {
            if (currentDialogue.removeAfterPlay)
                GameManager.Instance.removedDialogues.Add(currentDialogue.dialogueID);

            if (currentDialogue.removeTheseOnPlay != null)
            {
                foreach (var toRemove in currentDialogue.removeTheseOnPlay)
                {
                    if (toRemove != null)
                        GameManager.Instance.removedDialogues.Add(toRemove.dialogueID);
                }
            }
        }
        // ---------------------------------------------
        
        dialogueIndex = 0;
        isDialogueActive = false;
        ClearChoices();

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        Time.timeScale = 1f;

        // Oddanie questa
        if (currentDialogue != null && currentDialogue.turnInQuestOnEnd != null && GameManager.Instance.QuestManager.IsQuestComplete(currentDialogue.turnInQuestOnEnd))
        {
            QuestEvents.OnQuestTurnInRequested?.Invoke(currentDialogue.turnInQuestOnEnd);
        }
        // Wywołanie questa
        else if (currentDialogue != null && currentDialogue.offerQuestOnEnd != null)
        {
            QuestEvents.OnQuestOfferRequested?.Invoke(currentDialogue.offerQuestOnEnd);
        }
    }

    private void ClearChoices()
    {
        foreach (var button in choiceButtons)
        {
            button.gameObject.SetActive(false);
            button.onClick.RemoveAllListeners();
        }
        dialogueChoicePanel.gameObject.SetActive(false);
    }
}
