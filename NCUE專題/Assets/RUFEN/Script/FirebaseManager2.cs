using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;

namespace YourNamespace
{
    public class FirebaseManager2 : MonoBehaviour
    {
        private DatabaseReference databaseReference;

        void Start()
        {
            // 初始化 Firebase Database 參考
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            });
        }

        public class UserData
        {
            public string userName;
            public string email;
            public string profilePicUrl;
        }

        public void WriteUserDataToFirebase(string userId, UserData userData)
        {
            // 在 "users" 節點下，使用用戶的 UID 作為識別
            DatabaseReference userReference = databaseReference.Child("users").Child(userId);

            // 寫入用戶資訊到 Firebase Database
            userReference.Child("email").SetValueAsync(userData.email);
            userReference.Child("photoUrl").SetValueAsync(userData.profilePicUrl);
            userReference.Child("username").SetValueAsync(userData.userName);

        }
    }
}
