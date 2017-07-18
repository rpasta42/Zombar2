using UnityEngine;
using System.Collections;

public class CubeFaller : MonoBehaviour {

   public int frames_per_spawn;
   public bool disabled;
   private int curr_frame;

    // Use this for initialization
    void Start () {
        curr_frame = 0;

        GameObject fall_location = GameObject.Find("Cube Faller Ground");
        var rend = fall_location.GetComponent<Renderer>();
        var c = rend.material.color;
        rend.material.color = new Color(c.r, c.g, c.b, 0.30f); //new Color(0.0f, 0.0f, 0.0f, 0.0f);
    }

   void spawnCube() {
      var x = Random.Range(-6.0f, 10.0f);
      var y = 5.0f;
      var z = Random.Range(0.0f, 16.0f);
      GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

      cube.transform.position = new Vector3(x, y, z);

      Renderer rend = cube.GetComponent<Renderer>();
      //rend.material = Resources.Load("MyMaterial", typeof(Material)) as Material;
      var red = Random.Range(0, 100) / 100.0f;
      var green = Random.Range(0, 50) / 100.0f;
      var blue = Random.Range(50, 100) / 100.0f;
      var c = new Color(red, green, blue, 0);
      rend.material.SetColor("_Color", c);
      Rigidbody rb = cube.AddComponent<Rigidbody>();
   }

	// Update is called once per frame
	void Update () {
        if (disabled) return;
        if (curr_frame++ > frames_per_spawn) {
            spawnCube();
            curr_frame = 0;
        }
	}
}
