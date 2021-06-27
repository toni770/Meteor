using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//CONTROLA EL DISPARAR Y LAS ANIMACIONES DE CADA JUGADOR
public class PlayerNetwork : NetworkBehaviour
{

    Animator alienAnimador;
    Transform alienTransform;

    public Transform Camara;
    public BoxCollider camaraBox;
    public GameObject alien;

    public Renderer rendAlien;
    public Renderer rendOvni;

    // Use this for initialization
    private void Start()
    {
        if (isLocalPlayer)
        {
            rendAlien.enabled = false;
            rendOvni.enabled = false;
        }
        alienAnimador = alien.GetComponent<Animator>();
        alienTransform = alien.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Si no es el jugador local, elimina los scripts y la camara
        if (!isLocalPlayer)
        {
            GetComponentInChildren<Camera>().enabled = false;
            GetComponentInChildren<Gyro>().enabled = false;
            GetComponentInChildren<Canvas>().enabled = false;
            return;
        }

        alienTransform.rotation = Quaternion.Euler(0, Camara.transform.rotation.eulerAngles.y, 0);
    }

    //Al pulsar el boton de un color
    public void ButtonPress(string color)
    {
        CmdActivarColl(color);
    }

    //Activa la animacion y el box durante un tiempo
    public IEnumerator Disparar(string color)
    {
        Camara.tag = color;

        alienAnimador.SetBool("Shot", true);
        camaraBox.enabled = true;
        yield return new WaitForSeconds(0.2f);
        camaraBox.enabled = false;
        alienAnimador.SetBool("Shot", false);
    }

    //Activa el coll y la animacion del personaje en todos los jugadores
    [Command]
    void CmdActivarColl(string color)
    {
        RpcActivarColl(color);
    }

    [ClientRpc]
    void RpcActivarColl(string color)
    {
        StartCoroutine(Disparar(color));
    }


}
