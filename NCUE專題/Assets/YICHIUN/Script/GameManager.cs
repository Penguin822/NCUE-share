using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject leavePanel;
    public GameObject canvasBG;

    private static GameManager instance;


    void Start()
    {
        leavePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;   
    }

    public void OnleaveButtontClick()
    {     
        PauseGame();  
        leavePanel.SetActive(true);
    }

    public void OnconfirmButtonClick()
    {

    }

    public void OncancelButtonClick()
    {
        ResumeGame();
        leavePanel.SetActive(false);
    }

    public void OnMapButtontClick()
    {      
        PauseGame();
        SceneManager.LoadScene("B_game Map");
    }
}


