using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerCombactScript : MonoBehaviour
{


    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    public float attackRange = 0.5f;
    public int attacDamage = 40;
    public float attackRate = 2f;
    float nexAttackTime = 0f;


    public void Attack(InputAction.CallbackContext value)
    {
        if (Time.time >= nexAttackTime)
        {
            if (value.started)
            {
                animator.SetTrigger("Attack");

                Collider2D[] HitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

                foreach (Collider2D enemy in HitEnemies)
                {
                    enemy.GetComponent<Enemy>().TakeDamage(attacDamage);
                }
                nexAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null)
            return;


        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
