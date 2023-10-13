using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class AutoPlaceItem : MonoBehaviour
{
    public GameObject cat;
    public GameObject box;
    //ARRaycastManager raycastManager;
    //List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();

    // Start is called before the first frame update
    void Start()
    {
        //raycastManager = GetComponent<ARRaycastManager>();
        cat.SetActive(false);
        box.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        //if (raycastManager.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.2f, 0f)), raycastHits, TrackableType.PlaneWithinPolygon))  //從畫面正中央偏下的位置射出射線，動物的生成位置較接近腳邊
        //{
        //    Pose hitPose = raycastHits[0].pose;
        //    cat.transform.position = hitPose.position;
            box.transform.position = new Vector3(box.transform.position.x, cat.transform.position.y, box.transform.position.z);
            box.SetActive(true);
            cat.SetActive(true);
            if (cat.activeInHierarchy)
            {
                Destroy(GetComponent<AutoPlaceItem>());
            }
        //}
    }
}
