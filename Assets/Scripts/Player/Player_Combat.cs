using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player_Combat : MonoBehaviour
{
    public Animator anim;

    public float cooldown = 2;
    private float timer;
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    public void Attack()
    {
        if (timer <= 0)
        {
            timer = cooldown;
            anim.SetBool("isAttacking", true);
        }
    }

    public void FinishAttacking()
    {
        anim.SetBool("isAttacking", false);
    }
}
