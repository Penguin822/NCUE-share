using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mapbox.Utils;

public class FeatherObject : MonoBehaviour
{
    public Text feathercounter;
    private int counter = 0;
    public GameObject getfeatherPanel;

    void Start()
    {
        getfeatherPanel.SetActive(false);
    }

    private void UpdateCounter()
    {
        counter++;
        feathercounter.text = counter.ToString();

        if (counter == 10)
        {
            SceneManager.LoadScene("B_win");
        }
    }

    private void Update()
    {
        
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("GameFeather"))
                    {
                        // 销毁被点击的Feather物体
                        Destroy(hit.collider.gameObject);
                        // 更新计数器
                        UpdateCounter();

                        OnDestory();
                    }
                }
            }
        }
    }

    private void OnDestory()
    {
        getfeatherPanel.SetActive(true);
        StartCoroutine(ShowPanelForSeconds(1.5f));
    }


    private IEnumerator ShowPanelForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        getfeatherPanel.SetActive(false); 
    }

}
