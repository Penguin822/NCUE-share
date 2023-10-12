using System.Collections;
using UnityEngine;
public enum LocationState{
	Disabled,
	TimedOut,
	Failed,
	Enabled
}

public class GPSManager : MonoBehaviour {

	public static int SCREEN_DENSITY;
	private GUIStyle debugStyle;
	// Approximate radius of the earth (in kilometers)
	const float EARTH_RADIUS = 6371;
	private LocationState state;
	// Position on earth (in degrees)
	private float latitude;
	private float longitude;
	// Distance walked (in meters)
	private float distance;
	// Coins obtained (1 for every 100 meters walked)
	private int coins;

	// Use this for initialization
	IEnumerator Start () {
		if(Screen.dpi > 0f){
			SCREEN_DENSITY = (int)(Screen.dpi / 160f);
		}else{
			SCREEN_DENSITY = (int)(Screen.currentResolution.height / 600);
		}
		debugStyle = new GUIStyle ();
		debugStyle.fontSize = 16 * SCREEN_DENSITY;
		debugStyle.normal.textColor = Color.white;

		state = LocationState.Disabled;
		latitude = 0f;
		longitude = 0f;
		distance = 0f;
		coins = 0;
		if(Input.location.isEnabledByUser){
			Input.location.Start();
			int waitTime = 15;
			while(Input.location.status == LocationServiceStatus.Initializing && waitTime > 0){
				yield return new WaitForSeconds(1);
				waitTime--;
			}
			if(waitTime == 0){
				state = LocationState.TimedOut;
			}else if(Input.location.status == LocationServiceStatus.Failed){
				state = LocationState.Failed;
			}else{
				state = LocationState.Enabled;
				latitude = Input.location.lastData.latitude;
				longitude = Input.location.lastData.longitude;
			}
		}
	}

	IEnumerator OnApplicationPause(bool pauseState){
		if(pauseState){
			Input.location.Stop();
			state = LocationState.Disabled;
		}else{
			Input.location.Start();
			int waitTime = 15;
			while(Input.location.status == LocationServiceStatus.Initializing && waitTime > 0){
				yield return new WaitForSeconds(1);
				waitTime--;
			}
			if(waitTime == 0){
				state = LocationState.TimedOut;
			}else if(Input.location.status == LocationServiceStatus.Failed){
				state = LocationState.Failed;
			}else{
				state = LocationState.Enabled;
				latitude = Input.location.lastData.latitude;
				longitude = Input.location.lastData.longitude;
			}
		}
	}

void OnGUI(){
    Rect guiBoxRect = new Rect(30, 30, Screen.width-60, Screen.height-60);

    GUI.skin.box.fontSize = 32 * SCREEN_DENSITY;

    float buttonHeight = guiBoxRect.height / 7;

    switch(state){
        case LocationState.Enabled:
            Rect distanceTextRect = new Rect(guiBoxRect.x + 30, guiBoxRect.y + 30, guiBoxRect.width - 60, buttonHeight);

            //GUI.skin.label.fontSize = 32 * SCREEN_DENSITY;
            //GUI.skin.label.alignment = TextAnchor.UpperLeft;
            //GUI.Label(distanceTextRect, "latitude: " + latitude.ToString() + "\nlongitude: " + longitude.ToString() + "\nDistance walked: " + distance.ToString() + "m");
            break;

        case LocationState.Disabled:
            Rect disabledTextRect = new Rect(guiBoxRect.x + 30, guiBoxRect.y + guiBoxRect.height / 2,guiBoxRect.width - 60, buttonHeight * 2);

            //GUI.skin.label.fontSize = 40 * SCREEN_DENSITY;
            //GUI.skin.label.alignment = TextAnchor.UpperLeft;
            break;

        case LocationState.Failed:
            Rect failedTextRect = new Rect(guiBoxRect.x + 30, guiBoxRect.y + guiBoxRect.height / 2,guiBoxRect.width - 60, buttonHeight * 2);

            //GUI.skin.label.fontSize = 40 * SCREEN_DENSITY;
            //GUI.skin.label.alignment = TextAnchor.UpperLeft;
            //GUI.Label(failedTextRect, "Failed to initialize location service.\n" +"Try again later.");
            break;

        case LocationState.TimedOut:
            Rect timeOutTextRect = new Rect(guiBoxRect.x + 30, guiBoxRect.y + guiBoxRect.height / 2,guiBoxRect.width - 60, buttonHeight * 2);

            //GUI.skin.label.fontSize = 40 * SCREEN_DENSITY;
            //GUI.skin.label.alignment = TextAnchor.UpperLeft;
            //GUI.Label(timeOutTextRect, "Connection timed out. Try again later.");
            break;
    }
}




	// The Haversine formula
	// Veness, C. (2014). Calculate distance, bearing and more between
	//	Latitude/Longitude points. Movable Type Scripts. Retrieved from
    //	http://www.movable-type.co.uk/scripts/latlong.html
	float Haversine(ref float lastLatitude,ref float lastLongitude){
		float newLatitude = Input.location.lastData.latitude;
		float newLongitude = Input.location.lastData.longitude;
		float deltaLatitude = (newLatitude - lastLatitude) * Mathf.Deg2Rad;
		float deltaLongitude = (newLongitude - lastLongitude) * Mathf.Deg2Rad;
		float a = Mathf.Pow(Mathf.Sin(deltaLatitude / 2),2) +
			Mathf.Cos(lastLatitude * Mathf.Deg2Rad) * Mathf.Cos(newLatitude * Mathf.Deg2Rad) *
			Mathf.Pow(Mathf.Sin(deltaLongitude / 2),2);
		lastLatitude = newLatitude;
		lastLongitude = newLongitude;
		float c = 2 * Mathf.Atan2(Mathf.Sqrt(a),Mathf.Sqrt(1-a));
		return EARTH_RADIUS * c;
	}

	// Update is called once per frame
	void Update () {
		if(state == LocationState.Enabled){
			float deltaDistance = Haversine(ref latitude,ref longitude) * 1000f;
			if(deltaDistance > 0f){
				distance += deltaDistance;
				coins = (int)(distance / 100f);
			}
		}
	}
}