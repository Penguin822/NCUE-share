using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Info : MonoBehaviour
{
    public GameObject panelIntro;
    public Toggle toggleDoNotShowAgain;

    private void Start()
    {
        if (PlayerPrefs.HasKey("DoNotShowAgain"))
        {
            panelIntro.SetActive(false);
            toggleDoNotShowAgain.isOn = true;
        }
    }

    public void ClickConfirm()
    {
        //Time.timeScale = 1;       
        panelIntro.SetActive(false);       
    }

    public void ClickBackHome()
    {
        SceneManager.LoadScene("1_Home");
    }

    public void ClickIntro()
    {
        panelIntro.SetActive(true);
        //Time.timeScale = 0;
    }

    public void ClickDoNotShowAgain()
    {
        //不再顯示
        if(toggleDoNotShowAgain.isOn == true)
        {
            PlayerPrefs.SetInt("DoNotShowAgain", 1);
        }

        //維持每次都顯示
        if (toggleDoNotShowAgain.isOn == false)
        {
            PlayerPrefs.DeleteKey("DoNotShowAgain");
        }
    }
}