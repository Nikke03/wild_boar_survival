// Script principale per gestire i dialoghi
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Per supportare il font stile pixel
using UnityEngine.SceneManagement;


public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel; // Il pannello del dialogo (sfondo nero)
    public TextMeshProUGUI dialogueText; // Il testo del dialogo (box sopra il pannello)
    public Image characterIcon; // Immagine per mostrare l'icona del personaggio
    public Image progressBar; // Barra di avanzamento del tempo
    public float dialogueDuration = 3f; // Durata di ogni dialogo (in secondi)

    private Queue<DialogueTrigger.DialogueLine> dialogues; // Coda per contenere i dialoghi con icone
    private bool isDialogueActive = false;

    void Start()
    {
        dialogues = new Queue<DialogueTrigger.DialogueLine>();
        dialoguePanel.SetActive(false); // Nasconde il pannello all'inizio
        if (progressBar != null)
        {
            progressBar.fillAmount = 0; // Imposta la barra come vuota all'inizio
        }
    }

    public void StartDialogueWithIcons(DialogueTrigger.DialogueLine[] lines)
    {
        if (isDialogueActive) return;

        dialogues.Clear(); // Ripulisce i dialoghi precedenti
        foreach (var line in lines)
        {
            dialogues.Enqueue(line);
        }

        dialoguePanel.SetActive(true); // Mostra il pannello
        isDialogueActive = true;
        StartCoroutine(DisplayNextDialogueWithIcons());
    }

    private IEnumerator DisplayNextDialogueWithIcons()
    {
        while (dialogues.Count > 0)
        {
            var line = dialogues.Dequeue(); // Recupera la prossima linea
            dialogueText.text = line.text; // Mostra il testo
            characterIcon.sprite = line.characterIcon; // Aggiorna l'icona
            characterIcon.gameObject.SetActive(line.characterIcon != null); // Mostra o nasconde l'icona

            if (progressBar != null)
            {
                StartCoroutine(UpdateProgressBar());
            }

            yield return new WaitForSeconds(dialogueDuration); // Aspetta la durata del dialogo

            // Se questo è l'ultimo dialogo, cambia scena
            if (line.isLast)
            {
                yield return new WaitForSeconds(2f); // Ritardo di 1.5 secondi
                SceneManager.LoadScene("Opening");
                yield break; // Interrompe la coroutine
            }
        }

        EndDialogue();
    }

    private IEnumerator UpdateProgressBar()
    {
        float elapsed = 0f;

        while (elapsed < dialogueDuration)
        {
            elapsed += Time.deltaTime;
            progressBar.fillAmount = elapsed / dialogueDuration; // Aggiorna il riempimento della barra
            yield return null;
        }

        progressBar.fillAmount = 0; // Resetta la barra al termine del dialogo
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false); // Nasconde il pannello
        isDialogueActive = false;
    }
}


// Script per configurare bounding box e associare dialoghi specifici
public class DialogueSetup : MonoBehaviour
{
    [System.Serializable]
    public class DialogueBox
    {
        public GameObject boundingBox; // Oggetto del bounding box
        public DialogueTrigger.DialogueLine[] dialogueLines; // Dialoghi associati con icone
    }

    public List<DialogueBox> dialogueBoxes; // Lista di bounding box con dialoghi

    private void Start()
    {
        foreach (var dialogueBox in dialogueBoxes)
        {
            if (dialogueBox.boundingBox != null)
            {
                DialogueTrigger trigger = dialogueBox.boundingBox.GetComponent<DialogueTrigger>();

                if (trigger == null)
                {
                    trigger = dialogueBox.boundingBox.AddComponent<DialogueTrigger>();
                }

                trigger.dialogueLines = dialogueBox.dialogueLines;
            }
        }
    }
}
