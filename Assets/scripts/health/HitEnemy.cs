using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEnemy : MonoBehaviour
{
    [SerializeField] private float movementdistance;
    [SerializeField] private float damage;
    private PlayerController Player;

    private void Start()
    {
        Player = GetComponent<PlayerController>(); // Ottieni il riferimento al componente PlayerController
    }

    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "Enemy") && (Player.Correre == true) && (Player.gigi == false))
        {
            
            collision.GetComponent<EnemyHealth>().TakeDamage(damage);
        }
        else if((collision.tag == "Enemy") && (Player.gigi == true))
        {
            collision.GetComponent<EnemyHealth>().TakeDamage(damage * 4);
        }
    }
}
