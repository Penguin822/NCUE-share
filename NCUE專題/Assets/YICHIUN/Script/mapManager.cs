using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mapManager : MonoBehaviour
{
    public GameObject confirmButton;

    // Start is called before the first frame update
    void Start()
    {

        string previousScene = PlayerPrefs.GetString("PreviousScene");

        if(previousScene == "B_home")
        {
            confirmButton.gameObject.SetActive(true);
        }
        else if(previousScene == "B_game")
        {
            confirmButton.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnreturnButtonClick()
    {
        SceneManager.LoadScene("B_game");
    }

    public void OnconfirmButtonClick()
    {
        SceneManager.LoadScene("B_game");
    }

}
