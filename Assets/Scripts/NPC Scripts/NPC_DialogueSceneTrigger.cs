using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class NPC_DialogueSceneTrigger : MonoBehaviour
{
    [Header("Dialogue to trigger scene change")]
    public DialogueSO triggerDialogue;

    [Header("Scene to load after dialogue ends")]
    public string sceneToLoad;

    private bool waitingForDialogueEnd = false;

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && !waitingForDialogueEnd)
        {
            if (GameManager.Instance.DialogueManager.isDialogueActive)
            {
                GameManager.Instance.DialogueManager.AdvanceDialogue();
            }
            else
            {
                StartDialogueCheck();
            }
        }
    }

    private void StartDialogueCheck()
    {
        if (triggerDialogue != null && triggerDialogue.IsConditionMet())
        {
            GameManager.Instance.DialogueManager.StartDialogue(triggerDialogue);
            waitingForDialogueEnd = true;
            StartCoroutine(CheckDialogueEnd());
        }
    }

    private IEnumerator CheckDialogueEnd()
    {
        // Czekaj aż dialog się zakończy
        while (GameManager.Instance.DialogueManager.isDialogueActive)
        {
            yield return null;
        }

        waitingForDialogueEnd = false;

        // Jeśli chcesz "czystą" scenę, usuń GameManager i jego persistent objects
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }

        // Załaduj nową scenę w czystym stanie
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("NPC_DialogueSceneTrigger: sceneToLoad nie jest ustawiona!");
        }
    }
}
