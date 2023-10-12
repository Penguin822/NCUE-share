using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public Transform bone; // 參考骨頭物件
    public AudioSource dogAudioSource; // 狗叫的聲音來源
    public Animator dogAnimator; // 狗的Animator組件
    public float maxVolumeDistance = 100f; // 最大音量距離
    public float minVolumeDistance = 50f; // 最小音量距離
    private Vector3 lastPosition; // 上一幀的位置

    void Start()
    {
        dogAudioSource.mute = true;
        lastPosition = transform.position;
    }

    private void Update()
    {
        // 計算玩家與骨頭之間的距離
        float distanceToBone = Vector3.Distance(transform.position, bone.position);

        // 根據距離設定狗叫聲音的音量
        if (distanceToBone <= minVolumeDistance)
        {
            // 玩家距離骨頭50公尺以內，狗叫聲音大聲
            dogAudioSource.volume = 1f;
        }
        else if (distanceToBone <= maxVolumeDistance)
        {
            // 玩家距離骨頭100公尺以內，狗叫聲音根據距離設定音量
            float volume = 1f - (distanceToBone - minVolumeDistance) / (maxVolumeDistance - minVolumeDistance);
            dogAudioSource.volume = Mathf.Clamp01(volume);
        }
        else
        {
            // 玩家距離骨頭超過100公尺，狗叫聲音靜音
            dogAudioSource.volume = 0f;
        }

        // 計算玩家的移動距離
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);

        // 根據移動距離來控制動畫播放
        if (distanceMoved > 0f)
        {
            // 玩家有移動，播放狗的移動動畫
            dogAnimator.SetBool("IsMoving", true);
        }
        else
        {
            // 玩家沒有移動，停止播放狗的移動動畫
            dogAnimator.SetBool("IsMoving", false);
        }

        // 更新上一幀的位置
        lastPosition = transform.position;
    }
}
