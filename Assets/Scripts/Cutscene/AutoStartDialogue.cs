using UnityEngine;

public class AutoStartDialogue : MonoBehaviour
{
    public DialogueSO dialogueToStart;
    public float delay = 23f;

    private bool dialogueStarted = false;

    private void Start()
    {
        Invoke(nameof(StartDialogue), delay);
    }

    private void StartDialogue()
    {
        if (!dialogueStarted)
        {
            dialogueStarted = true;
            DialogueManager.Instance.StartDialogue(dialogueToStart);
        }
    }

    private void Update()
    {
        if (dialogueStarted && Input.GetButtonDown("Interact"))
        {
            if (DialogueManager.Instance.isDialogueActive)
                DialogueManager.Instance.AdvanceDialogue();
        }
    }
}
