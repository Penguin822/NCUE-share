using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DogMove : MonoBehaviour
{
    public GameObject dog;
    public GameObject bone;
    Animator dogAnimator;
    public GameObject ARCamera;

    public AudioClip soundClip;
    private AudioSource audioSource;
    private bool hasPlayedSound = false;

    public TMP_Text distanceText;

    void Start()
    {
        dogAnimator = dog.GetComponent<Animator>();
        dogAnimator.SetBool("IsMoving", false);

        audioSource = GetComponent<AudioSource>();
        audioSource.enabled = false;
        audioSource.clip = soundClip;
    }

    void Update()
    {
        var ARCamDistance = Vector3.Distance(dog.transform.position, ARCamera.transform.position);
        var boneDistance = Vector3.Distance(dog.transform.position, bone.transform.position);

        if (boneDistance < 20.0f && !hasPlayedSound) // 检查距离和是否已经播放过声音
        {
            distanceText.text = "快到了!";
            audioSource.enabled = true;
            audioSource.volume = 0.5f; 
            audioSource.Play();
            hasPlayedSound = true; // 设置标记，以确保声音只会播放一次

        }
        if (Mathf.Abs(ARCamDistance) > 1.5f)
        {
            Debug.Log("notmove");
            dogAnimator.SetBool("IsMoving", false);
        }
        else
        {
            Debug.Log("move");
            dogAnimator.SetBool("IsMoving", true);

            var direction = bone.transform.position - dog.transform.position;
            direction.y = 0;

            dog.transform.Translate(direction.normalized * Time.deltaTime * 5.5f, Space.World);
            var angle = Vector3.Angle(dog.transform.forward, direction);
            var cross = Vector3.Cross(dog.transform.forward, direction);
            var turn = cross.y >= 0 ? 1f : -1f;
            dog.transform.Rotate(dog.transform.up, angle * Time.deltaTime * 5f * turn, Space.World);
        }
    }

}
