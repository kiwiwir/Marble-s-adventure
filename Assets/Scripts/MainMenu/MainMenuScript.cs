using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject loadPanel;
    public GameObject optionsPanel;
    public GameObject creditsPanel;
    public SceneChanger sceneChanger;


    /*[Header("Save System")]
    public MainMenuSaveController saveController;*/

    public void OnStartClick()
    {
        /*if (GameManager.Instance != null)
        {
            GameManager.Instance.ClearPendingLoad();
        }

        AudioManager.Play("LoadGame");*/
        //SceneManager.LoadScene("Cutscene01");
        sceneChanger.ChangeSceneWithFade("Cutscene01");
    }

    public void OnLoadClick()
    {
        AudioManager.Play("ButtonAffirmative");
    }

    public void OnOptionsClick()
    {
        AudioManager.Play("ButtonAffirmative");
        if (optionsPanel != null)
            optionsPanel.SetActive(true);
        if (loadPanel != null)
            loadPanel.SetActive(false);
        if (creditsPanel != null)
            creditsPanel.SetActive(false);
    }

    public void OnCreditsClick()
    {
        AudioManager.Play("ButtonAffirmative");
        if (creditsPanel != null)
            creditsPanel.SetActive(true);
        if (optionsPanel != null)
            optionsPanel.SetActive(false);
        if (loadPanel != null)
            loadPanel.SetActive(false);
    }
    
    public void OnExitClick()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            AudioManager.Play("ButtonAffirmative");
            Application.Quit();
            Debug.Log("Application Quit");
    }
}
