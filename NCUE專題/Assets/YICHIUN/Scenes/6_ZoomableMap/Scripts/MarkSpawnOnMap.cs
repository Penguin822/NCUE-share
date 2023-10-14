using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;


public class MarkSpawnOnMap : MonoBehaviour
{
    [SerializeField]
    AbstractMap _map;

    [SerializeField]
    [Geocode]
    string[] _locationStrings;
    Vector2d[] _locations;

    [SerializeField]
    float _spawnScale = 100f;

    [SerializeField]
    GameObject _markerPrefab;

    List<GameObject> _spawnedObjects;

    private int cloneCount = 0;

    [SerializeField]
    string[] _markerInfo;

    public Text title;
    public Text description;
    public GameObject infoPanel;
    public GameObject Marker;
    Dictionary<string, MarkInfo> _markInfoDictionary;

    void Start()
    {
        _locations = new Vector2d[_locationStrings.Length];
        _spawnedObjects = new List<GameObject>();
        _markInfoDictionary = new Dictionary<string, MarkInfo>();

        for (int i = 0; i < _locationStrings.Length; i++)
        {
            var locationString = _locationStrings[i];
            _locations[i] = Conversions.StringToLatLon(locationString);

            // 创建 MarkInfo 并存储到字典中
            var markInfo = new MarkInfo();

            if(i < _markerInfo.Length)
            {
                string[] infoParts = _markerInfo[i].Split(',');

                if(infoParts.Length >= 2)
                {
                    markInfo.title = infoParts[0];
                    markInfo.description = infoParts[1];
                }
            }
            _markInfoDictionary.Add(locationString, markInfo);

            var instance = Instantiate(_markerPrefab);
            instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
            instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);

            instance.name = "MapMarker" + i;

            _spawnedObjects.Add(instance);
        }

        if(infoPanel != null)
        {
            infoPanel.SetActive(false);
        }
    }

    private void Update()
    {
        int count = _spawnedObjects.Count;
        for (int i = 0; i < count; i++)
        {
            var spawnedObject = _spawnedObjects[i];
            var location = _locations[i];
            spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
            spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
        }

        // 在这里根据当前位置查找对应的 MarkInfo，并更新 UI
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null && hit.collider.CompareTag("MapMarker"))
            {
                GameObject hitObject = hit.collider.gameObject;
                hitObject.transform.localScale = new Vector3(_spawnScale * 1.2f, _spawnScale * 1.2f, _spawnScale * 1.2f);
                
                if (infoPanel != null)
                {
                    //獲取被點擊物件
                    string markerName = hit.collider.gameObject.name;

                    // 使用正则表达式提取名字中的数字部分
                    Match match = Regex.Match(markerName, @"\d+");
                    if (match.Success)
                    {
                        int markerNumber = int.Parse(match.Value);

                        // 使用 markerNumber 訪問相應的訊息
                        if (markerNumber >= 0 && markerNumber < _markerInfo.Length)
                        {
                            string[] infoParts = _markerInfo[markerNumber].Split(',');

                            if (infoParts.Length >= 2)
                            {
                                title.text = infoParts[0];
                                description.text = infoParts[1];

                                infoPanel.SetActive(true);
                            }
                        }
                    }
                }
            }
            else
            {
                // 如果點擊其他區域或其他標記，隐藏面板
                if (infoPanel != null)
                {
                    infoPanel.SetActive(false);
                }

                // 還原 mark 的 scale
                foreach (var spawnedObject in _spawnedObjects)
                {
                    spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
                }
            }
        }
    }
}
