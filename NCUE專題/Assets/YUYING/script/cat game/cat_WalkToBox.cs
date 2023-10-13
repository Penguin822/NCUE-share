using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class cat_WalkToBox : MonoBehaviour
{
    public GameObject cat;
    public GameObject box;
    public GameObject btn;
    public GameObject ARCam;
    public GameObject panelWarn;

    Animator catAnimator;
    bool coll = false;

    // Start is called before the first frame update
    void Start()
    {
        //cat anim
        catAnimator = cat.GetComponent<Animator>();
        catAnimator.SetBool("walk", true);

        btn.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        var arCamDistance = cat.transform.position.z - ARCam.transform.position.z;

        //cat translate
        if (coll == false)
        {           
            if (Mathf.Abs(arCamDistance) > 2f)
            {
                catAnimator.SetBool("walk", false);
                panelWarn.SetActive(true);
            }

            if (Mathf.Abs(arCamDistance) < 2f)
            {
                panelWarn.SetActive(false);
                catAnimator.SetBool("walk", true);
                var direction = box.transform.position - cat.transform.position;
                direction.y = 0;
                cat.transform.Translate(direction.normalized * Time.deltaTime * 0.25f, Space.World);

                var angle = Vector3.Angle(cat.transform.forward, direction);
                var cross = Vector3.Cross(cat.transform.forward, direction);

                var turn = cross.y >= 0 ? 1f : -1f;
                //Debug.Log("angle= " + angle);
                //Debug.Log("cross= " + cross);
                //Debug.Log("turn= " + turn);
                cat.transform.Rotate(cat.transform.up, angle * Time.deltaTime * 5f * turn, Space.World);
            }        
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        catAnimator.SetBool("walk", false);
        coll = true;
        btn.SetActive(true);
    }

    public void ClickOpen()
    {
        Destroy(GameObject.FindGameObjectWithTag("Sound"));
        SceneManager.LoadScene("gameEnd");
    }

    public void ClickBack()
    {
        SceneManager.LoadScene("cat_model");
    }
}
