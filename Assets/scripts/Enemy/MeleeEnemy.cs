using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;
    private bool PlayerInSight;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;
    AudioManager audioManager;

    //References
    private Animator anim;
    private Health playerHealth;
    private EnemyPatrol enemyPatrol;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
        playerHealth = GameObject.FindGameObjectWithTag("player").GetComponent<Health>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        //Attack only when player in sight?
        if (PlayerInSight)
        {

            FacePlayer();
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("MeleeAttack");
            }
        }

        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Se il collider che entra ? il player
        if (other.CompareTag("player"))
        {
            //Debug.Log("polpetta");
            // Segnala che il player ? nel range del nemico
            PlayerInSight = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Se il collider che entra ? il player
        if (other.CompareTag("player"))
        {
            // Segnala che il player ? nel range del nemico
            PlayerInSight = false;
        }

    }

    private void FacePlayer()
    { // Controlla la posizione del giocatore rispetto al nemico
      
        float direction = playerHealth.transform.position.x - transform.position.x; 
      // Cambia la scala del nemico per rivolgersi verso il giocatore
      
        if (direction > 0 && transform.localScale.x < 0 || direction < 0 && transform.localScale.x > 0) { 
          transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z); } 
    
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    [SerializeField] public void DamagePlayer()
    {
        if (PlayerInSight)
        {
            //Debug.Log("polpetta");
            playerHealth.TakeDamage(damage);
            audioManager.PlaySFX(audioManager.attaccoPoliziotto);
        }

    }
}
