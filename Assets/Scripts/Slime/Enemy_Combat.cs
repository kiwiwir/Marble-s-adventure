using UnityEngine;

public class Enemy_Combat : MonoBehaviour
{
    public int damage = 1;
    public Transform attackPoint;
    public float weaponRange;
    public float knockbackForce;
    public float stunTime;
    public LayerMask playerLayer;
    /*public void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer);

        if (hits.Length > 0)
        {
            hits[0].GetComponent<PlayerHealth>().ChangeHealth(-damage);
            hits[0].GetComponent<PlayerMovement>().Knockback(transform, knockbackForce, stunTime);

            AudioManager.Play("EnemyHitsDamage");
        }
    }*/
    public void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer);

        foreach (var hit in hits)
        {
            PlayerHealth ph = hit.GetComponentInParent<PlayerHealth>();
            PlayerMovement pm = hit.GetComponentInParent<PlayerMovement>();

            if (ph != null && pm != null)
            {
                ph.ChangeHealth(-damage);
                pm.Knockback(transform, knockbackForce, stunTime);
                AudioManager.Play("EnemyHitsDamage");
                break;
            }
        }
    }
}
