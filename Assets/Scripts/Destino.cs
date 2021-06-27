using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destino : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.position = new Vector3 (Random.Range(-32, 9), 13, Random.Range(-24, 29));
		StartCoroutine (Coldown());

	}
	public IEnumerator Coldown(){
		yield return new WaitForSecondsRealtime (Random.Range(4,10));
		transform.position = new Vector3 (Random.Range(-32, 9), 13, Random.Range(-24, 29));
		StartCoroutine (Coldown());
	}
}
