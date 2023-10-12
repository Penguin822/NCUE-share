using UnityEngine;
public class AudioControl : MonoBehaviour
{
    public AudioClip soundEffect;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
         Debug.Log("Collision detected with tag: " + collision.gameObject.tag);

        if (collision.gameObject.CompareTag("Area"))
        {
            audioSource.enabled = true;
        }
    }
}
