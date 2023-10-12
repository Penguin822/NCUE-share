using UnityEngine;
using UnityEngine.SceneManagement;

public class Win_Button : MonoBehaviour
{
    public string Win; 
    public GameObject buttonToActivate;
    public void Start()
    {
        buttonToActivate.SetActive(false);
    }
    public void SwitchScene()
    {
        SceneManager.LoadScene("Dog_Win");
    }
}
