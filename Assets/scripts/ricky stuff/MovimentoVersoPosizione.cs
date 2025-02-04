using UnityEngine;
using System.Collections;

public class MovimentoVersoPosizione : MonoBehaviour
{
    [SerializeField] public Transform target;
    public float velocita = 5f;
    public float distance;
    AudioManager audioManager;

    private bool isSoundPlaying = false;
    private Coroutine soundCoroutine;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {
        if (target != null)
        {
            Vector2 direzione = target.position - transform.position;
            direzione.Normalize();
            distance = Vector2.Distance(transform.position, target.position);

            if (distance < 9)
            {
                Vector2 nuovaPosizione = Vector2.MoveTowards(transform.position, target.position, velocita * Time.deltaTime);
                transform.position = nuovaPosizione;

                // Avvia il suono solo se non è già in riproduzione
                if (!isSoundPlaying)
                {
                    soundCoroutine = StartCoroutine(PlayAttackSound());
                }
            }
            else
            {
                // Interrompi la coroutine se si esce dal raggio
                if (soundCoroutine != null)
                {
                    StopCoroutine(soundCoroutine);
                    isSoundPlaying = false;
                }
            }
        }
    }

    IEnumerator PlayAttackSound()
    {
        isSoundPlaying = true;

        // Riproduce l'effetto sonoro
        audioManager.PlaySFX(audioManager.attaccoApe);

        // Aspetta la durata del suono prima di permettere una nuova riproduzione
        yield return new WaitForSeconds(audioManager.attaccoApe.length);

        isSoundPlaying = false;
    }
}