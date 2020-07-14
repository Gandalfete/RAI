using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    //Este script se debe asignar a cada plataforma que sea dimensional y desde fuera decir si es azul o naranja (A NO SER QUE USEIS PREFABS)

    // De hecho olvidad el comentario de arriba, usemos solo prefabs para ahorrar problemas


    public bool imOrange;
    public bool imBlue;

    public bool imAnimated;

    public Material InactiveMat, ActiveMat;
    public GameManager GM;
    PlayerController Player;
    private Animator Anim;


    private void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        Player = GameObject.Find("Player").GetComponent<PlayerController>();
        Anim = GetComponent<Animator>();

    }

    void Update()
    {

        CheckMyDimensions();
        if (imAnimated == true)
        {
            CheckPause();
        }
    }


    void CheckMyDimensions()
    {
        if (imOrange == true)
        {
            if (GM.BlueDimensionActive == true)
            {
                GetComponent<BoxCollider>().enabled = false;
                GetComponent<Renderer>().material = InactiveMat;
            }
            if (GM.OrangeDimensionActive == false)
            {
                GetComponent<BoxCollider>().enabled = false;
                GetComponent<Renderer>().material = InactiveMat;
            }
            if (GM.OrangeDimensionActive == true)
            {
                GetComponent<BoxCollider>().enabled = true;
                GetComponent<Renderer>().material = ActiveMat;
            }
        }
        if (imBlue == true)
        {
            if (GM.OrangeDimensionActive == true)
            {
                GetComponent<BoxCollider>().enabled = false;
                GetComponent<Renderer>().material = InactiveMat;
            }
            if (GM.BlueDimensionActive == false)
            {
                GetComponent<BoxCollider>().enabled = false;
                GetComponent<Renderer>().material = InactiveMat;
            }
            if (GM.BlueDimensionActive == true)
            {
                GetComponent<BoxCollider>().enabled = true;
                GetComponent<Renderer>().material = ActiveMat;
            }
        }
    }

    void CheckPause()
    {
        if (Player.IsPlayerPausedForMagnetAndCamera == true)
        {
            Anim.enabled = false;
        }
        else
        {
            Anim.enabled = true;

        }

    }


}
