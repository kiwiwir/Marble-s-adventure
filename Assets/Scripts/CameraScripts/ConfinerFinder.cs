using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapBoundsFinder : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CinemachineConfiner2D confiner = GetComponent<CinemachineConfiner2D>();
        if (confiner == null)
        {
            Debug.LogWarning("MapBoundsFinder: Brak komponentu CinemachineConfiner2D na tym obiekcie.");
            return;
        }

        GameObject confinerObj = GameObject.FindWithTag("Confiner");
        if (confinerObj == null)
        {
            Debug.LogWarning("MapBoundsFinder: Nie znaleziono obiektu z tagiem 'Confiner' w scenie.");
            return;
        }

        PolygonCollider2D polygon = confinerObj.GetComponent<PolygonCollider2D>();
        if (polygon == null)
        {
            Debug.LogWarning("MapBoundsFinder: Obiekt 'Confiner' nie ma komponentu PolygonCollider2D.");
            return;
        }

        confiner.BoundingShape2D = polygon;
    }
}
