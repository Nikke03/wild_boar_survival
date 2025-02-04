using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class StoryboardManager : MonoBehaviour
{
    [System.Serializable]
    public class Slide
    {
        public Sprite image; // Immagine della vignetta
        public string characterName; // Nome del personaggio che parla
        public string dialogue; // Testo del dialogo
        public AudioClip voiceClip; // Audio del dialogo
    }

    public Image displayImage; // UI Image per mostrare le vignette
    public TextMeshProUGUI dialogueText; // Testo del dialogo
    public Slide[] slides; // Array di vignette con immagini, dialoghi e audio
    public string nextSceneName = "GameScene"; // Nome della scena successiva
    public Button skipButton; // Pulsante per saltare tutto
    public AudioSource audioSource; // Componente AudioSource per la riproduzione

    private int currentIndex = 0;
    private bool isSkipping = false; // Controllo per evitare doppie chiamate

    void Start()
    {
        if (skipButton != null)
        {
            skipButton.onClick.AddListener(SkipToNextScene); // Aggiunge il pulsante Skip
        }

        ShowSlide(); // Mostra la prima slide
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isSkipping)
        {
            NextSlide();
        }
    }

    void ShowSlide()
    {
        if (currentIndex < slides.Length)
        {
            displayImage.sprite = slides[currentIndex].image;
            dialogueText.text = $"<b>{slides[currentIndex].characterName}:</b> {slides[currentIndex].dialogue}"; // Rimosse le virgolette

            // Riproduce l'audio della vignetta
            if (slides[currentIndex].voiceClip != null && audioSource != null)
            {
                audioSource.Stop(); // Ferma eventuale audio in corso
                audioSource.clip = slides[currentIndex].voiceClip;
                audioSource.Play();
            }
        }
        else
        {
            LoadNextScene();
        }
    }

    void NextSlide()
    {
        currentIndex++;
        ShowSlide();
    }

    public void SkipToNextScene()
    {
        if (!isSkipping)
        {
            isSkipping = true;
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
