using UnityEngine;
using System.Collections.Generic;

public class LocationHistoryTracker : MonoBehaviour
{
    public static LocationHistoryTracker Instance;
    private readonly HashSet<LocationSO> locationsVisited = new HashSet<LocationSO>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RecordLocation(LocationSO locationSO)
    {
        if(locationsVisited.Add(locationSO))
        {
            Debug.Log($"Just visited {locationSO.displayName}.");
        }
    }

    public bool HasVisited(LocationSO locationSO)
    {
        return locationsVisited.Contains(locationSO);
    }
}
