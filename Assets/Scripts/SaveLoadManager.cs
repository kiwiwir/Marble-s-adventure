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
public class PlayerSaveData
{
    public string currentScene;
    public float playerPosX;
    public float playerPosY;
    public int gold;
    public List<ItemSaveData> inventoryItems = new List<ItemSaveData>();
    public List<string> removedDialogues = new List<string>();
    public List<ChestStateData> chestStates = new List<ChestStateData>();
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



        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetSavePath(), json);
        Debug.Log("Game saved to " + GetSavePath());
    }

    // ---------------- LOAD ----------------
    public void LoadGame()
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

        Debug.Log("Save loaded successfully!");
    }
}
