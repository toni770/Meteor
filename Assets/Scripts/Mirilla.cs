using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mirilla : MonoBehaviour {

    public Text mir;
	// Use this for initialization
	void OnEnable () {
        mir.text = "X";
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        mir.text = "O";
    }

    private void OnTriggerExit(Collider other)
    {
        mir.text = "X";
    }
}
