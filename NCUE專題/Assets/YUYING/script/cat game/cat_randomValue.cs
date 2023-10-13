using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cat_randomValue : MonoBehaviour
{
    static public float[] random = new float[4];
    int i = 0;

    // Update is called once per frame
    void Update()
    {
        while (i <= 3)
        {

            random[i] = Random.Range(0f, 1f);
            //Debug.Log("random[" + i + "]" + random[i]);
            i++;

        }
    }
}
