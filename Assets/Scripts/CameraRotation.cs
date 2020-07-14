using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraRotation : MonoBehaviour
{
    //Script de la orbita de la camara (ADVERTENCIA: TERRENO MUY DELICADO. RECOMENDADO DEJARLO COMO ESTA)

    public GameObject targetPlayer;
    private float input;
    private Vector3 targetPlayerPos;
    public float orbitDistance = 5f;
    private Vector3 height;
    public float CameraHeight = 2f;
    private float CameraSpeed = 240;
    private float MouseMultiplier = 3400;




    void Start()
    {

    }


    void Update()
    {

        // Inputs para llevar la orbita a cabo

        //input += Input.GetAxis("CamHoriz") * Time.deltaTime * 90;
        if (targetPlayer.GetComponent<PlayerController>().IsPlayerPausedForMagnetAndCamera == false)
        {
           
            input += Input.GetAxis("CamHoriz") * Time.deltaTime * 150;
            
            if(Input.GetAxis("Mouse X") != 0)
            {
                 input += Input.GetAxis("Mouse X") * Time.deltaTime * MouseMultiplier;
            }
     
        }
        else
        {
            input = 0;
        }


        //Esta cadena de 'if's permite mover la camara con joystick derecho y suaviza la orbita de esta
        if (input > 90 )
        {
            input = 90;
            
        }
        if (input < -90)
        {
            input = -90;
        }
        if (input > 0 && Input.GetAxis("CamHoriz") == 0)
        {
            //input -= Time.deltaTime * 60;
            input -= Time.deltaTime * CameraSpeed;

        }
        if (input < 0 && Input.GetAxis("CamHoriz") == 0)
        {
            //input += Time.deltaTime * 60;
            input += Time.deltaTime * CameraSpeed;
        }
        if (input < 5 && input > -5 && Input.GetAxis("CamHoriz") == 0)
        {
            input = 0;
        }

        // Orbita del GameObject vacio alrededor del player. Este GameObject es padre de la camara en la escena.    

        transform.position = targetPlayer.transform.position + (transform.position - targetPlayer.transform.position).normalized * orbitDistance;
        height = new Vector3(transform.position.x, targetPlayer.transform.position.y + CameraHeight, transform.position.z);
        transform.position = height;

        transform.LookAt(targetPlayer.transform);
        transform.RotateAround(targetPlayer.transform.position, targetPlayer.transform.up, input * Time.deltaTime);

        //Ocurren muchas cosas matematicas y se me ha olvidado que hace la mitad, por ello es mejor no tocar este script que por ahora va perfecto
    }
}
