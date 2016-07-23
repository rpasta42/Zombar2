using UnityEngine;
using System.Collections;

public class GyroController : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
      var accelRot = new Vector3(Input.acceleration.y*50.0f + 20.0f,
                                 -Input.acceleration.x*50.0f,
                                 0.0f);//-Input.acceleration.z*50.0f);


      var x = Mathf.LerpAngle(transform.eulerAngles.x, accelRot.x, 0.1f);
      var y = Mathf.LerpAngle(transform.eulerAngles.y, accelRot.y, 0.1f);
      //var z = Mathf.LerpAngle(transform.eulerAngles.z, accelRot.z, 0.1f);

      transform.eulerAngles = new Vector3(x, y, transform.eulerAngles.z); //z
	}
}
