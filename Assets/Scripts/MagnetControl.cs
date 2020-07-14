using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetControl : MonoBehaviour
{
    //Este script se debe asignar a cada iman, y desde el inspector asignar la cantidad de forceFactor de este dependiendo de la masa del objeto
    // (A NO SER QUE USEIS PREFABS) (DE HECHO USAD SOLO PREFABS MEJOR)

    private GameObject Player;

    public float forceFactor = 10;
    private int MagnetPlayerLevel;
    public bool IsInArea;
    private float currentAttractForce, currentRepelForce;

    private void Awake()
    {
        Player = GameObject.FindWithTag("Player");
        
        
    }



    void Update()
    {
        MagnetPlayerLevel = Player.GetComponent<PlayerController>().PlayerPowerLevel;

        //La fuerza que ejerce cada iman va proporcinal a cuanto aprietes los gatillos del mando
        currentAttractForce = forceFactor * Input.GetAxis("RightTrigger");
        currentRepelForce = -forceFactor * Input.GetAxis("LeftTrigger");

        if (Player.GetComponent<PlayerController>().PlayerAboveMagnet == false) //Si el jugador esta encima de un iman, los imanes se detienen
        {
            if(Player.GetComponent<PlayerController>().IsPlayerPausedForMagnetAndCamera == false) //Comprueba si el juego esta o no pausado a traves del player
            {

                if (MagnetPlayerLevel >= 2) // Si el jugador tiene desbloqueado el nivel 2 de poder (el de Iman de atrac.)
                {
                    if (Input.GetAxis("RightTrigger") > 0 && IsInArea == true) //Al usar el gatillo derecho, si el iman esta en el area de alcance...
                    {
                        GetComponent<Rigidbody>().AddForce((Player.transform.position - transform.position) * currentAttractForce * Time.smoothDeltaTime);
                    }
                }
                if (MagnetPlayerLevel >= 3)
                {
                    if (Input.GetAxis("LeftTrigger") > 0 && IsInArea == true) //Al usar el gatillo izquierdo, si el iman esta en el area de alcance...
                    {
                        GetComponent<Rigidbody>().AddForce((Player.transform.position - transform.position) * currentRepelForce * Time.smoothDeltaTime);
                    }
                }
            }

        }

        
    }
}
