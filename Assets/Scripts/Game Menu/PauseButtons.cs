using UnityEngine;

public class PauseButtons : MonoBehaviour
{
    public SceneChanger sceneChanger;
    public PauseController pauseController; // referencja do SceneChanger

    // Save Game
    public void OnSaveClick()
    {
        AudioManager.Play("ButtonAffirmative");
        Debug.Log("Save Game clicked");
    }

    // Main Menu
    public void OnMainMenuClick()
    {
        AudioManager.Play("ButtonAffirmative");

        // Zmiana sceny przez SceneChanger z fade

        Debug.Log("Main Menu clicked");
        pauseController.ClosePauseInstant();
        sceneChanger.ChangeSceneWithFade("MainMenuScene");

    }

    // Exit Game
    public void OnExitClick()
    {
        AudioManager.Play("ButtonAffirmative");
        Debug.Log("Exit Game clicked");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
