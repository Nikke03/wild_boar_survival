using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        public string text; // Testo del dialogo
        public Sprite characterIcon; // Icona del personaggio
        public bool isLast; // Indica se è l'ultimo dialogo
    }

    public DialogueLine[] dialogueLines; // Array di dialoghi con icone
    private DialogueManager dialogueManager;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("player")) // Controlla se il giocatore entra nel trigger
        {
            if (dialogueManager != null && dialogueLines.Length > 0)
            {
                dialogueManager.StartDialogueWithIcons(dialogueLines);
            }
        }
    }
}