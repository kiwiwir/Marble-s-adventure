using UnityEngine;
using System.Collections;

public class AutoStartDialogue : MonoBehaviour
{
    public DialogueSO dialogueToStart;
    public float delay = 23f;
    public SceneChanger sceneChanger;          // Dodaj SceneChanger
    public string sceneToLoad = "TrainStationScene";  // Nazwa sceny po dialogu

    private bool dialogueStarted = false;
    private bool sceneRequested = false;

    private void Start()
    {
        Invoke(nameof(StartDialogue), delay);
    }

    private void StartDialogue()
    {
        if (!dialogueStarted)
        {
            dialogueStarted = true;
            GameManager.Instance.DialogueManager.StartDialogue(dialogueToStart);
        }
    }

    private void Update()
    {
        if (dialogueStarted && Input.GetButtonDown("Interact"))
        {
            if (GameManager.Instance.DialogueManager.isDialogueActive)
            {
                GameManager.Instance.DialogueManager.AdvanceDialogue();
            }
        }

        // Sprawdzaj czy dialog się zakończył i zmień scenę tylko raz
        if (dialogueStarted && !GameManager.Instance.DialogueManager.isDialogueActive && !sceneRequested)
        {
            sceneRequested = true;
            StartCoroutine(WaitAndChangeScene());
        }
    }
    private IEnumerator WaitAndChangeScene()
    {
        AudioManager.Play("TrainDoorOpen");
        
        yield return new WaitForSeconds(1f);
        sceneChanger.ChangeSceneWithFade(sceneToLoad);
    }
}
