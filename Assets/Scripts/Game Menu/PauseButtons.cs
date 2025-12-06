using UnityEngine;

public class PauseButtons : MonoBehaviour
{
    public SceneChanger sceneChanger;
    public PauseController pauseController; // referencja do SceneChanger
    public Transform player;

    // Save Game
    public void OnSaveClick()
    {
        AudioManager.Play("ButtonAffirmative");
        Debug.Log("Save Game clicked");
        
        //GameManager.Instance.SaveGame(player.transform.position);
        SaveLoadManager.Instance.SaveGame(player.position);
    }

    // Main Menu
    public void OnMainMenuClick()
    {
        AudioManager.Play("ButtonAffirmative");

        // Zmiana sceny przez SceneChanger z fade

        Debug.Log("Main Menu clicked");
        //pauseController.ClosePauseInstant();
        //sceneChanger.ChangeSceneWithFade("MainMenuScene");
        if (pauseController != null)
            pauseController.Close();

        // dynamiczne wyszukiwanie SceneChanger w aktywnej scenie
        SceneChanger sc = FindObjectOfType<SceneChanger>();
        if (sc != null)
        {
            sc.ChangeSceneWithFade("MainMenuScene");
        }
        else
        {
            Debug.LogWarning("SceneChanger not found in scene! Cannot change scene.");
        }
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
