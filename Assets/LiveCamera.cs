using UnityEngine;
using System.Collections;

public class LiveCamera : MonoBehaviour {

   void getPermission() {
      if (ContextCompat.checkSelfPermission(thisActivity,Manifest.permission.READ_CONTACTS)
              != PackageManager.PERMISSION_GRANTED) {
           if (ActivityCompat.shouldShowRequestPermissionRationale(thisActivity,
                  Manifest.permission.READ_CONTACTS)) {

              // Show an expanation to the user *asynchronously* -- don't block
              // this thread waiting for the user's response! After the user
              // sees the explanation, try again to request the permission.

          } else {

              // No explanation needed, we can request the permission.

              ActivityCompat.requestPermissions(thisActivity,
                      new String[]{Manifest.permission.READ_CONTACTS},
                      MY_PERMISSIONS_REQUEST_READ_CONTACTS);

              // MY_PERMISSIONS_REQUEST_READ_CONTACTS is an
              // app-defined int constant. The callback method gets the
              // result of the request.
          }
      }

   }

   // Use this for initialization
   IEnumerator Start () {
      /*yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
      if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
         yield return false;*/


      WebCamDevice[] devices = WebCamTexture.devices;

      string camToUse = null;
      var gotBackCam = false;
      for(int i = 0; i < devices.Length; i++) {
         camToUse = devices[i].name;
         Debug.Log("Camera: " + devices[i].name);
         if (!devices[i].isFrontFacing) {
            gotBackCam = true;
            break;
         }
      }
      if (camToUse == null)
         Debug.LogError("No camera found");
      if (gotBackCam == false)
         Debug.LogWarning("Only found front camera");

      GameObject plane = GameObject.Find("VidPlane");
      WebCamTexture cam = new WebCamTexture(camToUse); //(480, 480); //WebCamTexture();
      var m = plane.GetComponent<Renderer>().material;
      m.mainTexture = cam;
      var c = m.color;
      m.color = new Color(c.r, c.g, c.b, 0.4f);
      cam.Play();
   }

	// Update is called once per frame
   void Update () {

   }
}
