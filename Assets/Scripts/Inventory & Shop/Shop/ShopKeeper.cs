/*using System.Collections.Generic;
using UnityEngine;
using System;

public class ShopKeeper : MonoBehaviour
{
    public static ShopKeeper currentShopKeeper;
    public Animator interactAnim;
    public CanvasGroup shopCanvasGroup;
    public ShopManager shopManager;

    [SerializeField] private List<ShopItems> shopPotionsItems;
    [SerializeField] private List<ShopItems> shopEdibleItems;
    [SerializeField] private List<ShopItems> shopOtherItems;

    public static event Action<ShopManager, bool> OnShopStateChanged;
    private bool playerInRange;
    private bool isShopOpen;

    [SerializeField] private DialogueSO shopkeeperDialogue;
    private bool dialoguePlayed;

    void Update()
    {
        if (playerInRange)
        {
            if (Input.GetButtonDown("Interact"))
            {
                if (!isShopOpen)
                {
                    Time.timeScale = 0;
                    currentShopKeeper = this;
                    isShopOpen = true;
                    OnShopStateChanged?.Invoke(shopManager, true);
                    shopCanvasGroup.alpha = 1;
                    shopCanvasGroup.interactable = true;
                    shopCanvasGroup.blocksRaycasts = true;
                    OpenPotionsShop();
                }
                else
                {
                    Time.timeScale = 1;
                    currentShopKeeper = null;
                    isShopOpen = false;
                    OnShopStateChanged?.Invoke(shopManager, false);
                    shopCanvasGroup.alpha = 0;
                    shopCanvasGroup.interactable = false;
                    shopCanvasGroup.blocksRaycasts = false;
                }
            }
        }
    }

    public void OpenPotionsShop()
    {
        shopManager.PopulateShopItems(shopPotionsItems);
    }
    public void OpenEdibleShop()
    {
        shopManager.PopulateShopItems(shopEdibleItems);
    }
    public void OpenOtherShop()
    {
        shopManager.PopulateShopItems(shopOtherItems);
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            interactAnim.Play("Open");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            interactAnim.Play("Close");
        }
    }
}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShopKeeper : MonoBehaviour
{
    public static ShopKeeper currentShopKeeper;
    public Animator interactAnim;
    public CanvasGroup shopCanvasGroup;
    public ShopManager shopManager;

    [SerializeField] private DialogueSO shopkeeperDialogue;

    [SerializeField] private List<ShopItems> shopPotionsItems;
    [SerializeField] private List<ShopItems> shopEdibleItems;
    [SerializeField] private List<ShopItems> shopOtherItems;

    [SerializeField] private Camera shopkeeperCam;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 0, -1);

    public static event Action<ShopManager, bool> OnShopStateChanged;
    private bool playerInRange;
    private bool isShopOpen;
    private bool dialoguePlayed;
    private bool dialogueRunning;

    void Update()
    {
        // 🧩 Jeśli gracz w zasięgu i naciśnie "Interact"
        if (playerInRange && Input.GetButtonDown("Interact"))
        {
            // ⏳ Jeśli trwa dialog — przewiń go dalej
            if (DialogueManager.Instance != null && DialogueManager.Instance.isDialogueActive)
            {
                DialogueManager.Instance.AdvanceDialogue();
                return;
            }

            // 💬 Jeśli dialog nie był jeszcze odtworzony — odpal go
            if (!dialoguePlayed && shopkeeperDialogue != null && !dialogueRunning)
            {
                StartCoroutine(StartDialogueThenShop());
            }
            // 🛍️ Jeśli dialog już był — otwórz/zamknij sklep
            else if (dialoguePlayed)
            {
                if (!isShopOpen)
                    OpenShop();
                else
                    CloseShop();
            }
        }
    }

    private IEnumerator StartDialogueThenShop()
    {
        dialogueRunning = true;

        // uruchom dialog
        DialogueManager.Instance.StartDialogue(shopkeeperDialogue);

        // czekaj, aż dialog się zakończy
        yield return new WaitUntil(() => DialogueManager.Instance.isDialogueActive == false);

        // po zakończeniu dialogu — otwórz sklep
        dialoguePlayed = true;
        dialogueRunning = false;
        OpenShop();
    }

    private void OpenShop()
    {
        Time.timeScale = 0;
        currentShopKeeper = this;
        isShopOpen = true;
        OnShopStateChanged?.Invoke(shopManager, true);
        shopCanvasGroup.alpha = 1;
        shopCanvasGroup.interactable = true;
        shopCanvasGroup.blocksRaycasts = true;

        shopkeeperCam.transform.position = transform.position + cameraOffset;
        shopkeeperCam.gameObject.SetActive(true);

        OpenPotionsShop();
    }

    private void CloseShop()
    {
        Time.timeScale = 1;
        currentShopKeeper = null;
        isShopOpen = false;
        OnShopStateChanged?.Invoke(shopManager, false);
        shopCanvasGroup.alpha = 0;
        shopCanvasGroup.interactable = false;
        shopCanvasGroup.blocksRaycasts = false;

        shopkeeperCam.gameObject.SetActive(false);
    }

    public void OpenPotionsShop() => shopManager.PopulateShopItems(shopPotionsItems);
    public void OpenEdibleShop() => shopManager.PopulateShopItems(shopEdibleItems);
    public void OpenOtherShop() => shopManager.PopulateShopItems(shopOtherItems);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            interactAnim.Play("Open");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Jeśli trwa dialog — ignoruj wyjście z zasięgu
        if (DialogueManager.Instance != null && DialogueManager.Instance.isDialogueActive)
            return;

        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            interactAnim.Play("Close");
            dialoguePlayed = false; // pozwól na ponowne odtworzenie dialogu przy następnym podejściu
        }
    }
}
