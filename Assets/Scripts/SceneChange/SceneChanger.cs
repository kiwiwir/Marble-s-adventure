using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneToLoad;
    public Animator fadeAnim;
    public float fadeTime = 0.5f;

    private bool sceneLoading = false;

    public Vector2 newPlayerPosition;
    private Transform player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !sceneLoading)
        {
            player = collision.transform;
            StartSceneChange();
        }
    }

    public void ChangeSceneWithFade(string sceneName)
    {
        if (sceneLoading) return;

        sceneToLoad = sceneName;
        StartSceneChange();
    }

    private void StartSceneChange()
    {
        sceneLoading = true;

        if (fadeAnim != null)
            fadeAnim.Play("FadeToDark");

        StartCoroutine(DelayFade());
    }

    IEnumerator DelayFade()
    {
        float time = 0f;
        float startVolume = AudioManager.Instance != null ? AudioManager.MusicSource.volume : 1f;

        // Wyciszanie muzyki bez zmiany ustawień
        while (time < fadeTime)
        {
            time += Time.deltaTime;
            float t = time / fadeTime;
            if (AudioManager.Instance != null)
                AudioManager.MusicSource.volume = Mathf.Lerp(startVolume, 0f, t);
            yield return null;
        }

        if (player != null)
            player.position = newPlayerPosition;

        SceneManager.LoadScene(sceneToLoad);
    }
}
