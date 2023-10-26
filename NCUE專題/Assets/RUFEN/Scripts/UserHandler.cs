using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserHandler : MonoBehaviour
{
    [SerializeField] private Text infoText;
    [SerializeField] private InputField newUserInputField;
    [SerializeField] private Text usernameText;  // 新增一個 Text UI 元素

    public void ShowUserInfo()
    {
        Firebase.Auth.FirebaseUser user = FirebaseAuthenticator.instance.auth.CurrentUser;
        if (user != null)
        {
            infoText.text = "User ID: " + user.UserId;
            usernameText.text = "Username: " + user.DisplayName;  // 將使用者的 DisplayName 顯示在 UI 文本中
        }
        else
        {
            infoText.text = "No user logged in.";
            usernameText.text = "Username: N/A";
        }
    }

    public void ChangeUsername()
    {
        Firebase.Auth.FirebaseUser user = FirebaseAuthenticator.instance.auth.CurrentUser;
        if (user != null)
        {
            Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
            {
                DisplayName = newUserInputField.text
            };
            user.UpdateUserProfileAsync(profile).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("User profile updated successfully.");
                ShowUserInfo();  // 更新使用者資訊並在 UI 中顯示
            });
        }
    }
}

