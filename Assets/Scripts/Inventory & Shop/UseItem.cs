using System.Collections;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    public void ApplyItemEffects(ItemSO itemSO)
    {
        if (itemSO.currentHealth > 0)
            StatsManager.Instance.UpdateCurrentHealth(itemSO.currentHealth);

        if (itemSO.maxHealth > 0)
            StatsManager.Instance.UpdateMaxHealth(itemSO.maxHealth);

        if (itemSO.moveSpeed > 0)
            StatsManager.Instance.UpdateSpeed(itemSO.moveSpeed);

        if (itemSO.duration > 0)
            StartCoroutine(EffectTimer(itemSO, itemSO.duration));
    }
    
    private IEnumerator EffectTimer(ItemSO itemSO, float duration)
    {
        yield return new WaitForSeconds(duration);

        if (itemSO.currentHealth > 0)
            StatsManager.Instance.UpdateCurrentHealth(-itemSO.currentHealth);

        if (itemSO.maxHealth > 0)
            StatsManager.Instance.UpdateMaxHealth(-itemSO.maxHealth);

        if (itemSO.moveSpeed > 0)
            StatsManager.Instance.UpdateSpeed(-itemSO.moveSpeed);
    }
}
