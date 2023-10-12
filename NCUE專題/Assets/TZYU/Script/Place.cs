using UnityEngine;

public class Place : MonoBehaviour
{
    public GameObject objectToPlace; // 你的3D物件
    public double targetLatitude; // 目标纬度，使用double类型
    public double targetLongitude; // 目标经度，使用double类型

    // Approximate radius of the earth (in kilometers)
    const double EARTH_RADIUS = 6371;

    private void Start()
    {
        // 计算3D物体的位置
        Vector3 objectPosition = CalculateObjectPosition(targetLatitude, targetLongitude);

        // 实际放置3D物体
        Instantiate(objectToPlace, objectPosition, Quaternion.identity);
    }

    private Vector3 CalculateObjectPosition(double latitude, double longitude)
    {
        // 将经度和纬度转换为弧度
        double latitudeRad = latitude * Mathf.Deg2Rad;
        double longitudeRad = longitude * Mathf.Deg2Rad;

        // 计算距离
        double distance = EARTH_RADIUS * 1000.0; // 假设物体距离地球表面1公里

        // 将极坐标转换为笛卡尔坐标
        double x = distance * Mathf.Cos((float)latitudeRad) * Mathf.Cos((float)longitudeRad);
        double y = distance * Mathf.Sin((float)latitudeRad);
        double z = distance * Mathf.Cos((float)latitudeRad) * Mathf.Sin((float)longitudeRad);

        return new Vector3((float)x, (float)y, (float)z);
    }
}
