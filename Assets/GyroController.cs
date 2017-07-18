using UnityEngine;
using System.Collections;

public class GyroController : MonoBehaviour {
   public float accelCamFactor; //= 50-150.0f;
   public float initialAngleStart; //= 90.0f;

	// Use this for initialization
	void Start () {
		Input.location.Start();
		Input.compass.enabled = true;
	}

	// Update is called once per frame
	void Update () {
		var accelRot = new Vector3(Input.acceleration.y*accelCamFactor + initialAngleStart,
					   -Input.acceleration.x*accelCamFactor, //+ initialAngleStart,
					   0.0f);

        var x = Mathf.LerpAngle(transform.eulerAngles.x, accelRot.x, 0.1f);
        var y = Mathf.LerpAngle(transform.eulerAngles.y, accelRot.y, 0.1f);

        //TODO: good compass, just lerp it
        /*//transform.eulerAngles = new Vector3(x, y, transform.eulerAngles.z);
        var heading1 = -Input.compass.magneticHeading;
        var heading2 = -Input.compass.trueHeading;
        transform.rotation = Quaternion.Euler(0, heading2, 0);*/
	}
}
