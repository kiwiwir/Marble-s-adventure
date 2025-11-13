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
                EndDialogue(); // automatyczne zakończenie, jeśli brak opcji
        }
    }

    private void ShowDialogue()
    {
        DialogueLine line = currentDialogue.lines[dialogueIndex];

        GameManager.Instance.DialogueHistoryTracker.RecordNPC(line.speaker);

        Sprite chosenPortrait = line.speaker.GetExpressionPortrait(line.expressionName);
        portrait.sprite = chosenPortrait;

        //actorName.text = line.speaker.actorName;
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

        string localizedText = line.GetLocalizedText();
        foreach (char letter in localizedText)
        {
            dialogueText.text += letter;

            if (line.speaker.voiceSound)
            {
                float basePitch = line.speaker.basePitch;
                float pitchVariation = line.speaker.pitchVariation;
                float finalPitch = basePitch + Random.Range(-pitchVariation, pitchVariation);

                AudioManager.PlayVoice(line.speaker.voiceSound, finalPitch);
            }

            yield return new WaitForSeconds(line.speaker.typingSpeed);
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
        {
            choiceButtons[0].GetComponentInChildren<TMP_Text>().text = "X";
            choiceButtons[0].onClick.AddListener(EndDialogue);
            choiceButtons[0].gameObject.SetActive(true);
        }
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
        dialogueIndex = 0;
        isDialogueActive = false;
        ClearChoices();

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
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
