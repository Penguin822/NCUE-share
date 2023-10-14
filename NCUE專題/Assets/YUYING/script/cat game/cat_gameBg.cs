using UnityEngine;
using UnityEngine.UI;

public class cat_gameBg : MonoBehaviour
{
    //canvas
    public GameObject bgCanvas;
    public GameObject bg;

    public Sprite bgNow;
    public Sprite bgPast;
    public Sprite bgFuture;

    public void ClickBg()
    {
        bgCanvas.SetActive(true);
        bg.GetComponent<Image>().sprite = bgNow;
    }
    public void ClickBackAR()
    {
        bgCanvas.SetActive(false);
    }
    public void ClickPass()
    {
        bg.GetComponent<Image>().sprite = bgPast;
    }
    public void ClickNow()
    {
        bg.GetComponent<Image>().sprite = bgNow;
    }
    public void ClickFuture()
    {
        bg.GetComponent<Image>().sprite = bgFuture;
    }
}
