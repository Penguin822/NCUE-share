using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

    public Vector3 rotationSpeed = new Vector3(0, 45, 0); // 旋轉的速度

    private void Update()
    {
        // 在每一幀更新時旋轉物體的旋轉值
        transform.rotation *= Quaternion.Euler(rotationSpeed * Time.deltaTime);
    }
}
