using System.Collections.Generic;
using UnityEngine;

public class NPC_Talk : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Animator interactAnim;
    
    public List<DialogueSO> conversations;
    public DialogueSO currentConversation;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        RemoveFinishedDialogues();
    }

    private void Start()
    {
        QuestEvents.OnQuestAccepted += OnQuestAccepted_RemoveOfferings;
    }

    private void OnDestroy()
    {
        QuestEvents.OnQuestAccepted -= OnQuestAccepted_RemoveOfferings;
    }

    private void OnEnable()
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        anim.Play("Idle");
        interactAnim.Play("Open");
    }
    private void OnDisable()
    {
        interactAnim.Play("Close");
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (GameManager.Instance.DialogueManager.isDialogueActive)
                GameManager.Instance.DialogueManager.AdvanceDialogue();
            else
            {
                CheckForNewConversation();

                var player = GameManager.Instance.Player.GetComponent<PlayerMovement>();
                player.FaceTarget(transform);

                GameManager.Instance.DialogueManager.StartDialogue(currentConversation);
            }
        }
    }

    private void CheckForNewConversation()
    {
        for(int i = 0; i < conversations.Count; i++)
        {
            var convo = conversations[i];
            if(convo != null && convo.IsConditionMet())
            {
                currentConversation = convo;

                // Remove this if it's one-time only
                if (convo.removeAfterPlay)
                    conversations.RemoveAt(i);
                
                // Remove any other dialogues that should be clreared when this one plays (like quest completion)
                if(convo.removeTheseOnPlay != null && convo.removeTheseOnPlay.Count > 0)
                {
                    foreach(var toRemove in convo.removeTheseOnPlay)
                    {
                        conversations.Remove(toRemove);
                    }
                }
                
                break;
            }
        }
    }

    private void OnQuestAccepted_RemoveOfferings(QuestSO acceptedQuest)
    {
        for(int i = conversations.Count - 1; i >= 0; i--)
        {
            var convo = conversations[i];
            if(convo == null)
                continue;
            if(convo.offerQuestOnEnd == acceptedQuest)
            {
                conversations.RemoveAt(i);
            }
        }
    }

    private void RemoveFinishedDialogues()
    {
        for (int i = conversations.Count - 1; i >= 0; i--)
        {
            var c = conversations[i];
            if (c != null && GameManager.Instance.removedDialogues.Contains(c.dialogueID))
            {
                conversations.RemoveAt(i);
            }
        }
    }
}
