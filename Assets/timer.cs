using UnityEngine;
using UnityEngine.UI;
using TMPro; // Per supportare il font stile pixel
using System.Collections; // Per utilizzare le coroutine
using System.Collections.Generic; // Per liste
using Firebase.Firestore; // Per Firestore
using Firebase.Auth; // Per autenticazione Firebase
using System.Linq; // Per supportare il metodo Count

public class LevelTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Per mostrare il timer a schermo
    public Text finalTimeText; // Per mostrare il tempo finale nel pannello delle stelle
    public GameObject starPanel; // Pannello che mostra le stelle a fine livello
    public GameObject[] stars; // Array di stelle che si accendono in base al tempo

    // Riferimenti specifici per ogni elemento della lista
    public List<TextMeshProUGUI> scoreTexts; // Lista dei testi per i punteggi
    public List<Transform> starsContainers; // Lista dei contenitori di stelle

    private FirebaseFirestore db; // Riferimento al database Firestore
    private float elapsedTime = 0f; // Tempo trascorso
    private bool isLevelActive = true; // Indica se il timer deve continuare

    // Soglie per le stelle (in secondi)
    public float threeStarThreshold = 30f; // Fino a 30 secondi
    public float twoStarThreshold = 45f; // Da 31 a 45 secondi
    public float oneStarThreshold = 60f; // Da 46 a 60 secondi

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        ResetTimer();
        starPanel.SetActive(false);

        Debug.Log($"scoreTexts Count: {scoreTexts.Count}, starsContainers Count: {starsContainers.Count}");

        for (int i = 0; i < scoreTexts.Count; i++)
        {
            Debug.Log($"scoreTexts[{i}]: {scoreTexts[i].name}");
        }

        for (int i = 0; i < starsContainers.Count; i++)
        {
            Debug.Log($"starsContainers[{i}]: {starsContainers[i].name}");
        }

        DisplayLevelScores("Ostiense");
    }


    void Update()
    {
        if (isLevelActive)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
        }

        if (Time.timeScale != 1f && !isLevelActive)
        {
            Debug.LogWarning($"Attenzione: Time.timeScale non è 1. Valore corrente: {Time.timeScale}");
        }
    }

    // Aggiorna il timer visivamente
    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Chiamato quando il livello termina
    public void EndLevel()
    {
        isLevelActive = false;
        StartCoroutine(ShowStarsWithDelay(3f)); // Mostra le stelle con un ritardo di 3 secondi
    }

    // Blocca completamente il gioco
    private void FreezeGame()
    {
        Time.timeScale = 0f;
        Debug.Log("Tempo congelato (Time.timeScale = 0).");
    }

    // Resetta il timer e disattiva il pannello stelle
    public void ResetTimer()
    {
        elapsedTime = 0f;
        isLevelActive = true;
        timerText.text = "00:00";
        starPanel.SetActive(false);

        // Disattiva tutte le stelle
        foreach (GameObject star in stars)
        {
            star.SetActive(false);
        }

        Time.timeScale = 1f; // Assicurati di resettare il tempo normale quando il livello ricomincia
        Debug.Log("Tempo ripristinato (Time.timeScale = 1).");
    }


    // Coroutine per mostrare le stelle con un ritardo
    private IEnumerator ShowStarsWithDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // Usa il tempo reale per ignorare Time.timeScale
        FreezeGame();
        CalculateStars();
        ShowFinalTime();
    }

    // Mostra il tempo finale nel pannello stelle
    private void ShowFinalTime()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        finalTimeText.text = string.Format("Tempo finale: {0:00}:{1:00}", minutes, seconds);
    }

    // Calcola quante stelle assegnare
    private void CalculateStars()
    {
        starPanel.SetActive(true); // Mostra il pannello stelle
        int starsEarned = 0;

        if (elapsedTime <= threeStarThreshold)
        {
            starsEarned = 3;
            ActivateStars(3);
        }
        else if (elapsedTime > threeStarThreshold && elapsedTime <= twoStarThreshold)
        {
            starsEarned = 2;
            ActivateStars(2);
        }
        else if (elapsedTime > twoStarThreshold && elapsedTime <= oneStarThreshold)
        {
            starsEarned = 1;
            ActivateStars(1);
        }
        else
        {
            starsEarned = 0;
            ActivateStars(0); // Nessuna stella
        }

        // Salva i dati del livello su Firestore
        SaveLevelData("Ostiense", starsEarned, elapsedTime);
    }

    // Salva i dati del livello su Firestore
    private void SaveLevelData(string levelName, int starsEarned, float timeTaken)
    {
        if (db == null)
        {
            Debug.LogError("Firestore non è inizializzato!");
            return;
        }

        string userId = FirebaseAuth.DefaultInstance.CurrentUser?.UserId;

        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("Utente non autenticato!");
            return;
        }

        // Genera un ID unico per ogni punteggio
        string uniqueScoreId = System.Guid.NewGuid().ToString();

        // Struttura dei dati da salvare
        var levelData = new
        {
            level = levelName,
            stars = starsEarned,
            time = Mathf.FloorToInt(timeTaken), // Arrotonda il tempo ai secondi
            timestamp = Timestamp.GetCurrentTimestamp()
        };

        // Percorso corretto: "users/{userId}/levels/{levelName}/scores/{uniqueScoreId}"
        DocumentReference docRef = db.Collection("users")
                                .Document(userId)
                                .Collection("levels")
                                .Document(levelName)
                                .Collection("scores")
                                .Document(uniqueScoreId);

        docRef.SetAsync(levelData).ContinueWith(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                Debug.Log($"Dati salvati correttamente per il livello: {levelName} con ID: {uniqueScoreId}");
            }
            else
            {
                Debug.LogError("Errore durante il salvataggio dei dati: " + task.Exception?.Message);
            }
        });
    }

    // Attiva il numero corretto di stelle
    private void ActivateStars(int starCount)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].SetActive(i < starCount);
        }
    }

    

    // Metodo per rilevare il trigger del livello
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("player")) // Assicurati che il trigger finale abbia il tag "player"
        {
            EndLevel();
        }
    }

    public void DisplayLevelScores(string levelName)
    {
        string userId = FirebaseAuth.DefaultInstance.CurrentUser?.UserId;

        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("Utente non autenticato!");
            return;
        }

        if (scoreTexts == null || starsContainers == null || scoreTexts.Count != starsContainers.Count)
        {
            Debug.LogError("I riferimenti a scoreTexts e starsContainers non sono configurati correttamente!");
            return;
        }

        // Disattiva tutti i riferimenti inizialmente
        for (int i = 0; i < scoreTexts.Count; i++)
        {
            scoreTexts[i].text = ""; // Pulisce il testo
            foreach (Transform star in starsContainers[i])
            {
                star.gameObject.SetActive(false); // Disattiva le stelle
            }
        }

        db.Collection("users").Document(userId).Collection("levels").Document(levelName).Collection("scores")
    .OrderBy("time") // Ordina i tempi in ordine crescente
    .Limit(5) // Prende solo i primi 5
    .GetSnapshotAsync().ContinueWith(task =>
    {
        if (task.IsCompleted && !task.IsFaulted)
        {
            QuerySnapshot snapshot = task.Result;

            Debug.Log($"Trovati {snapshot.Documents.Count()} punteggi nel database.");

            int index = 0;
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                if (index >= scoreTexts.Count)
                {
                    Debug.LogWarning("Numero di documenti eccede la capacità delle liste configurate.");
                    break;
                }

                var data = document.ToDictionary();
                string time = Mathf.FloorToInt(float.Parse(data["time"].ToString())).ToString();
                int stars = int.Parse(data["stars"].ToString());

                MainThreadDispatcher.Enqueue(() =>
                {
                    Debug.Log($"Aggiornamento punteggio: Indice {index}, Tempo {time}, Stelle {stars}");
                    scoreTexts[index].text = $"{index + 1}) {levelName} Tempo: {time}s";

                    for (int i = 0; i < starsContainers[index].childCount; i++)
                    {
                        starsContainers[index].GetChild(i).gameObject.SetActive(i < stars);
                    }
                    index++;
                });

               // index++;
            }
        }
        else
        {
            Debug.LogError("Errore durante il recupero dei dati: " + task.Exception?.Message);
        }
    });

    }
}