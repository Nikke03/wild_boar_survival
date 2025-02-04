using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingObj : MonoBehaviour
{

private Vector3 posizionePrecedente;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Ottieni il componente SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            // Se il componente SpriteRenderer non è presente, lo aggiungiamo
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }

        // Inizializza la posizione precedente all'avvio
        posizionePrecedente = transform.position;
    }

    void Update()
    {
        // Calcola la direzione di movimento
        Vector3 direzioneMovimento = transform.position - posizionePrecedente;

        // Flippa l'orientamento in base alla direzione di movimento
        if (direzioneMovimento.x < 0)
        {
            spriteRenderer.flipX = false; // Orientamento invertito se si muove a sinistra
        }
        else if (direzioneMovimento.x > 0)
        {
            spriteRenderer.flipX = true; // Orientamento normale se si muove a destra
        }
        // Se la direzione x è zero, mantieni l'orientamento attuale

        // Aggiorna la posizione precedente
        posizionePrecedente = transform.position;
    }
}