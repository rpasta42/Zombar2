using UnityEngine;
using System.Collections;

public class GyroController : MonoBehaviour {

   private Vector3 prevRot;
   /*private Vector3 prevPrevRot;
   private Vector3 prevPrevPrevRot;*/
   private float camVelX = 0.0f;
   private float camVelY = 0.0f;
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
      var accelRot = new Vector3(Input.acceleration.y*50.0f + 20.0f,
                                 -Input.acceleration.x*50.0f, 0.0f); //Input.acceleration.z*10);

      while (accelRot.x < 0)
         accelRot.x = 360 + accelRot.x;
      while (accelRot.y < 0)
         accelRot.y = 360 + accelRot.y;

      /*var tmpPrevRot = accelRot;
      if (prevRot != null && prevPrevRot != null && prevPrevPrevRot != null) {
         //accelRot = (accelRot + prevRot + prevPrevRot + prevPrevPrevRot) / 4.0f;
         prevRot = tmpPrevRot;
      }
      prevPrevPrevRot = prevPrevRot;
      prevPrevRot = prevRot;
      prevRot = tmpPrevRot;*/

      Vector3 currRot = transform.eulerAngles;
      accelRot.x = Mathf.SmoothDamp(currRot.x, accelRot.x, ref camVelX, 0.3f);
      accelRot.y = Mathf.SmoothDamp(currRot.y, accelRot.y, ref camVelY, 0.3f);
      transform.eulerAngles = accelRot;

      /*if (Mathf.Abs(accelRot.x - currRot.x) > ignoreSmallRot) {
         currRot.x = accelRot.x; //(currRot.x + accelRot.x)/2.0f;
      }
      if (Mathf.Abs(accelRot.y - currRot.y) > ignoreSmallRot) {
         currRot.y = accelRot.y; //(currRot.y + accelRot.y)/2.0f;
      }
      transform.eulerAngles = currRot;*/
	}
}
