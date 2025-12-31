using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour, IInteractable
{
    public bool IsOpened { get; private set; }
    public string ChestID { get; private set; }

    [Header("Loot Settings")]
    public GameObject lootPrefab;
    public ItemSO chestItem;        // jaki item ma być w skrzyni
    public int quantity = 1;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        ChestID ??= GlobalHelper.GenerateUniqueID(gameObject);
        bool savedState = GameManager.Instance.GetChestState(ChestID);
        SetOpened(savedState);
    }

    public bool CanInteract()
    {
        return !IsOpened;
    }

    public void Interact()
    {
        if (!CanInteract()) return;
        OpenChest();
    }

    private void OpenChest()
    {
        SetOpened(true);
        GameManager.Instance.SetChestState(ChestID, true); // zapisz stan
        StartCoroutine(DropItemAfterDelay());
    }

    private IEnumerator DropItemAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        
        if (lootPrefab && chestItem != null)
        {
            GameObject lootObj = Instantiate(lootPrefab, transform.position + Vector3.down, Quaternion.identity);

            // ustaw itemSO i ilość na loocie
            Loot loot = lootObj.GetComponent<Loot>();
            if (loot != null)
            {
                loot.Initialize(chestItem, quantity);
            }

            // uruchom efekt bounce
            BounceEffect bounce = lootObj.GetComponent<BounceEffect>();
            if (bounce != null)
            {
                bounce.StartBounce();
            }
        }
    }
    
    public void SetOpened(bool opened)
    {
        IsOpened = opened;
        if (animator != null)
            animator.SetBool("isOpened", opened);
    }
}
