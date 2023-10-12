using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPet : MonoBehaviour
{
    public GameObject dog;
    public GameObject cat;
    public GameObject bird;

    private void Start()
    {
        dog.SetActive(true);
        cat.SetActive(false);
        bird.SetActive(false);
    }

    public void BtnDog()
    {
        dog.SetActive(true);
        cat.SetActive(false);
        bird.SetActive(false);
    }
    public void BtnCat()
    {
        dog.SetActive(false);
        cat.SetActive(true);
        bird.SetActive(false);
    }
    public void BtnBird()
    {
        dog.SetActive(false);
        cat.SetActive(false);
        bird.SetActive(true);
    }
}
