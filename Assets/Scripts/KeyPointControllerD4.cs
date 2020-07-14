using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPointControllerD4 : MonoBehaviour
{
    private Animation KeyAnimC;
    GameManager GM;
    PlayerController Player;
    private ParticleSystem PSFinal;


    private void Awake()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        Player = GameObject.Find("Player").GetComponent<PlayerController>();

        KeyAnimC = GameObject.Find("KeyPointButtonC").GetComponent<Animation>();
        PSFinal = GameObject.Find("ParticleSystemC").GetComponent<ParticleSystem>();
    }
   

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (transform.name == "KeyPointC")
            {
                if (PlayerPrefs.GetInt("LastDangeonCompleted_") < 4)
                {
                    GM.SoundManager.CreateSound(4);
                    GameManager.LastDangeonCompleted = 4;
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
