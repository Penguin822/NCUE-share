using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tips : MonoBehaviour
{
    public GameObject Tip;
    private float time = 2.0f;

    void Start()
    {
        Tip.SetActive(true);
    }

    void Update()
    {
        if (Time.time > time)
        {
            Tip.SetActive(false);
        }
    }
}
