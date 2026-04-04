using System;
using UnityEngine;

public class EnemySword : MonoBehaviour
{
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerStats>();
        if (player != null)
        {
            GameEvents.EnemySwordHit?.Invoke();
        }
    }
}
