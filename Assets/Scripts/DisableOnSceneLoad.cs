using System.Collections;
using UnityEngine;

public class DisableOnSceneLoad : MonoBehaviour
{
    [Header("Persistent objects do wyłączenia w tej scenie")]
    public string[] persistentObjectNames; // nazwy obiektów persistent, które chcemy wyłączyć

    private void Start()
    {
        StartCoroutine(DisablePersistentObjectsNextFrame());
    }

    private IEnumerator DisablePersistentObjectsNextFrame()
    {
        // Poczekaj jedną klatkę, aby wszystkie persistent objects były już w scenie
        yield return null;

        foreach (string objName in persistentObjectNames)
        {
            GameObject obj = null;

            // Szukamy po nazwie w scenie (aktywny lub nieaktywny)
            var allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (var go in allObjects)
            {
                if (go.name == objName)
                {
                    obj = go;
                    break;
                }
            }

            if (obj != null)
            {
                obj.SetActive(false);
                Debug.Log($"Wyłączono persistent object: {objName}");
            }
            else
            {
                Debug.LogWarning($"Nie znaleziono persistent object o nazwie: {objName}");
            }
        }
    }
}
