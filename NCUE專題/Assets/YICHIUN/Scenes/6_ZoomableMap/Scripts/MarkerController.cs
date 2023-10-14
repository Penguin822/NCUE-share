using UnityEngine;
using UnityEngine.UI;

public class MarkerController : MonoBehaviour
{
    public Text titleText;
    public Text descriptionText;
    public GameObject infoPanel;
    public GameObject Marker;

    private MarkInfo markerInfo;
    private bool isPanelVisible = false;
    private void Start()
    {
        if(infoPanel != null)
        {
            infoPanel.SetActive(false);
        }     
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            if(hit.collider !=null && hit.collider.gameObject == Marker)
            {
                if (infoPanel != null)
                {
                    infoPanel.SetActive(true);
                }
            }
        }
    }

}


