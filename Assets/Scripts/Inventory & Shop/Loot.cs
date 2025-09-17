using UnityEngine;

public class Loot : MonoBehaviour
{
    public ItemSO itemSO;
    public SpriteRenderer sr;
    public Animator anim;

    public int quantity;

    private void OnValidate()
    {
        if (itemSO == null)
            return;

        sr.sprite = itemSO.icon;
        this.name = itemSO.GetLocalizedName();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.Play("LootPickUp");
            Destroy(gameObject, 0.5f);
        }
    }
}
