using UnityEngine;

public class CraftingTrigger : MonoBehaviour
{
    public ItemSO berrySO;
    public ItemSO slimeSO;
    public ItemSO emptyVialSO;
    public ItemSO potionSO;

    private bool canCraft = false;

    public Animator cookingAnimator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            canCraft = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            canCraft = false;
    }

    private void Update()
    {
        if (canCraft && Input.GetKeyDown(KeyCode.E)) // Interact = E
        {
            TryCraftPotion();
            
        }
    }

    private void TryCraftPotion()
    {
        var inv = InventoryManager.Instance;

        // sprawdzamy wymagane ilości
        if (inv.GetItemQuantity(berrySO) >= 2 &&
            inv.GetItemQuantity(slimeSO) >= 1 &&
            inv.GetItemQuantity(emptyVialSO) >= 1)
        {
            // animacja craftingu
            if (cookingAnimator != null)
                cookingAnimator.Play("Cooking", 0, 0f);

            // USUWAMY składniki
            inv.RemoveItem(berrySO, 2);
            inv.RemoveItem(slimeSO, 1);
            inv.RemoveItem(emptyVialSO, 1);

            // DODAJEMY miksturę
            inv.AddItem(potionSO, 1);

            Debug.Log("Mikstura stworzona!");
            AudioManager.Play("Alchemy");
        }
        else
        {
            Debug.Log("Brakuje składników!");
            AudioManager.Play("Error");
        }
    }
}
