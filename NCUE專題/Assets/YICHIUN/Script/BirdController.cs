using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    private Camera mainCamera;
    private float birdSpeed = 5.0f;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // 获取相机的位置
        Vector3 cameraPosition = mainCamera.transform.position;

        // 设置鸟的目标位置，这里以相机位置为基础向前移动
        Vector3 birdTargetPosition = cameraPosition + mainCamera.transform.forward * 0.7f;
        birdTargetPosition.y += 0.1f;
        birdTargetPosition.x += 0.02f;

        // 移动鸟向目标位置
        transform.position = Vector3.MoveTowards(transform.position, birdTargetPosition, birdSpeed * Time.deltaTime);
    }
}
