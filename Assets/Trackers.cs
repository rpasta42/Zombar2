using UnityEngine;
using System.Collections;

//GPS
//multiply by 100,000 for displaying location


public class Trackers : MonoBehaviour {
	public delegate void OnError(int level, string text);

	//GPS
	//0 = success, 1 = disabledByUser, 2 = timedOut, 3 = noLocationInitFail
	public delegate void OnGpsInitFinish(int NavMeshPathStatus);

	public OnError onError;
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
			OnGpsInitFinish(1);
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
	void StartGps(int maxWait = 20, float desiredAccuracyInMeters = 10f, float updateDistanceInMeters = 10f) {
		_StartGpsHelper(maxWait, desiredAccuracyInMeters, updateDistanceInMeters);
	}
	LocationInfo UpdateGps() {
		currentLocation = Input.location.lastData;
		return currentLocation;
	}
	void StopGps() {
		lastLocation = UpdateGps();
		Input.location.Stop();
	}
	//END GPS

	//COMPASS
	void StartCompass() { //TODO: errors
		Input.compass.enabled = true; //TODO: might need Location too?
		firstCompassRotation = UpdateCompass();
	}
	void StopCompass() {
		lastCompassRotation = UpdateCompass();
		Input.compass.enabled = false;
	}
	Vector3 UpdateCompass() {
		compassHeading1 = Input.compass.magneticHeading;
		compassHeading2 = Input.compass.trueHeading;

		compassRotation = Quaternion.Euler(0, compassHeading2, 0);
		return compassRotation;
	}
	//END COMPASS

	void StartGyro() {}
	void StopGyro() {}
	Vector3 UpdateGyro() {
		gyro = Input.acceleration;
		return gyro;
	}

	// Update is called once per frame
	void Update () {
	}
}
