using UnityEditor;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;
    void Start()
    {
        menuCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("ToggleMenu"))
        {
            menuCanvas.SetActive(!menuCanvas.activeSelf);
        }
    }
}
