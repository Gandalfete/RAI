using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceToDung1Control : MonoBehaviour
{
    private Animation KeyAnimC, Dung1Door;
    GameManager GM;
    PlayerController Player;
    private ParticleSystem PS_A;

    
    private void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        Player = GameObject.Find("Player").GetComponent<PlayerController>();
        Dung1Door = GameObject.Find("Dung1Door").GetComponent<Animation>();

        KeyAnimC = GameObject.Find("KeyPointButtonC").GetComponent<Animation>();
        PS_A = GameObject.Find("ParticleSystemC").GetComponent<ParticleSystem>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (transform.name == "KeyPointC")
            {
                if (GM.EntranceDung1Open == false)
                {
                    GM.SoundManager.CreateSound(4);
                    GM.GlobalPlayerPowerLevel = 1;
                    Player.ChangePlayerMat();
                    PS_A.Stop();
                    KeyAnimC.Play();
                    Dung1Door.Play();
                    GM.EntranceDung1Open = true;
                }
            }
        }
    }
}
