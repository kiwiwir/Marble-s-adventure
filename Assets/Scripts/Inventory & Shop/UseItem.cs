using System.Collections;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    public void ApplyItemEffects(ItemSO itemSO)
    {
        if (itemSO.currentHealth > 0)
            StatsManager.Instance.UpdateCurrentHealth(itemSO.currentHealth);

        if (itemSO.moveSpeed > 0)
            StatsManager.Instance.UpdateSpeed(itemSO.moveSpeed);
            
        if (itemSO.sprintSpeed > 0)
            StatsManager.Instance.UpdateSprintSpeed(itemSO.sprintSpeed);

        if (itemSO.duration > 0)
            StartCoroutine(EffectTimer(itemSO, itemSO.duration));
    }

    private IEnumerator EffectTimer(ItemSO itemSO, float duration)
    {
        yield return new WaitForSeconds(duration);

        if (itemSO.moveSpeed > 0)
            StatsManager.Instance.UpdateSpeed(-itemSO.moveSpeed);

        if(itemSO.sprintSpeed > 0)
            StatsManager.Instance.UpdateSprintSpeed(-itemSO.sprintSpeed);

        if (itemSO.damage > 0)
            StatsManager.Instance.UpdateDamage(-itemSO.damage);
        
    }
}
