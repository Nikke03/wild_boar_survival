using UnityEngine;
using TMPro; // Aggiunto per supportare TMP_InputField
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine.UI; // Aggiunto per supportare Text

public class FirebaseManager : MonoBehaviour
{
    // Campi per il form di registrazione
    public TMP_InputField registerEmailInput;
    public TMP_InputField registerPasswordInput;
    public TMP_InputField confirmPasswordInput; // Campo per confermare la password

    // Campi per il form di login
    public TMP_InputField loginEmailInput;
    public TMP_InputField loginPasswordInput;

    // Testo per i messaggi di feedback
    public Text feedbackText; // Cambiato da Text a TMP_Text per supportare TextMeshPro
    public Text successMessageText; // Cambiato per supportare Text normale
    public Text accountEmailText; // Aggiunto per mostrare l'email nel pannello account

    public GameObject loginPanel; // Pannello per il login
    public GameObject registerPanel; // Pannello per la registrazione
    public GameObject mainPanel; // Pannello principale
    public GameObject successPanel; // Pannello di successo login
    public GameObject errorPanel; // Pannello di errore login

    private FirebaseAuth auth; // Istanza di FirebaseAuth
    private FirebaseUser currentUser; // Utente attualmente autenticato

    void Start()
    {
        Debug.Log("FirebaseManager.Start() chiamato.");

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Errore inizializzazione Firebase: " + task.Exception);
                return;
            }

