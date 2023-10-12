using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Dog_Button : MonoBehaviour
{
   
public void Back(string Dog_Defult) 
    {
        SceneManager.LoadScene(Dog_Defult);
    }
}
