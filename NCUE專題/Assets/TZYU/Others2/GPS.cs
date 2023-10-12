using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPS : MonoBehaviour
{
    public static GPS Instance { get; private set; } // Changed InstancePP to Instance
    public float latitude;
    public float longitude; // Corrected the spelling of "longitude"

    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        StartCoroutine(StartLocationService()); // Fixed typo "StatCoroutine" to "StartCoroutine"
    }

    private IEnumerator StartLocationService()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("User has not enabled GPS");
            yield break;
        }

        Input.location.Start();
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) // Changed "locationServiceStatus" to "LocationServiceStatus"
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }
        if (maxWait <= 0)
        {
            Debug.Log("Time out");
            yield break;
        }
        //if (Input.location.status == LocationServiceStatus.Fail)
        //{
        //    Debug.Log("Time out");
          //  yield break;
        //}
        if(Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determin device location");
            yield break;
        }
        latitude = Input.location.lastData.latitude;
        longitude = Input.location.lastData.longitude; // Corrected the spelling of "longtitude"

        yield break;
    }
}
