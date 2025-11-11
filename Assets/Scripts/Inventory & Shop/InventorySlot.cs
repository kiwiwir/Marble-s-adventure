using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{

    public ItemSO itemSO;
    public int quantity;

    public Image itemImage;
    public Image backgroundImage;
    public TMP_Text quantityText;

    private InventoryManager inventoryManager;
    private static ShopManager activeShop;

    [SerializeField] private ItemInfo itemInfo;

    // --- DRAG ---
    private static GameObject dragIcon;  // ikona przedmiotu w trakcie przeciągania
    private static InventorySlot draggedFrom; // slot, z którego przeciągamy
    private Canvas canvas;

    private void Start()
    {
        inventoryManager = GetComponentInParent<InventoryManager>();
        canvas = GetComponentInParent<Canvas>(); // potrzebny do pozycji w UI
    }

    private void OnEnable()
    {
        ShopKeeper.OnShopStateChanged += HandleShopStateChanged;
    }
    private void OnDisable()
    {
        ShopKeeper.OnShopStateChanged -= HandleShopStateChanged;
    }

    private void HandleShopStateChanged(ShopManager shopManager, bool isOpen)
    {
        activeShop = isOpen ? shopManager : null;
    }



    public void OnPointerClick(PointerEventData eventData)
    {
        if (quantity > 0)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (activeShop != null)
                {
                    activeShop.SellItem(itemSO);
                    quantity--;
                    UpdateUI();
                }
                else
                {
                    if (itemSO.currentHealth > 0 && StatsManager.Instance.currentHealth >= StatsManager.Instance.maxHealth)
                    {
                        AudioManager.Play("Error");
                        return;
                    }
                    inventoryManager.UseItem(this);
                }
            }
        }
    }

    // --- DRAG & DROP ---
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemSO == null) return;

        // zapamiętujemy slot, z którego przeciągamy
        draggedFrom = this;

        // tworzymy tymczasową ikonę
        dragIcon = new GameObject("DragIcon");
        dragIcon.transform.SetParent(canvas.transform, false);
        dragIcon.transform.SetAsLastSibling(); // na wierzchu

        Image img = dragIcon.AddComponent<Image>();
        img.sprite = itemSO.icon;
        img.raycastTarget = false; // żeby nie blokowało OnDrop

        RectTransform rt = dragIcon.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(64, 64); // rozmiar ikony
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragIcon != null)
        {
            dragIcon.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragIcon != null)
        {
            Destroy(dragIcon);
            dragIcon = null;
        }

        // sprawdź czy upuszczono poza inventoryPanel
        if (itemSO != null)
        {
            RectTransform panel = inventoryManager.inventoryPanel;
            if (!RectTransformUtility.RectangleContainsScreenPoint(panel, eventData.position, eventData.enterEventCamera))
            {
                // wyrzuć itemek
                inventoryManager.DropItem(this);
            }
        }
    }


    public void OnDrop(PointerEventData eventData)
    {
        if (draggedFrom != null && draggedFrom != this)
        {
            SwapItems(draggedFrom);
        }
    }

    private void SwapItems(InventorySlot otherSlot)
    {
        // zamiana itemów
        ItemSO tempItem = itemSO;
        int tempQuantity = quantity;

        itemSO = otherSlot.itemSO;
        quantity = otherSlot.quantity;

        otherSlot.itemSO = tempItem;
        otherSlot.quantity = tempQuantity;

        UpdateUI();
        otherSlot.UpdateUI();
    }

    public void UpdateUI()
    {
        if (quantity <= 0)
            itemSO = null;

        if (itemSO != null)
        {
            itemImage.sprite = itemSO.icon;
        }
        itemImage.gameObject.SetActive(itemSO != null);
        backgroundImage.gameObject.SetActive(itemSO != null);
        quantityText.text = (itemSO != null) ? quantity.ToString() : "";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemSO != null)
            itemInfo.ShowItemInfo(itemSO);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemInfo.HideItemInfo();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (itemSO != null)
            itemInfo.FollowMouse();
    }
}
