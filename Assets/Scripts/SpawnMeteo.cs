using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//CONTROLA EL SPAWN DEL METEORITO.
public class SpawnMeteo : NetworkBehaviour {

    public GameObject meteorPrefab;
    GameObject meteor;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Se activa con el boton del server
    public void Spawnear()
    {
        meteor = Instantiate(meteorPrefab, transform.position, transform.rotation);
        NetworkServer.Spawn(meteor);
    }
}
