using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashingButton : MonoBehaviour
{
    public Button button;
    public float minAlpha = 0.2f;
    public float maxAlpha = 1.0f;
    public float fadeSpeed = 1.5f;

    private bool increasing = true;

    private void Start()
    {
        // Imposta l'opacità iniziale del bottone al valore massimo
        Color buttonColor = button.image.color;
        buttonColor.a = maxAlpha;
        button.image.color = buttonColor;

        // Avvia la coroutine per il lampeggiamento
        StartCoroutine(FlashButton());
    }

    IEnumerator FlashButton()
    {
        while (true)
        {
            // Cambia gradualmente l'opacità del bottone da minAlpha a maxAlpha o viceversa
            float targetAlpha = increasing ? maxAlpha : minAlpha;
            float currentAlpha = button.image.color.a;
            float newAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, fadeSpeed * Time.deltaTime);

            Color buttonColor = button.image.color;
            buttonColor.a = newAlpha;
            button.image.color = buttonColor;

            // Se l'opacità raggiunge il valore massimo o minimo, inverti la direzione
            if (newAlpha == minAlpha || newAlpha == maxAlpha)
            {
                increasing = !increasing;
            }

            yield return null;
        }
    }
}