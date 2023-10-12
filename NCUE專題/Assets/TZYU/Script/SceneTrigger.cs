using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    private Animator run;
    public Vector3 rotationSpeed = new Vector3(0, 45, 0); // 旋轉的速度

    private void Update()
    {
        transform.rotation *= Quaternion.Euler(rotationSpeed * Time.deltaTime);
    }
}
