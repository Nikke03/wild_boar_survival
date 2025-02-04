using System.Collections; // Necessario per IEnumerator
using UnityEngine;
using UnityEngine.UI;

public class RadialFillManager : MonoBehaviour
{
    public Image radialImage; // L'immagine della barra radiale
    private int collectedItems = 0; // Oggetti raccolti
    private const int totalItems = 3; // Totale degli oggetti richiesti
    public float fillAnimationDuration = 0.5f; // Durata dell'animazione del riempimento

    // Funzione per aumentare il riempimento con animazione
    public void IncreaseRadialFill()
    {
        if (radialImage != null && collectedItems < totalItems)
        {
            collectedItems++; // Incrementa il conteggio degli oggetti raccolti
            float targetFillAmount = (float)collectedItems / totalItems; // Calcola il riempimento target
            StopAllCoroutines(); // Ferma eventuali animazioni precedenti
            StartCoroutine(AnimateFill(targetFillAmount)); // Avvia l'animazione
        }
    }

    // Funzione per resettare la barra radiale
    public void ResetRadialFill()
    {
        if (radialImage != null)
        {
            collectedItems = 0; // Resetta il conteggio degli oggetti raccolti
            StopAllCoroutines(); // Ferma eventuali animazioni precedenti
            StartCoroutine(AnimateFill(0f)); // Avvia l'animazione per riportare la barra a 0
        }
    }

    // Coroutine per animare il riempimento
    private IEnumerator AnimateFill(float targetFillAmount)
    {
        float startFillAmount = radialImage.fillAmount; // Valore iniziale del riempimento
        float elapsedTime = 0f;

        while (elapsedTime < fillAnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            radialImage.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, elapsedTime / fillAnimationDuration); // Interpolazione lineare
            yield return null;
        }

        radialImage.fillAmount = targetFillAmount; // Assicura che il riempimento raggiunga esattamente il valore target
    }
}
