using UnityEngine;
using System.Collections;

//TODO: store isGpsEnabled isCompassEnabled isGyroEnabled, and if they
//are, have option to automatically update them in Update() for every frame
//GPS: multiply by 100,000 for displaying location


public class Trackers : MonoBehaviour {
	public delegate void OnError(int level, string text);
	public OnError onError;

	//GPS
	//0 = success, 1 = disabledByUser, 2 = timedOut, 3 = noLocationInitFail
	public delegate void OnGpsInitFinish(int status);
	public OnGpsInitFinish onGpsInitFinish;

	public LocationInfo firstLocation;
	public LocationInfo lastLocation;
	public LocationInfo currentLocation;
	//END GPS

	//COMPASS
	public float compassHeading1;
	public float compassHeading2;
	public Vector3 compassRotation; //based on compassHeading2

	public Vector3 firstCompassRotation;
	public Vector3 lastCompassRotation;
	//END COMPASS

	//GYRO 
	public Vector3 gyro;
	//END GYRO

	// Use this for initialization
	void Start () {}

	//GPS
	IEnumerator _StartGpsHelper(int maxWait, float desiredAccuracyInMeters, float updateDistanceInMeters) {
		if (!Input.location.isEnabledByUser) {//check if location service enabled
			onGpsInitFinish(1);
			yield break;
		}

		Input.location.Start(desiredAccuracyInMeters, updateDistanceInMeters);

		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
			yield return new WaitForSeconds(1);
			maxWait--;
		}

		if (maxWait < 1) {
			Debug.LogWarning("GPS Timed out");
			onGpsInitFinish(2);
			yield break;
		}

		if (Input.location.status == LocationServiceStatus.Failed) {
			Debug.LogWarning("GPS Input.location.Start() failed"); //print("Unable to determine device location");
			onGpsInitFinish(3);
			yield break;
		}
		var l = firstLocation = UpdateGps();
		Debug.Log("Location: " + l.latitude + " " + l.longitude + " " + l.altitude +
				  l.horizontalAccuracy + " " + l.timestamp);
		
	}
	public void StartGps(int maxWait = 20, float desiredAccuracyInMeters = 10f, float updateDistanceInMeters = 10f) {
		StartCoroutine(_StartGpsHelper(maxWait, desiredAccuracyInMeters, updateDistanceInMeters));
	}
	public LocationInfo UpdateGps() {
		currentLocation = Input.location.lastData;
		return currentLocation;
	}
	public void StopGps() {
		lastLocation = UpdateGps();
		Input.location.Stop();
	}
	//END GPS

	//COMPASS
	public void StartCompass() { //TODO: errors
		Input.compass.enabled = true; //TODO: might need Location too?
		firstCompassRotation = UpdateCompass();
	}
	public void StopCompass() {
		lastCompassRotation = UpdateCompass();
		Input.compass.enabled = false;
	}
	public Vector3 UpdateCompass() {
		compassHeading1 = Input.compass.magneticHeading;
		compassHeading2 = Input.compass.trueHeading;

		compassRotation = Quaternion.Euler(0, compassHeading2, 0).eulerAngles;
		return compassRotation;
	}
	//END COMPASS

	public void StartGyro() {}
	public void StopGyro() {}
	public Vector3 UpdateGyro() {
		gyro = Input.acceleration;
		return gyro;
	}

	// Update is called once per frame
	void Update () {}
}
