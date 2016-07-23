using UnityEngine;
using System.Collections;

public class LiveCamera : MonoBehaviour {
   public bool isMobile;

   // Use this for initialization
   IEnumerator Start () {
      yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
      if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
         yield return false;

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
      //var c = m.color;
      //m.color = new Color(c.r, c.g, c.b, 0.3f);
      cam.Play();

      if (isMobile) {
         var a = transform.eulerAngles;
         transform.eulerAngles = new Vector3(0.0f, a.y, a.z);
         //transform.localScale = new Vector3(11.5f, transform.localScale.y, 23.0f);
      }

      yield return true;
   }

	// Update is called once per frame
   void Update () {

   }
}
