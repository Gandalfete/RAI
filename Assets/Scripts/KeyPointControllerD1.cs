using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPointControllerD1 : MonoBehaviour
{
    private Animation KeyAnimTwrA, KeyAnimTwrB, KeyAnimA, KeyAnimB, KeyAnimC;
    GameManager GM;
    PlayerController Player;
    private ParticleSystem PS_A, PS_B, PSFinal;

    private void Awake()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        Player = GameObject.Find("Player").GetComponent<PlayerController>();

        KeyAnimTwrA = GameObject.Find("KeyPointTowerA").GetComponent<Animation>();
        KeyAnimTwrB = GameObject.Find("KeyPointTowerB").GetComponent<Animation>();
        KeyAnimA = GameObject.Find("KeyPointButtonA").GetComponent<Animation>();
        KeyAnimB = GameObject.Find("KeyPointButtonB").GetComponent<Animation>();
        KeyAnimC = GameObject.Find("KeyPointButtonC").GetComponent<Animation>();
        PS_A = GameObject.Find("ParticleSystemA").GetComponent<ParticleSystem>();
        PS_B = GameObject.Find("ParticleSystemB").GetComponent<ParticleSystem>();
        PSFinal = GameObject.Find("ParticleSystemC").GetComponent<ParticleSystem>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (transform.name == "KeyPointA")
            {
                if (GM.DungeonOneKeyPointAActive == false)
                {
                    GM.SoundManager.CreateSound(4);
                    GM.DungeonOneKeyPointAActive = true;
                    PS_A.Stop();
                    KeyAnimA.Play();
                    KeyAnimTwrA.Play();
                }
            }
            if (transform.name == "KeyPointB")
            {
                if (GM.DungeonOneKeyPointBActive == false)
                {
                    GM.SoundManager.CreateSound(4);
                    GM.DungeonOneKeyPointBActive = true;
                    PS_B.Stop();
                    KeyAnimB.Play();
                    KeyAnimTwrB.Play();
                }
            }
            if (transform.name == "KeyPointC")
            {
                if (PlayerPrefs.HasKey("IsAnySaved_") == false)
                {
                    GM.SoundManager.CreateSound(4);
                    GameManager.LastDangeonCompleted = 1;
                    PlayerPrefs.SetInt("LastDangeonCompleted_", GameManager.LastDangeonCompleted);
                    GM.DungeonOneKeyPointCActive = true;
                    PSFinal.Stop();
                    KeyAnimC.Play();

                    GM.GlobalPlayerPowerLevel++;
                    Player.ChangePlayerMat();

                    GM.SaveGame(GameManager.LastDangeonCompleted);
                    GM.DeactivateSaveBallIcon();
                }
            }
        }
    }
}
