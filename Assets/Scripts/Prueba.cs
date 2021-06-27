using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prueba : MonoBehaviour {

    float rotY;
    float rotX;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        rotY = Input.GetAxis("Mouse Y") * 10;
        rotX = Input.GetAxis("Mouse X") * 10;

        transform.Rotate(0, rotX, 0);
        transform.Rotate(-rotY, 0, 0);
    }
}
