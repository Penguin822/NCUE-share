using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    public string sceneName; // 目標場景的名稱

    private void OnCollisionEnter(Collision collision)
    {
        // 檢查碰撞的物體是否是你想要的目標物體
        if (collision.gameObject.CompareTag("bone"))
        {
            // 切換到目標場景
            SceneManager.LoadScene("Dog_Win");
        }
    }
}
