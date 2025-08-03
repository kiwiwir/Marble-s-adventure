using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI References")]
    public CanvasGroup canvasGroup;
    public Image portrait;
    public TMP_Text actorName;
    public TMP_Text dialogueText;
    public Image dialogueArrow;


    private bool isTyping;
    public bool isDialogueActive;

    private DialogueSO currentDialogue;
    private int dialogueIndex;

    private Coroutine typingCoroutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
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
            EndDialogue();
    }

    private void ShowDialogue()
    {
        DialogueLine line = currentDialogue.lines[dialogueIndex];

        Sprite chosenPortrait = line.speaker.GetExpressionPortrait(line.expressionName);
        portrait.sprite = chosenPortrait;

        actorName.text = line.speaker.actorName;

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

    private void EndDialogue()
    {
        dialogueIndex = 0;
        isDialogueActive = false;

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
