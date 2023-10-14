using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class cat_canvasChooseBox : MonoBehaviour
{
    public GameObject box;
    public GameObject box1;
    public GameObject box2;

    public GameObject cat;

    public GameObject panelIntro;
    public Toggle toggleDoNotShowAgain;

    public static bool boxMovingEnd;

    private void Start()
    {
        toggleDoNotShowAgain = GameObject.Find("ToggleDoNotShowAgain").GetComponent<Toggle>();
        boxMovingEnd = false;

        if (PlayerPrefs.HasKey("DoNotShowAgain"))
        {
            panelIntro.SetActive(false);
            toggleDoNotShowAgain.isOn = true;

            box.GetComponent<cat_box>().enabled = true;
            box1.GetComponent<cat_box>().enabled = true;
            box2.GetComponent<cat_box>().enabled = true;

            cat.GetComponent<Animator>().enabled = true;
        }
    }

    public void ClickConfirmForCatIntro()
    {
        Time.timeScale = 1;       
        panelIntro.SetActive(false);

        if (boxMovingEnd == false)
        {
            //Debug.Log('a');
            box.GetComponent<cat_box>().enabled = true;
            box1.GetComponent<cat_box>().enabled = true;
            box2.GetComponent<cat_box>().enabled = true;           
        }        

        cat.GetComponent<Animator>().enabled = true;
    }
}
