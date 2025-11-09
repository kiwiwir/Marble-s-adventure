using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class BuildingTeleport : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Vector2 teleportPosition;    // Pozycja, do której gracz ma zostać przeniesiony
    public float fadeTime = 0.5f;       // Czas trwania animacji fade
    public Animator fadeAnim;           // Animator odpowiedzialny za fade

    [Header("Camera Confiner")]
    public PolygonCollider2D targetConfiner; // Confiner, który ma zostać aktywowany po teleportacji

    private bool isTeleporting = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTeleporting)
        {
            StartCoroutine(TeleportPlayer(collision.transform));
        }
    }

    private IEnumerator TeleportPlayer(Transform player)
    {
        isTeleporting = true;

        // Uruchomienie animacji przyciemnienia (fade-out)
        if (fadeAnim != null)
            fadeAnim.Play("FadeToDark");

        // Czekaj aż ekran się przyciemni
        yield return new WaitForSeconds(fadeTime);

        // Przenieś gracza na nową pozycję
        player.position = teleportPosition;

        // Zmiana confinera kamery
        CinemachineConfiner2D confiner = FindObjectOfType<CinemachineConfiner2D>();
        if (confiner != null && targetConfiner != null)
        {
            confiner.BoundingShape2D = targetConfiner;
            confiner.InvalidateCache(); // konieczne, żeby odświeżyć ograniczenia
        }

        // Uruchomienie animacji rozjaśnienia (fade-in)
        if (fadeAnim != null)
            fadeAnim.Play("FadeFromDark");

        // Czekaj aż fade się zakończy, zanim znowu pozwolisz na teleport
        yield return new WaitForSeconds(fadeTime);

        isTeleporting = false;
    }
}
