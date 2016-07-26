using UnityEngine;
using System.Collections;

public class Syncer : MonoBehaviour {
	public bool gpsEnabled;
	public bool compassEnabled;
	public bool gyroEnabled;

	public GameObject mainPlayerCamera;
	public GameObject compassObject;
	public GameObject gpsObject;

	private Trackers tracker;

	public float gyroCamAcceleration; //50-150.0f
	public float gyroInitialAngleStart; //90.0f

	private bool goodGps;
	public int gpsNumFramesToUpdate;
	private int gpsFrameCounter;

	public int compassNumFramesToUpdate;
	private int compassFrameCounter;

	public int gyroNumFramesToUpdate;
	private int gyroFrameCounter;

	// Use this for initialization
	void Start() {
		goodGps = false;
		compassFrameCounter = gpsFrameCounter = gyroFrameCounter = 0;

		tracker = GetComponent<Trackers>();

		tracker.onError = (int level, string text) => {};

		tracker.onGpsInitFinish = (int status) => {
			if (status == 0) goodGps = true;
		};
		//goodGps = true;

		if (compassEnabled)
			tracker.StartCompass();
		if (gpsEnabled)
			tracker.StartGps(20, 0.1f, 0.1f); //1f, 1f);
		if (gyroEnabled)
			tracker.StartGyro();
	}

	void LocationUpdate() {
		var first = tracker.firstLocation;
		var current = tracker.UpdateGps();

		var xDiff = first.latitude - current.latitude;
		var yDiff = first.longitude - current.longitude;

		//10,000 = too small
		//500,000 = too much
		//100,000 = default, ok-ish
		//50,000 = current test
		//GameObject cam = GameObject.Find("Main Camera");
		var gpsScale = 50000; //100000; //200000; //50000;
		//mainPlayerCamera.transform.position = new Vector3(gpsScale * xDiff, 0.0f, gpsScale * yDiff);
		gpsObject.transform.position = new Vector3(gpsScale * xDiff, 0.0f, gpsScale * yDiff);
	}

	void gyroUpdate() {
		var g = tracker.UpdateGyro();
		var accelRot = new Vector3(
								g.y * gyroCamAcceleration + gyroInitialAngleStart,
			               -g.x * gyroCamAcceleration,
			               0.0f);
		
		var x = Mathf.LerpAngle(transform.eulerAngles.x, accelRot.x, 0.1f);
		var y = Mathf.LerpAngle(transform.eulerAngles.y, accelRot.y, 0.1f);
		transform.eulerAngles = new Vector3(x, y, transform.eulerAngles.z);
	}

	void compassUpdate() {
		var c = tracker.UpdateCompass();

		//transform.rotation = Quaternion.Euler(0, tracker.compassHeading1, 0);
		//transform.localEulerAngles = c;
		//compassObject.transform.eulerAngles = c;
		//compassObject.transform.localEulerAngles = c; //why doesn't this work?
		compassObject.transform.rotation = Quaternion.Euler(0, tracker.compassHeading2+90, 90);
	}

	// Update is called once per frame
	void Update () {
		if (gpsFrameCounter++ > gpsNumFramesToUpdate && gpsEnabled && goodGps) {
			gpsFrameCounter = 0;
			LocationUpdate();
		}
		if (compassFrameCounter++ > compassNumFramesToUpdate && compassEnabled) {
			compassFrameCounter = 0;
			compassUpdate();
		}
		if (gyroFrameCounter++ > gyroNumFramesToUpdate && gyroEnabled) {
			gyroFrameCounter = 0;
			gyroUpdate();
		}
	}
}
