using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Animator animator;
    public int maxHealth = 100;
    int currentHealt;

    void Start()
    {
        currentHealt = maxHealth;

    }

    public void TakeDamage(int damage)
    {
        currentHealt -= damage;

        animator.SetTrigger("Hurt");

        if(currentHealt <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetBool("IsDead", true);

        Invoke("Disattiva", 0.5f);

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
