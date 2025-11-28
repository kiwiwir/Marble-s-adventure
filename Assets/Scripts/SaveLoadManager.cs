using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ItemSaveData
{
    public string itemName;
    public int quantity;
}
[System.Serializable]
public class ChestStateData
{
    public string chestID;
    public bool isOpened;
}
[System.Serializable]
public class QuestObjectiveSaveData
{
    public string objectiveID;
    public int currentAmount;
}

[System.Serializable]
public class QuestSaveData
{
    public string questID;
    public List<QuestObjectiveSaveData> objectives = new List<QuestObjectiveSaveData>();
}
[System.Serializable]
public class PlayerSaveData
{
    public string currentScene;
    public float playerPosX;
    public float playerPosY;
    public int gold;
    public List<ItemSaveData> inventoryItems = new List<ItemSaveData>();
    public List<string> removedDialogues = new List<string>();
    public List<ChestStateData> chestStates = new List<ChestStateData>();
    // ---------------- QUESTS ----------------
    public List<QuestSaveData> activeQuests = new List<QuestSaveData>();
    public List<string> completedQuests = new List<string>();
}


public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private string GetSavePath() => Application.persistentDataPath + "/savegame.json";

    // ---------------- SAVE ----------------
    public void SaveGame(Vector3 playerPosition)
    {
        PlayerSaveData data = new PlayerSaveData();
        data.currentScene = SceneManager.GetActiveScene().name;
        data.playerPosX = playerPosition.x;
        data.playerPosY = playerPosition.y;

        // gold
        data.gold = InventoryManager.Instance.gold;

        // inventory
        data.inventoryItems.Clear();
        foreach (var slot in InventoryManager.Instance.itemSlots)
        {
            if (slot.itemSO != null && slot.quantity > 0)
            {
                ItemSaveData itemData = new ItemSaveData
                {
                    itemName = slot.itemSO.name, // nazwa SO
                    quantity = slot.quantity
                };
                data.inventoryItems.Add(itemData);
            }
        }

        // removed dialogues
        data.removedDialogues = new List<string>(GameManager.Instance.removedDialogues);

        // chest states
        data.chestStates = new List<ChestStateData>();
        foreach (var kvp in GameManager.Instance.chestStates)
        {
            data.chestStates.Add(new ChestStateData
            {
                chestID = kvp.Key,
                isOpened = kvp.Value
            });
        }
        
        // quests states
        data.activeQuests.Clear();
        foreach (var kvp in GameManager.Instance.QuestManager.GetQuestProgressDictionary())
        {
            QuestSO questSO = kvp.Key;
            QuestSaveData questData = new QuestSaveData();
            questData.questID = questSO.name;

            foreach (var obj in kvp.Value)
            {
                questData.objectives.Add(new QuestObjectiveSaveData
                {
                    objectiveID = obj.Key.descriptionPL,
                    currentAmount = obj.Value
                });
            }

            data.activeQuests.Add(questData);
        }

        data.completedQuests = new List<string>(GameManager.Instance.QuestManager.GetCompletedQuestsNames());



        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetSavePath(), json);
        Debug.Log("Game saved to " + GetSavePath());
    }

    // ---------------- LOAD ----------------
    /*public void LoadGame()
    {
        string path = GetSavePath();
        if (!File.Exists(path))
        {
            Debug.LogWarning("No save file found!");
            return;
        }

        string json = File.ReadAllText(GetSavePath());
        PlayerSaveData data = JsonUtility.FromJson<PlayerSaveData>(json);

        SceneManager.sceneLoaded += (scene, mode) =>
        {
            ApplySaveData(data);
            SceneManager.sceneLoaded -= ApplySaveDataHandler;
        };

        SceneManager.LoadScene(data.currentScene);
    }*/
    private PlayerSaveData loadedData = null;

    public void LoadGame()
    {
        string path = GetSavePath();
        if (!File.Exists(path))
        {
            Debug.LogWarning("No save file found!");
            return;
        }

        string json = File.ReadAllText(GetSavePath());
        loadedData = JsonUtility.FromJson<PlayerSaveData>(json);
        Debug.Log("Save data loaded into memory.");
    }
    public void ApplyLoadedSave()
    {
        if (loadedData == null)
        {
            Debug.LogWarning("No loaded save data to apply!");
            return;
        }

        // Załaduj scenę z zapisu
        SceneManager.sceneLoaded += OnSceneLoadedApplySave;
        SceneManager.LoadScene(loadedData.currentScene);
    }

    private void OnSceneLoadedApplySave(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoadedApplySave;
        ApplySaveData(loadedData);
        loadedData = null; // opcjonalnie czyścimy, żeby nie było ponownego zastosowania
    }



    private void ApplySaveDataHandler(Scene scene, LoadSceneMode mode) { }

    private void ApplySaveData(PlayerSaveData data)
    {
        // Pozycja gracza
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = new Vector2(data.playerPosX, data.playerPosY);
        }

        // Gold i inventory
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.gold = data.gold;
            InventoryManager.Instance.goldText.text = data.gold.ToString();

            // Czyścimy sloty przed załadowaniem
            foreach (var slot in InventoryManager.Instance.itemSlots)
            {
                slot.itemSO = null;
                slot.quantity = 0;
                slot.UpdateUI();
            }

            // Odtwarzamy przedmioty
            foreach (var itemData in data.inventoryItems)
            {
                ItemSO itemSO = Resources.Load<ItemSO>("ItemSOs/" + itemData.itemName);
                if (itemSO != null)
                {
                    InventoryManager.Instance.AddItem(itemSO, itemData.quantity);
                }
                else
                {
                    Debug.LogWarning("Nie znaleziono ItemSO: " + itemData.itemName);
                }
            }
        }

        // removed dialogues
        GameManager.Instance.removedDialogues = new HashSet<string>(data.removedDialogues);

        // chest states
        GameManager.Instance.chestStates.Clear();
        foreach (var chestData in data.chestStates)
        {
            GameManager.Instance.chestStates[chestData.chestID] = chestData.isOpened;
        }

        // quests
        if (GameManager.Instance.QuestManager != null)
        {
            GameManager.Instance.QuestManager.ClearAllQuests();

            foreach (var questData in data.activeQuests)
            {
                QuestSO questSO = Resources.Load<QuestSO>("QuestSOs/" + questData.questID);
                if (questSO != null)
                {
                    GameManager.Instance.QuestManager.AcceptQuest(questSO);

                    foreach (var objData in questData.objectives)
                    {
                        var objective = questSO.objectives.Find(o => o.descriptionPL == objData.objectiveID);
                        if (objective != null)
                            GameManager.Instance.QuestManager.SetObjectiveProgress(questSO, objective, objData.currentAmount);
                    }
                }
            }

            foreach (var questID in data.completedQuests)
            {
                QuestSO questSO = Resources.Load<QuestSO>("QuestSOs/" + questID);
                if (questSO != null)
                    GameManager.Instance.QuestManager.MarkQuestCompleted(questSO);
            }
        }

        Debug.Log("Save loaded successfully!");
    }

    public void ResetGameToDefault()
    {
        Debug.Log("RESET: Przygotowywanie nowej gry bez wczytywania JSON...");

        // Wyzeruj dane załadowane z pliku
        loadedData = null;

        // ============================
        // INVENTORY
        // ============================
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.gold = 0;

            if (InventoryManager.Instance.goldText != null)
                InventoryManager.Instance.goldText.text = "0";

            foreach (var slot in InventoryManager.Instance.itemSlots)
            {
                slot.itemSO = null;
                slot.quantity = 0;
                slot.UpdateUI();
            }
        }

        // ============================
        // REMOVED DIALOGUES
        // ============================
        if (GameManager.Instance != null)
        {
            GameManager.Instance.removedDialogues.Clear();
        }

        // ============================
        // CHEST STATES
        // ============================
        if (GameManager.Instance != null)
        {
            GameManager.Instance.chestStates.Clear();
        }

        // ============================
        // QUESTS
        // ============================
        if (GameManager.Instance != null && GameManager.Instance.QuestManager != null)
        {
            GameManager.Instance.QuestManager.ClearAllQuests();
        }

        // ============================
        // RESET SCENY I POZYCJI
        // ============================
        // To zostanie nadpisane przez system przy Cutscene01
        PlayerSaveData resetData = new PlayerSaveData();
        resetData.currentScene = "Cutscene01";
        resetData.playerPosX = 0f;
        resetData.playerPosY = 0f;

        Debug.Log("RESET GOTOWY — nowa gra ruszy od Cutscene01.");
    }

}
