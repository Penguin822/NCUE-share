using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
   public GameObject panel;
    void Start()
    {
          panel.SetActive(true);
          Invoke("HidePanel", 4f);
    }
    void HidePanel()
    {
        panel.SetActive(false);
      
    }
}
