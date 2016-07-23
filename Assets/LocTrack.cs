using UnityEngine;
using System.Collections;



public class LocTrack : MonoBehaviour {
   public LocationInfo last;
   private LocationInfo firstLoc;
   public int numFramesToUpdateLoc;

   private int frameCounter;

	// Use this for initialization
	IEnumerator Start () {
      if (!Input.location.isEnabledByUser) //check if location service enabled
         yield break;

      //Start server
      //(float desiredAccuracyInMeters = 10f, float updateDistanceInMeters = 10f)
      //Input.location.Start();
      Input.location.Start(1f, 1f);

      // Wait until service initializes
      int maxWait = 20;
      while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
         yield return new WaitForSeconds(1);
         maxWait--;
      }

      // Service didn't initialize in 20 seconds
      if (maxWait < 1) {
         print("Timed out");
         yield break;
      }

      // Connection has failed
      if (Input.location.status == LocationServiceStatus.Failed) {
         print("Unable to determine device location");
         yield break;
      }
      else {
         // Access granted and location value could be retrieved
         var l = Input.location.lastData;
         firstLoc = last = l;
         print("Location: " + l.latitude + " " + l.longitude + " " +
               l.altitude + " " + l.horizontalAccuracy + " " + l.timestamp);
      }

      // Stop service if there is no need to query location updates continuously
      //Input.location.Stop();
   }

   void getLoc() {
      last = Input.location.lastData;
   }

   void runLoc() {
      var xDiff = firstLoc.latitude - last.latitude;
      var yDiff = firstLoc.longitude - last.longitude;

      GameObject cam = GameObject.Find("Main Camera");
      cam.transform.position = new Vector3(100*xDiff, 0.0f, 100*yDiff);
   }

	// Update is called once per frame
	void Update () {
      if (frameCounter++ > numFramesToUpdateLoc) {
         getLoc();
         runLoc();
         frameCounter = 0;
      }
	}
}
