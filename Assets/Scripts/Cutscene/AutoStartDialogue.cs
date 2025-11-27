using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoStartDialogue : MonoBehaviour
{
    public DialogueSO dialogueToStart;
    public float delay = 23f;
    public SceneChanger sceneChanger;                // Dodaj SceneChanger
    public string sceneToLoad = "TrainStationScene"; // Nazwa sceny po dialogu

    [Header("Persistent Objects To Activate")]
    public string[] persistentObjectNames; // nazwy obiektów persistent, które chcemy aktywować

    private bool dialogueStarted = false;
    private bool sceneRequested = false;

    private void Start()
    {
        Invoke(nameof(StartDialogue), delay);
    }

    /*private void StartDialogue()
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
            if (GameManager.Instance != null && GameManager.Instance.DialogueManager != null)
            {
                if (GameManager.Instance.DialogueManager.isDialogueActive)
                {
                    GameManager.Instance.DialogueManager.AdvanceDialogue();
                }
            }
            else
            {
                Debug.LogWarning("GameManager lub DialogueManager jest null!");
            }
        }


        if (dialogueStarted && !GameManager.Instance.DialogueManager.isDialogueActive && !sceneRequested)
        {
            sceneRequested = true;
            StartCoroutine(WaitAndChangeScene());
        }
    }*/
    private void StartDialogue()
    {
        if (dialogueStarted) return;

        if (GameManager.Instance == null || GameManager.Instance.DialogueManager == null)
        {
            // Poczekaj jedną klatkę i spróbuj ponownie
            StartCoroutine(WaitForDialogueManager());
            return;
        }

        dialogueStarted = true;
        GameManager.Instance.DialogueManager.StartDialogue(dialogueToStart);
    }

    private IEnumerator WaitForDialogueManager()
    {
        while (GameManager.Instance == null || GameManager.Instance.DialogueManager == null)
        {
            yield return null;
        }

        dialogueStarted = true;
        GameManager.Instance.DialogueManager.StartDialogue(dialogueToStart);
    }
    private void Update()
    {
        if (!dialogueStarted) return;

        var dm = GameManager.Instance?.DialogueManager;
        if (dm == null) return;

        if (Input.GetButtonDown("Interact") && dm.isDialogueActive)
        {
            dm.AdvanceDialogue();
        }

        if (!dm.isDialogueActive && !sceneRequested)
        {
            sceneRequested = true;
            StartCoroutine(WaitAndChangeScene());
        }
    }



    private IEnumerator WaitAndChangeScene()
    {
        // Poczekaj 1 klatkę, aby wszystkie persistent objects były dostępne
        yield return null;

        foreach (string objName in persistentObjectNames)
        {
            GameObject obj = null;

            if (objName == "Player")
            {
                obj = GameManager.Instance.Player;
            }
            else
            {
                // szukaj po nazwie w Resources
                var allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
                foreach (var go in allObjects)
                {
                    if (go.name == objName)
                    {
                        obj = go;
                        break;
                    }
                }
            }

            if (obj != null)
            {
                obj.SetActive(true);
            }
            else
            {
                Debug.LogWarning($"Nie znaleziono persistent object o nazwie lub tagu: {objName}");
            }
        }


        AudioManager.Play("TrainDoorOpen");
        yield return new WaitForSeconds(1f);

        sceneChanger.ChangeSceneWithFade(sceneToLoad);
    }
}
