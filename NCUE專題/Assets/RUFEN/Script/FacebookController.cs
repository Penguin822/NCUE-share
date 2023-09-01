using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

// Include Facebook namespace
using Facebook.Unity;

public class FacebookController : MonoBehaviour
{
    // Awake function from Unity's MonoBehavior
    void Awake()
    {
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }
public void OnLoginBtnClick()
{
    var perms = new List<string>() { "public_profile", "email" };
    FB.LogInWithReadPermissions(perms, AuthCallback);
}

private void AuthCallback (ILoginResult result) {
    if (FB.IsLoggedIn) {
        // AccessToken class will have session details
        var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
        // Print current access token's User ID
        Debug.Log(aToken.UserId);
        // Print current access token's granted permissions
        foreach (string perm in aToken.Permissions) {
            Debug.Log(perm);
        }
    } else {
        Debug.Log("User cancelled login");
    }
}
public Text resultText;

private IEnumerator GetResult(string token)
{
    var postData = new List<IMultipartFormSection>();
    postData.Add(new MultipartFormDataSection("Token", token));

    var www = UnityWebRequest.Post("http://192.168.56.1/login.php", postData);
    yield return www.SendWebRequest();

    if (www.result != UnityWebRequest.Result.Success)
    {
        resultText.text = www.error;
    }
    else
    {
        resultText.text = www.downloadHandler.text;
    }
}
}