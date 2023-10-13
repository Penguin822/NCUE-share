using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Security.AccessControl;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Animations;

public class MainCamera : MonoBehaviour
{
    private string path;
    public Camera ARCam;

    public void OnScreenShootClick()
    {
        if (DogFlagInScreen.inScreen == false && CatFlagInScreen.inScreen == false && BirdFlagInScreen.inScreen == false) //沒照到旗子
        {
            ScreenShoot(ARCam);
        }
        else //進入遊戲，設三種不同顏色的旗子，對應三種寵物遊戲
        {
            if (DogFlagInScreen.inScreen == true)
            {
                SceneManager.LoadScene("Dog_Main");
            }
            if (CatFlagInScreen.inScreen == true)
            {
                SceneManager.LoadScene("cat_game_intro");
            }
            if (BirdFlagInScreen.inScreen == true)
            {
                SceneManager.LoadScene("B_game");
            }
        }
    }

    //獲取圖片保存路徑
    public string ScreenshotPath()
    {
        string filePath = "";
        if(Application.platform == RuntimePlatform.Android)
        {
            filePath = "/sdcard/DCIM/StepbyStep/";

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
        }
        else
        {
            filePath = Application.dataPath + "/StepbyStep/";

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
        }

        return filePath;
    }

    //截圖保存之後刷新相冊
    Texture2D ScreenShoot(Camera camera)
    {
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 0);
        camera.targetTexture = rt;
        camera.Render();

        // 激活rt，並從中讀取像素
        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, true);
        tex.Apply();       

        camera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        byte[] byt = tex.EncodeToPNG();

        //圖片名稱，一般以日期命名
        DateTime currentTime = DateTime.Now;
        string fileName = "Custom" + currentTime.Year + currentTime.Month + currentTime.Day + "_" + currentTime.Hour + currentTime.Minute + currentTime.Second + ".png";
        path = ScreenshotPath() + fileName;
        File.WriteAllBytes(path, byt);
        string[] paths = { path };

        //安卓平台才調用安卓的活動刷新相冊
        if (Application.platform == RuntimePlatform.Android)
        {
            ScanFile(paths);
        }
        return tex;
    }

    //刷新圖片，顯示到相冊中
    void ScanFile(string[] path)
    {
        using (AndroidJavaClass PlayerActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject playerActivity = PlayerActivity.GetStatic<AndroidJavaObject>("currentActivity");
            using (AndroidJavaObject Conn = new AndroidJavaObject("android.media.MediaScannerConnection", PlayerActivity, null))
            {
                Conn.CallStatic("scanFile", playerActivity, path, null, null);
            }
        }
    }
}
