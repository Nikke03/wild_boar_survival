using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Aggiungi questo using

public class FireBaseController : MonoBehaviour

{

    public GameObject loginPanel, singupPanel, profilePanel, forgetPasswordPanel, notificationPanel;

    public TMP_InputField loginEmail, loginPassword, singupEmail, singnupPassword, singupCPassword, forgetPassEmail;

    public TMP_Text notif_Title_Text, notif_Message_Text, profileUserEmail_Text;

    public Toggle rememberMe;

    public void OpenLoginPanel()
    {
        loginPanel.SetActive(true);
        singupPanel.SetActive(false);
        profilePanel.SetActive(false);
        forgetPasswordPanel.SetActive(false);
    }

    public void OpenSingUpPanel()
    {
        loginPanel.SetActive(false);
        singupPanel.SetActive(true);
        profilePanel.SetActive(false);
        forgetPasswordPanel.SetActive(false);
    }

    public void OpenProfilePanel()
    {
        loginPanel.SetActive(false);
        singupPanel.SetActive(false);
        profilePanel.SetActive(true);
        forgetPasswordPanel.SetActive(false);
    }

    public void OpenForgetPassPanel()
    {
        loginPanel.SetActive(false);
        singupPanel.SetActive(false);
        profilePanel.SetActive(false);
        forgetPasswordPanel.SetActive(true);
    }

    public void LoginUser()
    {
        if(string.IsNullOrEmpty(loginEmail.text) && string.IsNullOrEmpty(loginPassword.text)) {
            showNotificationMessage("Error", "Some Fields Empty");
            return;
        }
    }

    public void SignUpUser()
    {
        if (string.IsNullOrEmpty(singupEmail.text) && string.IsNullOrEmpty(singnupPassword.text) && string.IsNullOrEmpty(singupCPassword.text))
        {
            return;
        }
    }

    public void LoginPassword()
    {
        if (string.IsNullOrEmpty(singupEmail.text) && string.IsNullOrEmpty(singnupPassword.text)) {
            showNotificationMessage("Error", "Some Fields Empty");
            return;
        }
    }

    public void forgetPassword()
    {
        if(string.IsNullOrEmpty(forgetPassEmail.text))
        {
            showNotificationMessage("Error", "Some Fields Empty");
            return;
        }
    }

    private void showNotificationMessage(string title, string message)
    {
        notif_Title_Text.text = "" + title;
        notif_Message_Text.text = "" + message;

        notificationPanel.SetActive(true);
    }

    public void CloseNotIf_Panel()
    {
        notif_Title_Text.text = "";
        notif_Message_Text.text = "";

        notificationPanel.SetActive(false);
    }

    public void logOut()
    {
        profileUserEmail_Text.text = "";
        OpenLoginPanel();
    }
}
