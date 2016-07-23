using UnityEngine;
using System.Collections;

public class GyroController : MonoBehaviour {

   private Vector3 prevRot;
   private Vector3 prevPrevRot;
   private Vector3 prevPrevPrevRot;

   public double ignoreSmallRot;

	// Use this for initialization
	void Start () {
      //prevRot = new Vector3(0.0f, 0.0f, 0.0f);
	}

	// Update is called once per frame
	void Update () {
      //Vector3 dir = Vector3.zero;
      //dir.x = Input.acceleration.y;
      //dir.z = Input.acceleration.x;
      var accelRot = new Vector3(20.0f+Input.acceleration.y*50.0f, -Input.acceleration.x*50.0f, 0.0f); //Input.acceleration.z*10);

      var tmpPrevRot = accelRot;
      if (prevRot != null && prevPrevRot != null && prevPrevPrevRot != null) {
         accelRot = (accelRot + prevRot + prevPrevRot + prevPrevPrevRot) / 4.0f;
         prevRot = tmpPrevRot;
      }
      prevPrevPrevRot = prevPrevRot;
      prevPrevRot = prevRot;
      prevRot = tmpPrevRot;

      Vector3 currRot = transform.eulerAngles;

      if (Mathf.Abs(accelRot.x - currRot.x) > ignoreSmallRot) {
         currRot.x = accelRot.x; //(currRot.x + accelRot.x)/2.0f;
      }
      if (Mathf.Abs(accelRot.y - currRot.y) > ignoreSmallRot) {
         currRot.y = accelRot.y; //(currRot.y + accelRot.y)/2.0f;
      }
      transform.eulerAngles = currRot;
	}
}
