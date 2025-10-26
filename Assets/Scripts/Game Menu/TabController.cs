using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    public Image[] tabImages;
    public GameObject[] pages;

    void Start()
    {
        ActiveTab(0);
    }

    public void ActiveTab(int tabNo)
    {
        // Dźwięk przełączenia tabów
        if (AudioManager.Instance != null && AudioManager.Instance.isActiveAndEnabled)
            AudioManager.Play("Menu_In");

        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == tabNo);
            //tabImages[i].color = (i == tabNo) ? Color.white : Color.grey;
        }

        // Jeśli to pierwsza zakładka (player page) — aktualizuj statystyki
        if (tabNo == 0)
        {
            StatsUI stats = pages[tabNo].GetComponent<StatsUI>();
            if (stats != null)
            {
                stats.UpdateAllStats();
            }
        }
    }
}

