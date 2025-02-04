using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;
    private Rigidbody2D rb;
    public int oggettiDaRaccogliere = 3; // Numero di oggetti da raccogliere per diventare immortale
    public float durataImmortalita = 10f; // Durata dell'immortalità in secondi
    public bool immortale = false;
    public bool immortalePartito = false;// Flag per indicare se il giocatore è immortale
    private int oggettiRaccolti = 0; // Contatore degli oggetti raccolti
    private float tempoImmortalitaRimanente = 0f; // Tempo rimanente dell'immortalità
    private Collider2D collision;
    public PlayerController player;
    AudioManager audioManager;
    RadialFillManager radialfill;

    private void Start()
    {
        player = GetComponent<PlayerController>(); // Ottieni il riferimento al componente PlayerController
    }


    void Update()
    {
        // Controllo se il giocatore è immortale
        if (immortalePartito)
        {
            aggiornaAnimazioni();

            // Riduci il tempo rimanente dell'immortalità
            tempoImmortalitaRimanente -= Time.deltaTime;

            // Disattiva l'immortalità quando il tempo è scaduto
            if (tempoImmortalitaRimanente <= 0)
            {
                anim.SetTrigger("FalseInvincible");
                immortale = false;
                player.gigi = false;
                immortalePartito = false;

                if (radialfill != null)
                {
                    radialfill.ResetRadialFill();
                }


            }
        }

    }

    private void aggiornaAnimazioni()
    {

        if (player.vecMove.x == 0)
            anim.SetTrigger("InvincibleIdle");
        else if(player.vecMove.x != 0)
            anim.SetTrigger("FalseInvincibleIdle");


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Se l'oggetto ha il tag "cherry", raccogli l'oggetto
        if (other.CompareTag("cherry"))
        {
            RaccogliOggetto(other.gameObject);

            if (radialfill != null)
            {
                radialfill.IncreaseRadialFill();
            }
            else
            {
                Debug.LogWarning("RadialFillManager non trovato nella scena.");
            }
        }
    }

    // Metodo per raccogliere un oggetto
    private void RaccogliOggetto(GameObject oggettoRaccolto)
    {
        audioManager.PlaySFX(audioManager.special);
        // Incrementa il contatore degli oggetti raccolti
        oggettiRaccolti++;
        // Disattiva l'oggetto raccolto per farlo sparire
        oggettoRaccolto.SetActive(false);
        


        // Se il giocatore ha raccolto il numero necessario di oggetti, attiva l'immortalità
        if (oggettiRaccolti >= oggettiDaRaccogliere)
        {
            immortale = true;
            oggettiRaccolti = 0;
        }
    }

    // Metodo per attivare l'immortalità
    public void AttivaImmortalita()
    {
        immortalePartito = true;
        tempoImmortalitaRimanente = durataImmortalita;
        anim.SetTrigger("Invincible");
        audioManager.PlaySFX(audioManager.playerAttack);


    }

    // Metodo per controllare se il giocatore è immortale
    public bool IsImmortale()
    {
        return immortale;
    }



    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        radialfill = FindObjectOfType<RadialFillManager>();

    }

    public void TakeDamage(float _damage)
    {
        if (!immortalePartito)
        {

            currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

            if (currentHealth > 0)
            {
                anim.SetTrigger("hurt");
                audioManager.PlaySFX(audioManager.dannoSubito);
            }
            else
            {
                if (!dead)
                {
                    anim.SetTrigger("die");
                    GetComponent<PlayerController>().enabled = false;
                    dead = true;
                    Die();
                }
            }
        }
    }
    public void addHealth(float _value)
    {
        audioManager.PlaySFX(audioManager.healing);
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    private void Die()
    {
        audioManager.PlaySFX(audioManager.death);
        rb.bodyType = RigidbodyType2D.Static;
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}