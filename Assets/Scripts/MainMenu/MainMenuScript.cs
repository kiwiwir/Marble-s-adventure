using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [Header("UI Panels")]
    //public GameObject loadPanel;
    public GameObject optionsPanel;
    public GameObject creditsPanel;
    public SceneChanger sceneChanger;

    [Header("Persistent Objects To Activate")]
    public string[] persistentObjectNames;

    public void OnStartClick()
    {
        AudioManager.Play("ButtonAffirmative");
        SaveLoadManager.Instance.ResetGameToDefault();
        sceneChanger.ChangeSceneWithFade("Cutscene01");
    }

    public void OnLoadClick()
    {
        AudioManager.Play("ButtonAffirmative");
        //SaveLoadManager.Instance.LoadGame();
        // Sprawdź czy istnieje savegame.json
        string path = Application.persistentDataPath + "/savegame.json";

        if (!File.Exists(path))
        {
            Debug.LogWarning("Brak pliku savegame.json — nie można wczytać gry!");
            AudioManager.Play("ButtonNegative"); 

            return; // NIE uruchamiamy Coroutine
        }

        // Jeśli save istnieje — normalnie uruchamiamy loading
        // Najpierw odblokuj wszystkie persistent objects
        StartCoroutine(ActivatePersistentObjectsAndLoad());
    }
    private IEnumerator ActivatePersistentObjectsAndLoad()
    {
        // Poczekaj 1 klatkę, żeby wszystko się ustabilizowało
        yield return null;

        foreach (string objName in persistentObjectNames)
        {
            GameObject obj = null;

            if (objName == "Player")
            {
                obj = GameManager.Instance?.Player;
            }
            else
            {
                var allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
                foreach (var go in allObjects)
                {
                    if (go.name == objName)
                    {
                        obj = go;
                        break;
                    }
                }
            }

            if (obj != null)
                obj.SetActive(true);
            else
                Debug.LogWarning($"Nie znaleziono persistent object o nazwie lub tagu: {objName}");
        }

        // Teraz załaduj grę
        SaveLoadManager.Instance.LoadGame();
        SaveLoadManager.Instance.ApplyLoadedSave();
    }

    public void OnOptionsClick()
    {
        AudioManager.Play("ButtonAffirmative");
        if (optionsPanel != null)
            optionsPanel.SetActive(true);
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
