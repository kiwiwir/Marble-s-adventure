using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    [Header("Player Icon")]
    public RectTransform playerIcon;

    [Header("Location Points")]
    public RectTransform trainStationPoint;
    public RectTransform tunnelPoint;
    public RectTransform woodPoint;

    private Dictionary<string, RectTransform> locationPoints;

    private void Awake()
    {
        // Przypisanie scen do punktów na mapie
        locationPoints = new Dictionary<string, RectTransform>()
        {
            { "TrainStationScene", trainStationPoint },
            { "TunnelScene", tunnelPoint },
            { "WoodScene", woodPoint }
        };

        // Subskrybuj event ładowania sceny
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        MovePlayerIconToCurrentLocation();
    }

    private void OnDestroy()
    {
        // Ważne: odsubskrybuj event, żeby uniknąć błędów po zniszczeniu obiektu
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        MovePlayerIconToCurrentLocation();
    }

    public void MovePlayerIconToCurrentLocation()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (locationPoints.TryGetValue(currentScene, out RectTransform targetLocation))
        {
            playerIcon.anchoredPosition = targetLocation.anchoredPosition;
            Debug.Log($"Ikona gracza ustawiona dla sceny: {currentScene}");
        }
        else
        {
            Debug.LogWarning($"Brak przypisanego punktu mapy dla sceny: {currentScene}");
        }
    }
}
