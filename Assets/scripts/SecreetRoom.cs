using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SecretRoom : MonoBehaviour
{

    public string nomeLayerDaNascondere = "secretRoom"; // Inserisci il nome del layer che vuoi nascondere

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Assicurati che l'oggetto che entra nella zona sia il giocatore
        {
            // Trova tutti gli oggetti con il layer specificato
            GameObject[] oggettiConLayer = GameObject.FindGameObjectsWithTag(nomeLayerDaNascondere);

            foreach (GameObject oggetto in oggettiConLayer)
            {
                oggetto.SetActive(false); // Nascondi l'oggetto
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Assicurati che l'oggetto che esce dalla zona sia il giocatore
        {
            // Trova tutti gli oggetti con il layer specificato
            GameObject[] oggettiConLayer = GameObject.FindGameObjectsWithTag(nomeLayerDaNascondere);

            foreach (GameObject oggetto in oggettiConLayer)
            {
                oggetto.SetActive(true); // Mostra l'oggetto
            }
        }
    }
}










