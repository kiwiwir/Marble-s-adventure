using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private IInteractable interactableInRange = null;
    public GameObject interactionIcon;
    public Animator interactionIconAnimator;

    private bool isHidingIcon = false;

    void Start()
    {
        interactionIcon.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            Debug.Log("Interact pressed");
            interactableInRange?.Interact();
            HideIconWithAnimation();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
        {
            interactableInRange = interactable;
            interactionIcon.SetActive(true);

            if (interactionIconAnimator != null)
                interactionIconAnimator.Play("Open");

            Debug.Log("Interactable detected: " + collision.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable == interactableInRange)
        {
            interactableInRange = null;
            HideIconWithAnimation();
        }
    }

    private void HideIconWithAnimation()
    {
        if (interactionIcon.activeSelf && interactionIconAnimator != null)
        {
            interactionIconAnimator.Play("Close");
            if (!isHidingIcon)
                StartCoroutine(HideIconAfterClose());
        }
    }

    private IEnumerator HideIconAfterClose()
    {
        isHidingIcon = true;
        yield return new WaitForSeconds(0.3f); // czas trwania animacji "Close"
        interactionIcon.SetActive(false);
        isHidingIcon = false;
    }
}
