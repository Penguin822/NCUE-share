using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase.Auth;

namespace YourNamespace
{
    public class mainfb : MonoBehaviour
    {
        public TMP_Text FB_userName;
        public Image FB_useerDp;
        public TMP_Text buttonLabel;
        private bool isFirstButtonClick = true;
        private FirebaseManager2 firebaseManager;


        private void Awake()
        {
            firebaseManager = new FirebaseManager2();
            if (!FB.IsInitialized)
            {
                FB.Init(SetInit, onHideUnity);
            }
            else
            {
                SetInit();
            }
        }

        private void SetInit()
        {
            if (FB.IsInitialized && FB.IsLoggedIn)
            {
                Debug.Log("Facebook is Logged in!");
                DealWithFbMenus(true);
            }
            else if (FB.IsInitialized && !FB.IsLoggedIn)
            {
                Debug.Log("Facebook is Initialized but not Logged in");
            }
            else
            {
                Debug.Log("Facebook initialization failed. Retrying...");
                FB.Init(SetInit, onHideUnity);
            }
        }

        private void onHideUnity(bool isGameShown)
        {
            if (!isGameShown)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

        public void OnButtonClick()
        {
            if (isFirstButtonClick)
            {
                List<string> permissions = new List<string> { "public_profile", "email" };
                FB.LogInWithReadPermissions(permissions, AuthCallback);
            }
            else
            {
                // 第二次按下按鈕，切換場景
                SceneManager.LoadScene("1_Home"); // 將 "YourSceneName" 替換為您想要切換的場景名稱
            }
        }

        private void AuthCallback(ILoginResult result)
        {
            if (FB.IsLoggedIn)
            {
                AccessToken accessToken = AccessToken.CurrentAccessToken;
                string token = accessToken.TokenString;

                // 使用 Firebase Authentication 的 Facebook 登录方法
                FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(FacebookAuthProvider.GetCredential(token))
                    .ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                        {
                            Debug.LogError("Firebase Facebook Login failed: " + task.Exception);
                            buttonLabel.text = "登入";
                        }
                        else if (task.IsCompleted)
                        {
                            Debug.Log("Firebase Facebook Login successful!");

                    // 確認從 Facebook 獲取的使用者資料
                    FB.API("/me?fields=first_name,email", HttpMethod.GET, (response) =>
                            {
                                if (response.Error == null)
                                {
                                    string userName = response.ResultDictionary["first_name"].ToString();
                                    string email = response.ResultDictionary["email"].ToString();

                            // 確保 userData 物件被正確填充
                            FirebaseManager2.UserData userData = new FirebaseManager2.UserData
                                    {
                                        userName = userName,
                                        email = email,
                                        profilePicUrl = "http://example/profile.jpg" // 替換為從 Facebook 獲取的 profile picture URL
                            };

                            // 調用 WriteUserDataToFirebase 方法將用戶數據寫入 Firebase
                            firebaseManager.WriteUserDataToFirebase(accessToken.UserId, userData);
                                }
                                else
                                {
                                    Debug.LogError("Error retrieving user data from Facebook: " + response.Error);
                                }
                            });

                            DealWithFbMenus(true);
                        }
                    });
            }
            else
            {
                Debug.LogWarning("Facebook login failed or cancelled.");
                buttonLabel.text = "登入";
            }
        }


        void DealWithFbMenus(bool isLoggedIn)
        {
            if (isLoggedIn)
            {
                FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUsername);
                FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);
            }
            else
            {
                // Handle not logged in scenario
            }
        }

        void DisplayUsername(IResult result)
        {
            if (result.Error == null)
            {
                string name = "" + result.ResultDictionary["first_name"];
                Debug.Log("User Name: " + name);
                FB_userName.text = name;
            }
            else
            {
                Debug.LogError("Error retrieving user name: " + result.Error);
            }
        }

        void DisplayProfilePic(IGraphResult result)
        {
            if (result.Texture != null)
            {
                Debug.Log("Profile Pic");

                Sprite profilePicSprite = Sprite.Create(result.Texture, new Rect(0, 0, result.Texture.width, result.Texture.height), new Vector2(0.5f, 0.5f));
                FB_useerDp.sprite = profilePicSprite;
            }
            else
            {
                Debug.LogError("Error retrieving profile picture: " + result.Error);
            }
        }
    }
}
