using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class Trasparenza: MonoBehaviour
{
    // Variabili per gestire i materiali
    public Material materialeTrasparente; // Il materiale trasparente che sar� applicato alla roccia
    private Material materialeOriginale; // Il materiale originale della roccia
    private Renderer rend; // Il componente Renderer della roccia

    void Start()
    {
        // Alla partenza, otteniamo il componente Renderer della roccia e il suo materiale originale
        rend = GetComponent<Renderer>();
        materialeOriginale = rend.material;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Questo metodo viene chiamato quando un oggetto entra nel trigger della roccia
        if (other.CompareTag("player")) // Verifica se l'oggetto che � entrato � il giocatore
        {
            rend.material = materialeTrasparente; // Se s�, applica il materiale trasparente alla roccia
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Questo metodo viene chiamato quando un oggetto esce dal trigger della roccia
        if (other.CompareTag("player")) // Verifica se l'oggetto che � uscito � il giocatore
        {
            rend.material = materialeOriginale; // Se s�, ripristina il materiale originale della roccia
        }
    }
}