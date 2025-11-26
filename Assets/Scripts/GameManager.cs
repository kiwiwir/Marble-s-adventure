using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public DialogueManager DialogueManager;
    public DialogueHistoryTracker DialogueHistoryTracker;
    public LocationHistoryTracker LocationHistoryTracker;
    public QuestManager QuestManager;

    [Header("Persistent Objects")]
    public GameObject[] persistentObjects;

    // słownik do przechowywania stanu skrzyń
    public Dictionary<string, bool> chestStates = new Dictionary<string, bool>();
    // przechowywanie zbiór ID dialogów, które zostały usunięte
    public HashSet<string> removedDialogues = new HashSet<string>();


    private void Awake()
    {
        if (Instance != null)
        {
            CleanUpAndDestroy();
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            MarkPersistentObjects();
        }
    }

    private void MarkPersistentObjects()
    {
        foreach (GameObject obj in persistentObjects)
        {
            if (obj != null)
            {
                DontDestroyOnLoad(obj);
            }
        }
    }
    private void CleanUpAndDestroy()
    {
        foreach (GameObject obj in persistentObjects)
        {
            Destroy(obj);
        }

        Destroy(gameObject);
    }

    // metoda do ustawienia stanu skrzyni
    public void SetChestState(string chestID, bool isOpened)
    {
        if (chestStates.ContainsKey(chestID))
            chestStates[chestID] = isOpened;
        else
            chestStates.Add(chestID, isOpened);
    }

    // metoda do pobrania stanu skrzyni
    public bool GetChestState(string chestID)
    {
        if (chestStates.TryGetValue(chestID, out bool state))
            return state;
        return false; // domyślnie zamknięta
    }
}
