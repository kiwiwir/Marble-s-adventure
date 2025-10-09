using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class StatsUI : MonoBehaviour
{
    public GameObject[] statsSlots;

    private void OnEnable()
    {
        // Podpinamy się pod zdarzenie zmiany języka
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;

        // Na wypadek gdyby język zmienił się zanim UI się włączyło
        UpdateAllStats();
    }

    private void OnDisable()
    {
        // Odpinamy event, żeby uniknąć wycieków pamięci
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    // Event wywoływany automatycznie przy każdej zmianie języka
    private void OnLocaleChanged(UnityEngine.Localization.Locale newLocale)
    {
        UpdateAllStats();
    }

    private string Localize(string key)
    {
        string lang = LocalizationSettings.SelectedLocale.Identifier.Code;
        return key switch
        {
            "max_health" => lang == "pl" ? "Maksymalne zdrowie" : "Max Health",
            "current_health" => lang == "pl" ? "Aktualne zdrowie" : "Current Health",
            "speed" => lang == "pl" ? "Szybkość" : "Speed",
            "damage" => lang == "pl" ? "Obrażenia" : "Damage",
            _ => key
        };
    }

    public void UpdateMaxHealth()
    {
        statsSlots[0].GetComponentInChildren<TMP_Text>().text =
            $"{Localize("max_health")}: {StatsManager.Instance.maxHealth}";
    }

    public void UpdateCurrentHealth()
    {
        statsSlots[1].GetComponentInChildren<TMP_Text>().text =
            $"{Localize("current_health")}: {StatsManager.Instance.currentHealth}";
    }

    public void UpdateSpeed()
    {
        statsSlots[2].GetComponentInChildren<TMP_Text>().text =
            $"{Localize("speed")}: {StatsManager.Instance.moveSpeed}";
    }

    public void UpdateDamage()
    {
        statsSlots[3].GetComponentInChildren<TMP_Text>().text =
            $"{Localize("damage")}: {StatsManager.Instance.damage}";
    }

    public void UpdateAllStats()
    {
        UpdateDamage();
        UpdateSpeed();
        UpdateMaxHealth();
        UpdateCurrentHealth();
    }
}
