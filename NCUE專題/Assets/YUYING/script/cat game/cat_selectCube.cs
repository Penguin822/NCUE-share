using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class cat_selectCube : MonoBehaviour
{
    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;

    //canvas
    public GameObject panelBg;
    public GameObject panelLose;
    public Text chance;

    //cat anim
    public GameObject cat;
    private Animator animatorCat;

    //audio
    public AudioSource audioSourceWin;
    public AudioSource audioSourceLose;

    //box anim
    public GameObject box;
    public GameObject box1;
    public GameObject box2;
    private Animator animatorBox;
    private Animator animatorBox1;
    private Animator animatorBox2;
    public RuntimeAnimatorController newBoxController;

    public GameObject fish;

    int wrong = 0;
    int correct = 0;

    // Start is called before the first frame update
    void Start()
    {
        animatorCat = cat.GetComponent<Animator>();
        animatorBox = box.GetComponent<Animator>();
        animatorBox1 = box1.GetComponent<Animator>();
        animatorBox2 = box2.GetComponent<Animator>();
        animatorBox.runtimeAnimatorController = newBoxController;
        animatorBox1.runtimeAnimatorController = newBoxController;
        animatorBox2.runtimeAnimatorController = newBoxController;
        DeleteScript();
    }

    void Update()
    {
        chance.text = wrong+correct + "/2";

        // Highlight
        if (highlight != null)
        {
            highlight.gameObject.GetComponent<Outline>().enabled = false;
            highlight = null;            
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit)) //Make sure you have EventSystem in the hierarchy before using EventSystem
        {
            highlight = raycastHit.transform;
            if (highlight.CompareTag("Selectable") && highlight != selection)
            {
                if (highlight.gameObject.GetComponent<Outline>() != null)
                {
                    highlight.gameObject.GetComponent<Outline>().enabled = true;                   
                }
                else
                {
                    Outline outline = highlight.gameObject.AddComponent<Outline>();
                    outline.enabled = true;
                    highlight.gameObject.GetComponent<Outline>().OutlineColor = Color.green;
                    highlight.gameObject.GetComponent<Outline>().OutlineWidth = 7.0f;
                }
            }
            else
            {
                highlight = null;
            }
        }

        // Selection
        if (Input.GetMouseButtonDown(0))
        {           
            if (highlight)
            {
                if (selection != null)
                {
                    selection.gameObject.GetComponent<Outline>().enabled = false;
                }
                selection = raycastHit.transform;
                selection.gameObject.GetComponent<Outline>().enabled = true;
                highlight = null;

                //確認選擇的箱子中是否有魚
                if (selection.gameObject.name == "Cube")
                {
                    animatorCat.SetBool("lost", false);
                    animatorCat.SetBool("win", true);
                    animatorBox.SetBool("open", true);                   
                    audioSourceWin.Play();
                    audioSourceLose.Pause();
                    correct++;
                    panelBg.SetActive(true);
                    NextScene();
                }
                if (selection.gameObject.name != "Cube")
                {
                    if (selection.gameObject.name == "Cube (1)")
                    {
                        animatorBox1.SetBool("open", true);
                    }
                    else
                    {
                        animatorBox2.SetBool("open", true);
                    }

                    animatorCat.SetBool("lost", true);
                    audioSourceLose.Play();
                    audioSourceWin.Pause();
                    wrong++;
                    if(wrong == 2)
                    {
                        panelBg.SetActive(true);
                        panelLose.SetActive(true);
                    }
                }
            }
            else
            {   
                //當canvas存在時不執行，若執行，點擊畫面時highlight會消失
                if (selection && panelBg.activeInHierarchy == false)
                {
                    selection.gameObject.GetComponent<Outline>().enabled = false;
                    selection = null;
                }
            }         
        }

        if(correct == 1)
        {
            Vector3 fishDes = new Vector3(-0.12f, 2, 7.9f);
            fish.transform.position = Vector3.MoveTowards(fish.transform.position, fishDes, 6 * Time.deltaTime);
        }
    }

    //延遲進入下一關(等聲音播完)
    public void NextScene()
    {
        StartCoroutine(NextSceneAfterDelay(4));
    }
    private IEnumerator NextSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("cat_gameLAR");
    }

    //延遲刪除cat_box script
    public void DeleteScript()
    {
        StartCoroutine(DeleteScriptAfterDelay(1));
    }
    private IEnumerator DeleteScriptAfterDelay(float delay)
    {
        
        yield return new WaitForSeconds(delay);

        cat_canvasChooseBox.boxMovingEnd = true;

        Destroy(box.GetComponent<cat_box>());
        Destroy(box1.GetComponent<cat_box>());
        Destroy(box2.GetComponent<cat_box>());       
    }
}
