using UnityEngine;

public class QuestBoard : MonoBehaviour, IInteractable
{
    [SerializeField] private QuestSO questToOffer;
    private bool playerInRange;

    public void Interact()
    {
        QuestEvents.OnQuestOfferRequested?.Invoke(questToOffer);
        Debug.Log("QuestBoard: Interact()");
    }
    public bool CanInteract()
    {
        return true; // Always interactable
    }

    /*private void Update()
    {
        if (playerInRange && Input.GetButtonDown("Interact"))
        {
            QuestEvents.OnQuestOfferRequested?.Invoke(questToOffer);
        }
    }*/
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
}
