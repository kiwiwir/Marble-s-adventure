using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.XR;

public class Enemy_Movement : MonoBehaviour
{
    public float speed;
    //private int facingDirection = -1;
    private EnemyState enemyState;

    private Rigidbody2D rb;
    private Transform player;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ChangeState(EnemyState.Idle);
    }

    void Update()
    {
        if (enemyState == EnemyState.Chasing)
        {
            Vector2 direction = (player.position - transform.position).normalized;

            // Parametry do Blend Tree
            anim.SetFloat("MoveX", direction.x);
            anim.SetFloat("MoveY", direction.y);

            //Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (player == null)
            {
                player = collision.transform;
            }
            ChangeState(EnemyState.Chasing);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            rb.linearVelocity = Vector2.zero;
            ChangeState(EnemyState.Idle);
        }
    }

    void ChangeState(EnemyState newState)
    {
        // Exit current animation
        if (enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", false);
        else if (enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", false);

        // Update current state
        enemyState = newState;

        // Update new animation
        if (enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", true);
        else if (enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", true);
    }
}

public enum EnemyState
{
    Idle,
    Chasing,
}