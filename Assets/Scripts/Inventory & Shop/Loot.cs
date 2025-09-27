using System;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.UI;

public class Loot : MonoBehaviour
{
    public ItemSO itemSO;
    public SpriteRenderer sr;
    public Animator anim;

    public bool canBePickedUp = true;
    public int quantity;
    public static event Action<ItemSO, int> OnItemLooted;

    private void OnValidate()
    {
        if (itemSO == null)
            return;

        UpdateAppearance();
    }
    public void Initialize(ItemSO itemSO, int quantity)
    {
        this.itemSO = itemSO;
        this.quantity = quantity;
        canBePickedUp = false;
        UpdateAppearance();
    }

    private void UpdateAppearance()
    {
        sr.sprite = itemSO.icon;
        this.name = itemSO.GetLocalizedName();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canBePickedUp == true)
        {
            anim.Play("LootPickUp");
            PickUp();
            OnItemLooted?.Invoke(itemSO, quantity);
            Destroy(gameObject, 0.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canBePickedUp = true;
        }
    }

    public virtual void PickUp()
    {
        Sprite itemIcon = itemSO.icon;
        string localizedName = itemSO.GetLocalizedName();
        if(ItemPickupUIController.Instance != null)
        {
            ItemPickupUIController.Instance.ShowItemPickup(localizedName, itemIcon);
        }
    }
}
