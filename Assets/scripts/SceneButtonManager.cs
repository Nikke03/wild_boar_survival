using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButtonManager : MonoBehaviour
{
    // Funzione per caricare una scena specifica
    public void LoadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName); // Carica la scena con il nome specificato
        }
        else
        {
            Debug.LogWarning("Il nome della scena non è valido.");
        }
    }
}
