using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DisableBoundaryOnPlayerLayer : MonoBehaviour
{
    public string playerTag = "Player";                     // tag gracza
    public List<TilemapCollider2D> collidersToEnable;       // collidery do włączenia, gdy sortingOrder != 2
    public List<TilemapCollider2D> collidersToDisable;      // collidery do wyłączenia, gdy sortingOrder == 2

    private SpriteRenderer playerSR;

    private void Update()
    {
        // jeśli gracza jeszcze nie znaleziono, spróbuj go znaleźć po tagu
        if (playerSR == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
            if (playerObj != null)
                playerSR = playerObj.GetComponent<SpriteRenderer>();
            else
                return; // gracz jeszcze nie istnieje w scenie
        }

        if (playerSR != null)
        {
            if (playerSR.sortingOrder == 2)
            {
                // wyłącz wszystkie w collidersToDisable
                foreach (var col in collidersToDisable)
                {
                    if (col != null && col.enabled)
                    {
                        col.enabled = false;
                        Debug.Log("Disabled collider: " + col.name);
                    }
                }

                // włącz wszystkie w collidersToEnable tylko jeśli są wyłączone
                foreach (var col in collidersToEnable)
                {
                    if (col != null && !col.enabled)
                    {
                        col.enabled = true;
                        Debug.Log("Enabled collider: " + col.name);
                    }
                }
            }
            else
            {
                // przy sortingOrder != 2 odwracamy: włączamy disable list, wyłączamy enable list
                foreach (var col in collidersToDisable)
                {
                    if (col != null && !col.enabled)
                    {
                        col.enabled = true;
                        Debug.Log("Enabled collider: " + col.name);
                    }
                }

                foreach (var col in collidersToEnable)
                {
                    if (col != null && col.enabled)
                    {
                        col.enabled = false;
                        Debug.Log("Disabled collider: " + col.name);
                    }
                }
            }
        }
    }
}
