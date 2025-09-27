using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventoryPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            bool willBeActive = !inventoryPanel.activeSelf;
            if (willBeActive)
                AudioManager.Play("Menu_In");
            else
                AudioManager.Play("Menu_Out");
            inventoryPanel.SetActive(willBeActive);
        }
    }
}
