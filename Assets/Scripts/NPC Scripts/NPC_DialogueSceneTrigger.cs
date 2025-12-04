using System;
using UnityEngine;

public class DialogueSceneLoader : MonoBehaviour
{
    [Header("Dialogue to trigger scene change")]
    public DialogueSO targetDialogue;        // ten dialog musi się zakończyć
    public string sceneToLoad;               // nazwa sceny do załadowania

    private bool waitingForDialogueEnd = false;

    private void OnEnable()
    {
        // nasłuchujemy aktywacji dialogu
        DialogueEvents.OnDialogueStarted += OnDialogueStarted;
        DialogueEvents.OnDialogueEnded += OnDialogueEnded;
    }

    private void OnDisable()
    {
        DialogueEvents.OnDialogueStarted -= OnDialogueStarted;
        DialogueEvents.OnDialogueEnded -= OnDialogueEnded;
    }

    private void OnDialogueStarted(DialogueSO dialogue)
    {
        // jeśli rozpoczął się dialog, na który czekamy → aktywujemy flagę
        if (dialogue == targetDialogue)
        {
            waitingForDialogueEnd = true;
        }
    }

    private void OnDialogueEnded(DialogueSO dialogue)
    {
        // jeśli zakończony dialog to ten, którego szukamy
        if (waitingForDialogueEnd && dialogue == targetDialogue)
        {
            waitingForDialogueEnd = false;

            // znajdź SceneChanger i przełącz scenę
            var changer = FindObjectOfType<SceneChanger>();
            if (changer != null)
            {
                changer.ChangeSceneWithFade(sceneToLoad);
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}
public static class DialogueEvents
{
    public static Action<DialogueSO> OnDialogueStarted;
    public static Action<DialogueSO> OnDialogueEnded;
}
