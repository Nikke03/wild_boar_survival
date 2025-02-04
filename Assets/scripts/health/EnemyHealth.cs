using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    [SerializeField] private GameObject dematerializzatore;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;
    private Rigidbody2D rb;
    AudioManager audioManager;
    public EnemyHealthBar enemyHealthBar;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        enemyHealthBar.SetMaxHealth(startingHealth);
    }

    public void TakeDamage(float _damage)
    {
        //Debug.Log("polpetta");
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        enemyHealthBar.SetHealth(currentHealth);

        if (currentHealth > 0)
        {
            audioManager.PlaySFX(audioManager.attaccoPoliziotto);
            anim.SetTrigger("hurt");
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("die");
                GetComponent<MeleeEnemy>().enabled = false;
                dead = true;
                Die();
                audioManager.PlaySFX(audioManager.mortePoliziotto);
            }
        }
    }
    public void addHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    private void Die()
    {
        Destroy(dematerializzatore);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}