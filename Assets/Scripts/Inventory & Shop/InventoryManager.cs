using System.Collections;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public InventorySlot[] itemSlots;
    public RectTransform inventoryPanel;

    public UseItem useItem;
    public int gold;
    public TMP_Text goldText;
    public GameObject lootPrefab;
    public Transform player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        foreach (var slot in itemSlots)
        {
            slot.UpdateUI();
        }
    }

    private void OnEnable()
    {
        Loot.OnItemLooted += AddItem;
    }

    private void OnDisable()
    {
        Loot.OnItemLooted -= AddItem;
    }

    public void AddItem(ItemSO itemSO, int quantity)
    {
        if (itemSO.isGold)
        {
            gold += quantity;
            goldText.text = gold.ToString();

            // Dźwięk dla złota
            AudioManager.Play("Coin");
            return;
        }

        // Dźwięk dla zwykłego przedmiotu
        AudioManager.Play("Collect");

        foreach (var slot in itemSlots)
        {
            if (slot.itemSO == itemSO && slot.quantity < itemSO.stackSize)
            {
                int availableSpace = itemSO.stackSize - slot.quantity;
                int amountToAdd = Mathf.Min(availableSpace, quantity);

                slot.quantity += amountToAdd;
                quantity -= amountToAdd;

                slot.UpdateUI();

                if (quantity <= 0)
                    return;
            }
        }

        foreach (var slot in itemSlots)
        {
            if (slot.itemSO == null)
            {
                int amountToAdd = Mathf.Min(itemSO.stackSize, quantity);
                slot.itemSO = itemSO;
                slot.quantity = quantity;
                slot.UpdateUI();
                return;
            }
        }

        if (quantity > 0)
        {
            DropLoot(itemSO, quantity);
        }
    }

    private void DropLoot(ItemSO itemSO, int quantity)
    {
        // losowy kierunek w 2D (okrąg wokół gracza)
        // losujemy punkt w pierścieniu [1, 1.5]
        float radius = Random.Range(1.5f, 1.75f);
        Vector2 randomCircle = Random.insideUnitCircle.normalized * radius;

        Vector3 dropPosition = player.position + new Vector3(randomCircle.x, randomCircle.y, 0f);

        GameObject lootObj = Instantiate(lootPrefab, dropPosition, Quaternion.identity);
        Loot loot = lootObj.GetComponent<Loot>();
        loot.Initialize(itemSO, quantity);

        // Bounce effect
        BounceEffect bounce = lootObj.GetComponent<BounceEffect>();
        if (bounce != null)
        {
            bounce.StartBounce();
        }
    }

    /*public void DropItem(InventorySlot slot)
    {
        DropLoot(slot.itemSO, 1);
        slot.quantity--;
        if (slot.quantity <= 0)
        {
            slot.itemSO = null;
        }
        slot.UpdateUI();
        
        // Dźwięk wyrzucenia przedmiotu
        AudioManager.Play("DropItem");
    }*/
    public void DropItem(InventorySlot slot)
    {
        StartCoroutine(DropItemRoutine(slot));
    }

    private IEnumerator DropItemRoutine(InventorySlot slot)
    {
        // najpierw zrób kopię danych slotu
        ItemSO itemToDrop = slot.itemSO;
        int quantityToDrop = 1;

        // zmniejsz slot w inventory od razu
        slot.quantity--;
        if (slot.quantity <= 0)
            slot.itemSO = null;
        slot.UpdateUI();

        yield return null; // czekaj do końca klatki, żeby loot nie został od razu podniesiony

        // losowy kierunek w 2D (okrąg wokół gracza)
        Vector2 randomCircle = Random.insideUnitCircle * 1.5f;
        Vector3 dropPosition = player.position + new Vector3(randomCircle.x, randomCircle.y, 0f);

        Loot loot = Instantiate(lootPrefab, dropPosition, Quaternion.identity).GetComponent<Loot>();
        loot.Initialize(itemToDrop, quantityToDrop);

        AudioManager.Play("DropItem");
    }


    public void UseItem(InventorySlot slot)
    {
        if (slot.itemSO != null && slot.quantity > 0 && slot.itemSO.isUsable)
        {
            useItem.ApplyItemEffects(slot.itemSO);

            slot.quantity--;
            if (slot.quantity <= 0)
            {
                slot.itemSO = null;
            }
            slot.UpdateUI();

            // Dźwięk użycia przedmiotu
            AudioManager.Play("UseItem");
        }
        else
        {
            AudioManager.Play("Error");
        }
    }

    public bool HasItem(ItemSO itemSO)
    {
        foreach (var slot in itemSlots)
        {
            if (slot.itemSO == itemSO && slot.quantity > 0)
            {
                return true;
            }
        }
        return false;
    }

    public int GetItemQuantity(ItemSO itemSO)
    {
        int total = 0;
        foreach (var slot in itemSlots)
        {
            if (slot.itemSO == itemSO)
            {
                total += slot.quantity;
            }
        }
        return total;
    }
}
