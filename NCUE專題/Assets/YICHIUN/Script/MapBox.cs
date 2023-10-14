using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class MapBox : MonoBehaviour
{
    //從mapbox拿地圖
    public string accessToken;
    public float centerLatitude = 23.933f;
    public float centerLongitude = 120.6112f;
    public float zoom = 10.0f;
    public int bearing = 0;
    public int pitch = 0;

    public enum style { Light, Dark, Streets, Outdoors, Satellite, SatelliteStreets };
    public style mapStyle = style.Streets;

    public enum resolution { low = 1, high = 2 };
    public resolution mapResolution = resolution.low;

    private int mapWidth = 840;
    private int mapHeight = 960;
    private string[] styleStr = new string[] { "light-v10", "dark-v10", "streets-v11", "outdoors-v11", "satellite-v9", "satellite-streets-v11" };
    private string url = "";
    private Rect rect;
    private bool updateMap = true;

    private string accessTokenLast;
    private float centerLatitudeLast = 23.933f;
    private float centerLongitudeLast = 120.6112f;
    private float zoomLast = 10.0f;
    private int bearingLast = 0;
    private int pitchLast = 0;
    private style mapStyleLast = style.Streets;
    private resolution mapResolutionLast = resolution.low;

    public GameObject mapMarkersPrefab;

    //座標
    public Image markerImage;

    //移動縮放
    private Vector3 touchStart;
    private float zoomSpeed = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetMapbox());
        rect = gameObject.GetComponent<RawImage>().rectTransform.rect;
        mapWidth = (int)Math.Round(rect.width);
        mapHeight = (int)Math.Round(rect.height);

        GenerateRandomMarkers();
    }

    // Update is called once per frame
    void Update()
    {
        if (updateMap && (accessTokenLast != accessToken || !Mathf.Approximately(centerLatitudeLast, centerLatitude) || !Mathf.Approximately(centerLongitudeLast, centerLongitude) || zoomLast != zoom ||
        bearingLast != bearing || pitchLast != pitch || mapStyleLast != mapStyle || mapResolutionLast != mapResolution))
        {
            rect = gameObject.GetComponent<RawImage>().rectTransform.rect;
            mapWidth = (int)Math.Round(rect.width);
            mapHeight = (int)Math.Round(rect.height);
            StartCoroutine(GetMapbox());
            updateMap = false;
        }

        HandleInput();//縮放功能更新

        if (updateMap && (accessTokenLast != accessToken || !Mathf.Approximately(centerLatitudeLast, centerLatitude) || !Mathf.Approximately(centerLongitudeLast, centerLongitude) || zoomLast != zoom ||
        bearingLast != bearing || pitchLast != pitch || mapStyleLast != mapStyle || mapResolutionLast != mapResolution))
        {
            rect = gameObject.GetComponent<RawImage>().rectTransform.rect;
            mapWidth = (int)Math.Round(rect.width);
            mapHeight = (int)Math.Round(rect.height);
            StartCoroutine(GetMapbox());
            updateMap = false;
        }
    }

    IEnumerator GetMapbox()
    {
        url = "https://api.mapbox.com/styles/v1/mapbox/" + styleStr[(int)mapStyle] + "/static/" + centerLongitude + "," + centerLatitude + "," + zoom + "," + bearing + "," + pitch + "/" + mapWidth + "x" +
        mapHeight + "?" + "access_token=" + accessToken;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("WWW ERROR: " + www.error);
        }
        else
        {
            gameObject.GetComponent<RawImage>().texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            accessTokenLast = accessToken;
            centerLatitudeLast = centerLatitude;
            centerLongitudeLast = centerLongitude;
            zoomLast = zoom;
            bearingLast = bearing;
            pitchLast = pitch;
            mapStyleLast = mapStyle;
            mapResolutionLast = mapResolution;
            updateMap = true;
        }
    }

    void GenerateRandomMarkers()//生成隨機羽毛的位置
    {
        for (int i = 0; i < 10; i++)
        {
            // 隨機生成經緯度座標
            float randomLat = centerLatitude + UnityEngine.Random.Range(-0.01f, 0.01f); // 調整範圍以避免超出 1000 公尺
            float randomLon = centerLongitude + UnityEngine.Random.Range(-0.01f, 0.01f); // 調整範圍以避免超出 1000 公尺

            // 創建標記 GameObject
            GameObject marker = Instantiate(mapMarkersPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
            marker.transform.SetParent(GameObject.Find("MapMarkers").transform); // 設置父級為 "MapMarkers"
            marker.transform.localPosition = YourMapboxLatLonToUnityPosition(randomLat, randomLon); // 將經緯度轉換為 Unity 世界座標
        }
    }

    Vector3 YourMapboxLatLonToUnityPosition(float latitude, float longitude)//換算世界座標到unity
    {
        // 计算地理坐标的相对位置
        float latitudeDelta = latitude - centerLatitude;
        float longitudeDelta = longitude - centerLongitude;

        // 能需要根据世界座標與场景尺寸和缩放
        float scaleFactor = 100000.0f; 
        Vector3 unityPosition = new Vector3(longitudeDelta * scaleFactor, 0f, latitudeDelta * scaleFactor);

        return unityPosition;
    }

    public void UpdatePosition(Vector3 worldPosition)//處理座標變化
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        markerImage.transform.position = screenPosition;
    }

    void HandleInput()//地圖縮放
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 touchEnd = Input.mousePosition;
            Vector3 delta = touchEnd - touchStart;

            // 移动地图中心坐标
            centerLatitude -= delta.y * zoomSpeed;
            centerLongitude += delta.x * zoomSpeed;

            // 限制移动范围，以防超出地图范围
            centerLatitude = Mathf.Clamp(centerLatitude, -90f, 90f);
            centerLongitude = Mathf.Clamp(centerLongitude, -180f, 180f);

            // 更新地图
            updateMap = true;

            // 保存新的触摸开始点
            touchStart = touchEnd;
        }

        // 处理缩放
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            // 修改缩放级别
            zoom += scroll;
            zoom = Mathf.Clamp(zoom, 0f, 20f);

            // 更新地图
            updateMap = true;
        }
    }


}