            if (task.Result == DependencyStatus.Available)
            {
                Debug.Log("Firebase inizializzato con successo.");
                auth = FirebaseAuth.DefaultInstance;

                // Controlla se l'utente è già loggato
                currentUser = auth.CurrentUser;
                if (currentUser != null)
                {
                    Debug.Log($"Utente già loggato: {currentUser.Email}");
                    feedbackText.text = "Bentornato, " + currentUser.Email;
                    UpdateAccountEmail(currentUser.Email); // Aggiorna l'email nell'account panel
                }
                else
                {
                    feedbackText.text = "Benvenuto! Effettua il login o la registrazione.";
                }
            }
            else
            {
                Debug.LogError($"Errore: Non è stato possibile risolvere le dipendenze Firebase: {task.Result}");
            }
        });
    }

    // Metodo per registrare un nuovo utente
    public void RegisterUser()
    {
        string email = registerEmailInput.text; // Legge l'email dal form di registrazione
        string password = registerPasswordInput.text; // Legge la password dal form di registrazione
        string confirmPassword = confirmPasswordInput.text; // Legge la conferma della password

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            ShowErrorPanelWithMessage("Tutti i campi sono obbligatori.");
            return;
        }

        if (password != confirmPassword)
        {
            ShowErrorPanelWithMessage("Le password non coincidono. Riprova.");
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                Debug.Log($"Registrazione avvenuta con successo: {auth.CurrentUser.Email}");
                ShowLoginPanel(); // Mostra il pannello di login
            }
            else
            {
                string errorMessage = "Errore durante la registrazione.";
                if (task.Exception != null)
                {
                    FirebaseException firebaseEx = task.Exception.GetBaseException() as FirebaseException;
                    if (firebaseEx != null)
                    {
                        switch ((AuthError)firebaseEx.ErrorCode)
                        {
                            case AuthError.EmailAlreadyInUse:
                                errorMessage = "Email già in uso.";
                                break;
                            case AuthError.InvalidEmail:
                                errorMessage = "Email non valida.";
                                break;
                            case AuthError.WeakPassword:
                                errorMessage = "Password troppo debole.";
                                break;
                            default:
                                errorMessage = "Errore sconosciuto.";
                                break;
                        }
                    }
                }
                Debug.LogError($"Errore durante la registrazione: {errorMessage}");
                ShowErrorPanelWithMessage(errorMessage);
            }
        });
    }

    public void LoginUser()
    {
        string email = loginEmailInput.text; // Legge l'email dal form di login
        string password = loginPasswordInput.text; // Legge la password dal form di login

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ShowErrorPanelWithMessage("Email o password non possono essere vuoti.");
            return;
        }

        if (password.Length < 6)
        {
            ShowErrorPanelWithMessage("La password deve contenere almeno 6 caratteri.");
            return;
        }

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                currentUser = auth.CurrentUser;
                Debug.Log($"Login avvenuto con successo: {currentUser.Email}");
                ShowSuccessPanelWithMessage("Utente collegato"); // Mostra il pannello di successo con messaggio
                UpdateAccountEmail(currentUser.Email); // Aggiorna l'email nell'account panel
            }
            else
            {
                string errorMessage = "Errore durante il login.";
                if (task.Exception != null)
                {
                    FirebaseException firebaseEx = task.Exception.GetBaseException() as FirebaseException;
                    if (firebaseEx != null)
                    {
                        AuthError authError = (AuthError)firebaseEx.ErrorCode;
                        Debug.LogError($"Codice errore Firebase: {authError}");
                        switch (authError)
                        {
                            case AuthError.InvalidEmail:
                                errorMessage = "Email non valida.";
                                break;
                            case AuthError.WrongPassword:
                                errorMessage = "Password errata.";
                                break;
                            case AuthError.UserNotFound:
                                errorMessage = "Utente non trovato.";
                                break;
                            case AuthError.NetworkRequestFailed:
                                errorMessage = "Errore di connessione. Controlla la tua rete.";
                                break;
                            case AuthError.TooManyRequests:
                                errorMessage = "Troppi tentativi di login. Riprova più tardi.";
                                break;
                            default:
                                errorMessage = $"Errore sconosciuto: {authError}";
                                break;
                        }
                    }
                    else
                    {
                        Debug.LogError($"Errore generico durante il login: {task.Exception.Message}");
                    }
                }
                ShowErrorPanelWithMessage(errorMessage);
            }
        });
    }



    // Metodo per il pulsante che apre login o main panel
    public void OpenLoginOrMainPanel()
    {
        if (auth.CurrentUser != null)
        {
            Debug.Log($"Utente già loggato: {auth.CurrentUser.Email}");
            UpdateAccountEmail(auth.CurrentUser.Email); // Aggiorna l'email nell'account panel
            ShowMainPanel(); // Mostra il pannello principale se l'utente è loggato
        }
        else
        {
            Debug.Log("Nessun utente loggato, mostro il pannello di login.");
            ShowLoginPanel(); // Mostra il pannello di login se non c'è un utente loggato
        }
    }

    // Metodo per effettuare il logout
    public void LogoutUser()
    {
        if (auth != null && auth.CurrentUser != null)
        {
            auth.SignOut();
            Debug.Log("Logout effettuato.");
            UpdateAccountEmail(""); // Rimuove l'email dall'account panel
            HideMainPanelAndShowLoginPanel();
        }
        else
        {
            Debug.LogWarning("Nessun utente loggato.");
        }
    }

    // Metodi per gestire i pannelli
    public void ShowLoginPanel()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
        mainPanel.SetActive(false);
        successPanel.SetActive(false);
        errorPanel.SetActive(false);
    }

    public void ShowRegisterPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
        mainPanel.SetActive(false);
        successPanel.SetActive(false);
        errorPanel.SetActive(false);
    }

    private void ShowMainPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        mainPanel.SetActive(true);
        successPanel.SetActive(false);
        errorPanel.SetActive(false);
    }

    private void ShowSuccessPanelWithMessage(string message)
    {
        if (loginPanel != null) loginPanel.SetActive(false);
        if (successPanel != null)
        {
            successPanel.SetActive(true);
            if (successMessageText != null)
            {
                successMessageText.text = message;
            }
        }
    }

    private void ShowErrorPanelWithMessage(string errorMessage)
    {
        if (loginPanel != null) loginPanel.SetActive(false);
        if (errorPanel != null)
        {
            errorPanel.SetActive(true);
            if (feedbackText != null)
            {
                feedbackText.text = errorMessage;
            }
        }
    }

    private void HideMainPanelAndShowLoginPanel()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
        ShowSuccessPanelWithMessage("Utente disconnesso"); // Mostra il pannello di successo con messaggio
    }

    private void UpdateAccountEmail(string email)
    {
        if (accountEmailText != null)
        {
            accountEmailText.text = string.IsNullOrEmpty(email) ? "Nessun utente collegato" : email;
        }
    }
}
