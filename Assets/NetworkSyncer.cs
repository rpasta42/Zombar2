using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkSyncer : NetworkBehaviour {
	public bool gpsEnabled;
	public bool compassEnabled;
	public bool gyroEnabled;

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
		if (!isLocalPlayer)
			return;
		
		goodGps = false;
		compassFrameCounter = gpsFrameCounter = gyroFrameCounter = 0;

		tracker = gameObject.AddComponent<Trackers>(); //GetComponent<Trackers>();

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

	[Command]
	void CmdSetPos(Vector3 v) {
		RpcSetPos(v);
	}
	[Command]
	void CmdSetRot2(int a, int b, int c) {
		transform.rotation = Quaternion.Euler(a, b, c);
	}
	[Command]
	void CmdSetRot1(Vector3 v) {
		RpcSetRot1(v);
	}

	[ClientRpc]
	void RpcSetPos(Vector3 v) {
		transform.position = v;
	}

	[ClientRpc]
	void RpcSetRot1(Vector3 v) {
		transform.eulerAngles = v;
	}

	[Command]
	void CmdLocationUpdate() {
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
		//transform.position = new Vector3(gpsScale * xDiff, 0.0f, gpsScale * yDiff);
		CmdSetPos(new Vector3(gpsScale * xDiff, 0.0f, gpsScale * yDiff));
	}

	[Command]
	void CmdGyroUpdate() {
		var g = tracker.UpdateGyro();
		var accelRot = new Vector3(
			g.y * gyroCamAcceleration + gyroInitialAngleStart,
			-g.x * gyroCamAcceleration,
			0.0f);

		var x = Mathf.LerpAngle(transform.eulerAngles.x, accelRot.x, 0.1f);
		var y = Mathf.LerpAngle(transform.eulerAngles.y, accelRot.y, 0.1f);
		//transform.eulerAngles = new Vector3(x, y, transform.eulerAngles.z);
		CmdSetRot1(new Vector3(x, y, transform.eulerAngles.z));
	}

	//[Command]
	void CompassUpdate() {
		var c = tracker.UpdateCompass();

		//transform.rotation = Quaternion.Euler(0, tracker.compassHeading1, 0);
		//transform.localEulerAngles = c;
		//compassObject.transform.eulerAngles = c;
		//compassObject.transform.localEulerAngles = c; //why doesn't this work?
		transform.rotation = Quaternion.Euler(0, tracker.compassHeading2+90, 90);
	}

	// Update is called once per frame
	void Update() {
		if (!isLocalPlayer)
			return;
		
		if (gpsFrameCounter++ > gpsNumFramesToUpdate && gpsEnabled && goodGps) {
			gpsFrameCounter = 0;
			CmdLocationUpdate();
		}
		if (compassFrameCounter++ > compassNumFramesToUpdate && compassEnabled) {
			compassFrameCounter = 0;
			CompassUpdate();
		}
		if (gyroFrameCounter++ > gyroNumFramesToUpdate && gyroEnabled) {
			gyroFrameCounter = 0;
			CmdGyroUpdate();
		}
	}
}
