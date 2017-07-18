using UnityEngine;
using System.Collections;

public class CompassSwag : MonoBehaviour {
    private int start = 0;

    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
        return;
        if (start++ < 30) return;
        start = 0;

        GameObject c = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Rigidbody rb = c.AddComponent<Rigidbody>();

        var north = Input.compass.magneticHeading;


        //Vector3 forward = new Vector3(0, 1, 0);
        //forward = Quaternion.AngleAxis(-north, Vector3.forward) * forward;
        Vector3 forward = new Vector3(0, -north, 0);

        rb.AddForce(forward);
        //Quaternion.Euler(0, -north, 0);
        /*//transform.eulerAngles = new Vector3(x, y, transform.eulerAngles.z);
        var heading1 = -Input.compass.magneticHeading;
        var heading2 = -Input.compass.trueHeading;
        transform.rotation = Quaternion.Euler(0, heading2, 0);
        */
    }
}
