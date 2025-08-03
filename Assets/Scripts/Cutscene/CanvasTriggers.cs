using UnityEngine;

public class CanvasTriggers : MonoBehaviour
{
    public float hornDelay = 20f;

    void Start()
    {
        // Zagraj od razu dźwięk zwalniania pociągu
        AudioManager.Play("TrainSlowingDown", randomPitch: false);

        // Po 20 sekundach zagraj róg pociągu
        Invoke(nameof(PlayTrainHorn), hornDelay);
    }

    private void PlayTrainHorn()
    {
        AudioManager.Play("TrainHorn", randomPitch: false);
    }
}
