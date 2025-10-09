using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;
    public TabController tabController;
    private bool isMenuOpen = false;

    void Start()
    {
        menuCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("ToggleMenu"))
        {
            if (!isMenuOpen)
            {
                // Otwieranie menu
                //AudioManager.Play("Menu_In");
                menuCanvas.SetActive(true);
                isMenuOpen = true;

                // Aktywuj pierwszą zakładkę (player page)
                tabController.ActiveTab(0);
            }
            else
            {
                // Zamykanie menu
                AudioManager.Play("Menu_Out");
                menuCanvas.SetActive(false);
                isMenuOpen = false;
            }
        }
    }
}

