using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class provaScriptSecretRoom : MonoBehaviour
{
    public string tagDaRiattivare = "secretRoom"; // Inserisci il nome del tag che vuoi riattivare

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Assicurati che l'oggetto che entra nella zona sia il giocatore
        {
            // Trova tutti gli oggetti con il tag specificato
            GameObject[] oggettiConTag = GameObject.FindGameObjectsWithTag(tagDaRiattivare);

            foreach (GameObject oggetto in oggettiConTag)
            {
                oggetto.layer = LayerMask.NameToLayer("Default"); // Riporta il layer al suo stato originale
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Assicurati che l'oggetto che esce dalla zona sia il giocatore
        {
            // Trova tutti gli oggetti con il tag specificato
            GameObject[] oggettiConTag = GameObject.FindGameObjectsWithTag(tagDaRiattivare);

            foreach (GameObject oggetto in oggettiConTag)
            {
                oggetto.layer = LayerMask.NameToLayer("Default"); // Riporta il layer al suo stato originale
            }
        }
    }
}


