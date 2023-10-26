using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using Firebase.Auth;
using TMPro;

public class FacebookAuthenticationHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text displayName;
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private TMP_Text usernameText;

    private void Start()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(OnFacebookInitialized);
        }
        else
        {
            FB.ActivateApp();
        }

        if (PlayerPrefs.GetString("current_access_token") == "fb_access_token")
        {
            StartCoroutine(WaitForInitialization());
        }
    }

    private void OnFacebookInitialized()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            Debug.LogError("Failed to initialize the Facebook SDK");
        }
    }

    public void Login()
    {
        var permissions = new List<string> { "public_profile", "email" };
        FB.LogInWithReadPermissions(permissions, OnFacebookLoggedIn);
    }

    private void OnFacebookLoggedIn(ILoginResult result)
    {
        if (result.Cancelled || !string.IsNullOrEmpty(result.Error))
        {
            Debug.LogError("Failed to login to Facebook: " + result.Error);
            return;
        }

        Debug.Log("Successfully logged in to Facebook");
        AuthenticateWithFirebase(result.AccessToken);
    }

    private void AuthenticateWithFirebase(AccessToken accessToken)
    {
        var credential = FacebookAuthProvider.GetCredential(accessToken.TokenString);
        FirebaseAuthenticator.instance.auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Failed to authenticate with Firebase: " + task.Exception.Message);
                return;
            }

            Debug.Log("Successfully authenticated with Firebase");
            PlayerPrefs.SetString("current_access_token", "fb_access_token");
            var user = task.Result;
            ShowUserInfo();
        });
    }

    private IEnumerator WaitForInitialization()
    {
        while ((!FB.IsInitialized) || (FirebaseAuthenticator.instance.auth == null))
        {
            yield return null;
        }

        Debug.Log("FB isInitialized: " + FB.IsInitialized);
        Debug.Log("Firebase isInitialized: " + FirebaseAuthenticator.instance.auth == null);
        Login();
    }

    public void ShowUserInfo()
    {
        Firebase.Auth.FirebaseUser user = FirebaseAuthenticator.instance.auth.CurrentUser;
        if (user != null)
        {
            infoText.text = "User ID: " + user.UserId;
            usernameText.text = "Username: " + user.DisplayName;
        }
        else
        {
            infoText.text = "No user logged in.";
            usernameText.text = "Username: N/A";
        }
    }
}
