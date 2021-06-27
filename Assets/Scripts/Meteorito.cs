using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//CONTROLA EL METEORITO.
public class Meteorito : NetworkBehaviour {

    [SyncVar]
    public float speed = 4;

	GameObject[] targets;
    [SyncVar]
	GameObject target;

	string colorActual;

	int Rand;

    public float maxSpeed = 20;

    [Tooltip("Cuanto mas grande, mas lento irá")]
    public float velocidadGiro = 6;

    [Tooltip("Tiempo que tarda la pelota en buscar a una victima desde que es golpeado")]
    public float tiempoBuscar = 0.5f;

    [SyncVar]
    Vector3 direction;

    ManagerJuego gm;


    //public Spawn Sp;

	void Start () {
        if(isServer)
        {
            //Sp = GameObject.FindGameObjectWithTag("Respawn").GetComponent<Spawn>();

            targets = GameObject.FindGameObjectsWithTag("Player");
            //target = targets[Random.Range(0, targets.Length)];
            target = targets[Random.Range(0,targets.Length)];

            direction = (target.transform.position - this.transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
            RpcRotarCliente(gameObject, direction);

            gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ManagerJuego>();

            ChangeColor();
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
            //Cuando encuentra un objetivo, rota hacia el a su ritmo, haciendo que vaya con efecto.
            direction = (target.transform.position - this.transform.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed / velocidadGiro * Time.deltaTime);
        }

        //Siempre se mueve en direccion al eje de las X
        this.transform.position += transform.forward * speed * Time.deltaTime;

    }

	void OnTriggerEnter (Collider other){
        //Si es golpeado con el color correcto, va en direccion a la que apunta el jugador
        if (!isServer) return;

        //Si golpean el meteorito
        if (other.gameObject.tag == colorActual)
        {
            ChangeColor();

            if (speed <= maxSpeed)
            {
                speed = speed + 5;
            }

            target = null;

            transform.rotation = Quaternion.LookRotation(other.transform.forward);
            RpcRotarCliente(gameObject, other.transform.forward);

            StartCoroutine(BuscarVictima(other.transform.parent.transform.parent.gameObject));
        }

        //Si golpea a un jugador, elimina al jugador y al meteorito
		if (other.gameObject.tag == "Player")
        {
            //Sp.Spawnit = true;
            RpcDesactivar(other.gameObject);
            other.gameObject.SetActive(false);
            gm.AñadirEliminado(other.gameObject);
            NetworkServer.Destroy(gameObject);
        }
	}

    //Rota el meteorito en los clietnes
    [ClientRpc]
    void RpcRotarCliente(GameObject meteo, Vector3 target)
    {
        meteo.transform.rotation = Quaternion.LookRotation(target);
    }

    //Desactivar los jugadores en los clietnes
    [ClientRpc]
    void RpcDesactivar(GameObject o)
    {
        o.SetActive(false);
    }

    //Funciones para cambiar el color del meteorito en todos los jugadores.
	void ChangeColor(){
        //Cambia a un color aleatoriamente
        Rand = Random.Range(1, 4);
        switch (Rand)
        {
            case 1:
                colorActual = ("red");
                break;
            case 2:
                colorActual = ("yellow");
                break;
            case 3:
                colorActual = ("blue");
                break;
        }
        RpcCambiarColor(gameObject, Rand);
    }

    [ClientRpc]
    void RpcCambiarColor(GameObject meteor, int col)
    {
        switch (col)
        {
            case 1:
                meteor.GetComponentInChildren<Renderer>().material.color = Color.red;
                break;
            case 2:
                meteor.GetComponentInChildren<Renderer>().material.color = Color.yellow;
                break;
            case 3:
                meteor.GetComponentInChildren<Renderer>().material.color = Color.blue;
                break;
        }
    }

    //FUNCIONES PARA BUSCAR OBJETIVO

    GameObject jugadorMasCercano(GameObject golpeador)
    {
        //Devuelve el jugador mas cercano a la pelota.
        GameObject target2 = null;

        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] != golpeador)
            {
                if (target2 == null)
                {
                    target2 = targets[i];
                }
                else
                {
                    if (Vector3.Distance(targets[i].transform.position, transform.position) < Vector3.Distance(target2.transform.position, transform.position))
                    {
                        target2 = targets[i];
                    }
                }
            }
        }
        return target2;
    }

    //Busca al jugador mas cercano tras cierto tiempo
    IEnumerator BuscarVictima(GameObject golpeador)
    {
        yield return new WaitForSeconds(tiempoBuscar);
        target = jugadorMasCercano(golpeador);
    }
}
