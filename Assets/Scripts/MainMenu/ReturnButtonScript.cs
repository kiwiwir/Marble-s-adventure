using UnityEngine;

public class ReturnButtonScript : MonoBehaviour
{
    public GameObject panelToClose;

    public void ClosePanel()
    {
        AudioManager.Play("ButtonDissenting");
        if (panelToClose != null)
            panelToClose.SetActive(false);
    }
}
