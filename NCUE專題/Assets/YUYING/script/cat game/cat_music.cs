using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cat_music : MonoBehaviour
{
    public GameObject TitleBGM;
    GameObject BGM = null;
    private void Start()
    {
        BGM = GameObject.FindGameObjectWithTag("Sound");
        if(BGM == null)
        {
            BGM = Instantiate(TitleBGM);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
