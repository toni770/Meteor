using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

//CONTROLA EL GIROSCOPIO
public class Gyro : MonoBehaviour
{

    private bool permiteGiro = false; //Comprueba que el mobil soporte giroscopio
    private Gyroscope giro;
    private GameObject contenedorDeCamara; //Objeto vacio donde sera hijo la camara. Necesario para que no la lie mucho
    private Quaternion rotacion;


    // Use this for initialization
    void Start()
    {
        contenedorDeCamara = new GameObject("Contenedor de camara");
        contenedorDeCamara.transform.position = this.transform.position;
        contenedorDeCamara.transform.SetParent(transform.parent);
        transform.SetParent(contenedorDeCamara.transform);

        permiteGiro = ActivarGiro();


    }

    // Update is called once per frame
    void Update()
    {
        if (permiteGiro) { transform.localRotation = giro.attitude * rotacion; }
    }

    private bool ActivarGiro()
    {
        //Activa el giroscopio si lo soporta
        if (SystemInfo.supportsGyroscope)
        {
        giro = Input.gyro;
        giro.enabled = true;
        contenedorDeCamara.transform.rotation = Quaternion.Euler(90f, 90f, 0);
            rotacion = new Quaternion(0, 0, 1, 0);
            return true;
        }

        return false;
    }




}
