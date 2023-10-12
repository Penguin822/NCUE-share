using UnityEngine;
using UnityEngine.UI;

public class Dog_BG : MonoBehaviour
{
    //canvas
    public GameObject bgCanvas;
    public GameObject bg;
    public Sprite bgPass;
    public Sprite bgFuture;

    public void ClickBackAR()
    {
        bgCanvas.SetActive(false);
    }
    public void ClickPass()
    {
        bg.GetComponent<Image>().sprite = bgPass;
    }
    public void ClickFuture()
    {
        bg.GetComponent<Image>().sprite = bgFuture;
    }
}