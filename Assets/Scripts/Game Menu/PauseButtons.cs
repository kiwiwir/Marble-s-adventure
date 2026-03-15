using UnityEngine;
using TMPro;
using System.Collections;

public class PauseButtons : MonoBehaviour
{
    public SceneChanger sceneChanger;
    public PauseController pauseController; // referencja do SceneChanger
    public Transform player;

        [Header("Save UI")]
    public TMP_Text saveText;
    public CanvasGroup saveTextCanvas;
    public float fadeDuration = 0.5f;
    public float visibleTime = 1.5f;

    private Coroutine saveCoroutine;

    [Header("Persistent Objects To Deactivate")]
    public string[] persistentObjectNames;

    [Header("Pause Canvas")]
    public CanvasGroup pauseCanvas;

    // Save Game
    public void OnSaveClick()
    {
        AudioManager.Play("ButtonAffirmative");
        Debug.Log("Save Game clicked");
        
        SaveLoadManager.Instance.SaveGame(player.position);

        // przerwij poprzednią animację jeśli kliknięto szybko kilka razy
        if (saveCoroutine != null)
            StopCoroutine(saveCoroutine);
        StartCoroutine(ShowSaveText());
    }
    private IEnumerator ShowSaveText()
    {
        // ZAWSZE zaczynamy od zera
        saveTextCanvas.alpha = 0f;

        // Fade in
        yield return Fade(0f, 1f);

        // Widoczny
        yield return new WaitForSecondsRealtime(visibleTime);

        // Fade out
        yield return Fade(1f, 0f);
    }

    private IEnumerator Fade(float from, float to)
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            saveTextCanvas.alpha = Mathf.Lerp(from, to, t / fadeDuration);
            yield return null;
        }

        saveTextCanvas.alpha = to;
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

        // WYŁĄCZ persistent objects
        DeactivatePersistentObjects();

        // WYŁĄCZ canvas pause menu
        DisablePauseCanvas();

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
    private void DeactivatePersistentObjects()
    {
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
                obj.SetActive(false);
            else
                Debug.LogWarning($"Nie znaleziono persistent object: {objName}");
        }
    }
    private void DisablePauseCanvas()
    {
        if (pauseCanvas != null)
        {
            pauseCanvas.alpha = 0f;
            pauseCanvas.interactable = false;
            pauseCanvas.blocksRaycasts = false;
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
