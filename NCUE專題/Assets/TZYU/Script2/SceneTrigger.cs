using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTrigger : MonoBehaviour
{
    public string sceneName;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Ä²µo¸I¼²¨Æ¥ó");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Dog_Win");
        }
    }
}
