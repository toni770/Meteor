using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//RENICIA LA PARTIDA SI SOLO QUEDA UN JUGADOR. COMPRUEBA EL NUMERO DE JUGADORES ACTIVOS CADA VEZ QUE SE ELIMINA A UNO.
public class ManagerJuego : NetworkBehaviour {

    List<GameObject> jugadoresEliminados;

    // Use this for initialization
    void Start () {
        jugadoresEliminados = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AñadirEliminado(GameObject o)
    {
        if (!isServer) return;

        jugadoresEliminados.Add(o);

        if (SolounJugador())
        {
            StartCoroutine(RestartPartida());
        }
    }

    //Si queda un solo jugador
    bool SolounJugador()
    {
        GameObject[] jugadores = GameObject.FindGameObjectsWithTag("Player");
        return jugadores.Length < 2;
    }

    //Activar jugadores eliminados
    IEnumerator RestartPartida()
    {
        yield return new WaitForSeconds(3f);

        for (int i = 0; i < jugadoresEliminados.Count; i++)
        {
            RpcActivarJugador(jugadoresEliminados[i]);
        }
        jugadoresEliminados.Clear();
    }

    //Activa los jugadores en los clientes
    [ClientRpc]
    void RpcActivarJugador(GameObject o)
    {
        if(o!=null) o.SetActive(true);
    }

}
