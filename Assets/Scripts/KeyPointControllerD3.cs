using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPointControllerD3 : MonoBehaviour
{
    private Animation KeyAnimA, KeyAnimB, KeyAnimC;
    GameManager GM;
    PlayerController Player;
    private ParticleSystem PSFinal;
    private GameObject UnlockableGround1, UnlockableGround2;


    private void Awake()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        Player = GameObject.Find("Player").GetComponent<PlayerController>();

        KeyAnimA = GameObject.Find("KeyPointButtonA").GetComponent<Animation>();
        UnlockableGround1 = GameObject.Find("UnlockableGround1");
        KeyAnimB = GameObject.Find("KeyPointButtonB").GetComponent<Animation>();
        UnlockableGround2 = GameObject.Find("UnlockableGround2");
        KeyAnimC = GameObject.Find("KeyPointButtonC").GetComponent<Animation>();
        PSFinal = GameObject.Find("ParticleSystemC").GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        UnlockableGround1.SetActive(false);
        UnlockableGround2.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Magnet"))
        {
            if (transform.name == "KeyPointA" && other.transform.name == "SteelBallMagnetA")
            {
                if (GM.DungeonThreeKeyPointAActive == false)
                {
                    GM.SoundManager.CreateSound(4);
                    GM.DungeonThreeKeyPointAActive = true;
                    KeyAnimA.Play();
                    UnlockableGround1.SetActive(true);
                }
            }
            if (transform.name == "KeyPointB" && other.transform.name == "SteelBallMagnetB")
            {
                if (GM.DungeonThreeKeyPointBActive == false)
                {
                    GM.SoundManager.CreateSound(4);
                    GM.DungeonThreeKeyPointBActive = true;
                    KeyAnimB.Play();
                    UnlockableGround2.SetActive(true);
                }
            }
        }
        if (other.CompareTag("Player"))
        {
            if (transform.name == "KeyPointC")
            {
                if (PlayerPrefs.GetInt("LastDangeonCompleted_") < 3)
                {
                    GM.SoundManager.CreateSound(4);
                    GameManager.LastDangeonCompleted = 3;
                    PlayerPrefs.SetInt("LastDangeonCompleted_", GameManager.LastDangeonCompleted);
                    GM.DungeonThreeKeyPointCActive = true;
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
