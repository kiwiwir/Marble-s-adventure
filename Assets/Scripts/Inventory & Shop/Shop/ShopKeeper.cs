/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShopKeeper : MonoBehaviour, IMenu
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
        if (playerInRange && Input.GetButtonDown("Interact"))
        {
            if (GameManager.Instance.DialogueManager != null && GameManager.Instance.DialogueManager.isDialogueActive)
            {
               GameManager.Instance.DialogueManager.AdvanceDialogue();
                return;
            }

            if (!dialoguePlayed && shopkeeperDialogue != null && !dialogueRunning)
            {
                StartCoroutine(StartDialogueThenShop());
            }
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
        GameManager.Instance.DialogueManager.StartDialogue(shopkeeperDialogue);

        // czekaj, aż dialog się zakończy
        yield return new WaitUntil(() => GameManager.Instance.DialogueManager.isDialogueActive == false);

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
        AudioManager.Play("Menu_In");

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
        AudioManager.Play("Menu_Out");
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
        if (GameManager.Instance.DialogueManager != null && GameManager.Instance.DialogueManager.isDialogueActive)
            return;

        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            interactAnim.Play("Close");
            dialoguePlayed = false; // pozwól na ponowne odtworzenie dialogu przy następnym podejściu
        }
    }
}*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShopKeeper : MonoBehaviour, IMenu
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

    private void Start()
    {
        GlobalMenuManager.Instance.Register(this);
    }

    void Update()
    {
        if (playerInRange && Input.GetButtonDown("Interact"))
        {
            // Jeśli coś mówi — skip dialogu
            if (GameManager.Instance.DialogueManager != null &&
                GameManager.Instance.DialogueManager.isDialogueActive)
            {
                GameManager.Instance.DialogueManager.AdvanceDialogue();
                return;
            }

            // Najpierw dialog → potem sklep
            if (!dialoguePlayed && shopkeeperDialogue != null && !dialogueRunning)
            {
                StartCoroutine(StartDialogueThenShop());
                return;
            }

            // Sklep już aktywowany wcześniej → Toggle
            //GlobalMenuManager.Instance.Toggle(this);
            if (!isShopOpen)
                OpenShop();
            else
                CloseShop();
        }
    }
    private void OnDestroy()
    {
        if (GlobalMenuManager.Instance != null)
            GlobalMenuManager.Instance.Unregister(this);
    }


    private IEnumerator StartDialogueThenShop()
    {
        dialogueRunning = true;

        GameManager.Instance.DialogueManager.StartDialogue(shopkeeperDialogue);

        yield return new WaitUntil(() =>
            GameManager.Instance.DialogueManager.isDialogueActive == false);

        dialoguePlayed = true;
        dialogueRunning = false;

        GlobalMenuManager.Instance.RequestOpen(this);
    }

    // ——————————————————————————————
    // IMenu IMPLEMENTACJA
    // ——————————————————————————————
    public void Open()
    {
        OpenShop();
    }

    public void Close()
    {
        CloseShop();
    }

    public void CloseInstant()
    {
        shopCanvasGroup.alpha = 0;
        shopCanvasGroup.interactable = false;
        shopCanvasGroup.blocksRaycasts = false;
        shopkeeperCam.gameObject.SetActive(false);

        isShopOpen = false;
        Time.timeScale = 1;
        currentShopKeeper = null;
    }

    public bool IsOpen => isShopOpen;

    // ——————————————————————————————
    // SKLEP NORMALNIE
    // ——————————————————————————————
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

        AudioManager.Play("Menu_In");

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

        AudioManager.Play("Menu_Out");
    }

    // ——————————————————————————————
    // SELECTOR KATEGORII
    // ——————————————————————————————
    public void OpenPotionsShop() => shopManager.PopulateShopItems(shopPotionsItems);
    public void OpenEdibleShop() => shopManager.PopulateShopItems(shopEdibleItems);
    public void OpenOtherShop() => shopManager.PopulateShopItems(shopOtherItems);

    // ——————————————————————————————
    // TRIGGER
    // ——————————————————————————————
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
        if (GameManager.Instance.DialogueManager != null &&
            GameManager.Instance.DialogueManager.isDialogueActive)
            return;

        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            interactAnim.Play("Close");

            // dialog od nowa przy następnym podejściu
            dialoguePlayed = false;
        }
    }
}

