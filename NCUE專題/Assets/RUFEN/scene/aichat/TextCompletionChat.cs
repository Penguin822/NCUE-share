using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System;

namespace OpenAI
{
    public class TextCompletionChat : MonoBehaviour
    {
          [SerializeField] private TMPro.TMP_InputField inputField;
        [SerializeField] private Button button;
        [SerializeField] private ScrollRect scroll;
        
        [SerializeField] private RectTransform sent;
        [SerializeField] private RectTransform received;

        private float height;
        private OpenAIApi openai = new OpenAIApi();

        private string userInput;
        private string prompt = "我是你的寵物，言語上使用溫暖的語言，可以給予你愛與溫暖。\n你想要跟我聊什麼呢？";

        
        private void Start()
        {  // 读取auth.json文件中的API密钥
            string authFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".openai", "auth.json");

            if (File.Exists(authFilePath))
            {
                string json = File.ReadAllText(authFilePath);
                AuthData authData = JsonUtility.FromJson<AuthData>(json);
                openai = new OpenAIApi(authData.api_key);
            }

            button.onClick.AddListener(SendReply);
        }

        [System.Serializable]
        private class AuthData
        {
            public string api_key;
        }

        private void AppendMessage(string message, bool isUser = true)
        {
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

            var item = Instantiate(isUser ? sent : received, scroll.content);
            item.GetChild(0).GetChild(0).GetComponent<Text>().text = message;
            item.anchoredPosition = new Vector2(0, -height);
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);
            height += item.sizeDelta.y;
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            scroll.verticalNormalizedPosition = 0;
        }

        private async void SendReply()
        {
            userInput = inputField.text;
            prompt += $"{userInput}\n";
            AppendMessage(userInput);
            
            button.enabled = false;
            inputField.text = "";
            inputField.enabled = false;
            
            // Complete the instruction
            var completionResponse = await openai.CreateCompletion(new CreateCompletionRequest()
            {
                Prompt = prompt,
                 Model = "text-davinci-003",
                MaxTokens = 128
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                string answerText = completionResponse.Choices[0].Text;
                answerText = answerText.Replace("A: ", ""); // 去除 "A: "
                AppendMessage(answerText, false);
                prompt += $"{answerText}\nQ: ";
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }

            button.enabled = true;
            inputField.enabled = true;
        }
    }
}
