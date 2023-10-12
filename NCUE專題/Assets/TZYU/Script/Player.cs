using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject buttonToActivate;
    public AudioClip collisionSound;
    public GameObject dog;
    private Animator run;
    static public bool coll = false;

    public void Start()
    {
        run = dog.GetComponent<Animator>(); 
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("bone"))
        {
            coll = true;
            Debug.Log("Ä²µo¸I¼²½d³ò");
            buttonToActivate.SetActive(true);
            run.SetBool("IsMoving", false);
            AudioSource.PlayClipAtPoint(collisionSound, transform.position);
        }
    }
  
}
