using UnityEngine;

public class QuestBoard : MonoBehaviour
{
    [SerializeField] private QuestSO questToOffer;
    [SerializeField] private QuestSO questToTurnIn;
    private bool playerInRange;
    [SerializeField] private QuestLogUI questLogUI;
    [SerializeField] private SpriteRenderer exclamationMark;

    private void OnEnable()
    {
        QuestEvents.OnQuestStateChanged += UpdateQuestIcon;
    }

    private void OnDisable()
    {
        QuestEvents.OnQuestStateChanged -= UpdateQuestIcon;
    }

    private void Start()
    {
        UpdateQuestIcon();
    }

    private void Awake()
    {
        if (questLogUI == null)
        {
            GameObject questLogObj = GameObject.Find("QuestLog");
            if (questLogObj != null)
            {
                questLogUI = questLogObj.GetComponent<QuestLogUI>();
            }
            else
            {
                Debug.LogWarning("QuestLog object not found in scene!");
            }
        }
    }
    

    private void Update()
    {
        if (playerInRange && Input.GetButtonDown("Interact"))
        {
            if (questLogUI != null)
            {
                questLogUI.UpdateAllQuestProgress();
                questLogUI.RefreshQuestList();
            }

            bool canTurnIn = questToTurnIn != null && QuestEvents.IsQuestComplete?.Invoke(questToTurnIn) == true;

            if (canTurnIn)
            {
                QuestEvents.OnQuestTurnInRequested?.Invoke(questToTurnIn);
            }
            else
            {
                QuestEvents.OnQuestOfferRequested?.Invoke(questToOffer);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Press 'E' to view quests.");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void UpdateQuestIcon(QuestSO _)
    {
        UpdateQuestIcon();
    }

    private void UpdateQuestIcon()
    {
        if (exclamationMark == null)
            return;

        bool canTurnIn = questToTurnIn != null &&
                        (QuestEvents.IsQuestComplete?.Invoke(questToTurnIn) ?? false);

        bool canOffer = questToOffer != null &&
                        !(QuestEvents.IsQuestActive?.Invoke(questToOffer) ?? false);

        // Jeśli quest został już ukończony i dodany do completed listy
        bool alreadyCompleted = false;
        if (questToOffer != null)
        {
            alreadyCompleted = FindObjectOfType<QuestManager>()
                .GetCompleteQuest(questToOffer);
        }

        if (canTurnIn || (canOffer && !alreadyCompleted))
            exclamationMark.enabled = true;
        else
            exclamationMark.enabled = false;
    }
}
