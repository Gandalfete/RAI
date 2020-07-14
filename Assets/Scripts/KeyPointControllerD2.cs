using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPointControllerD2 : MonoBehaviour
{
    GameManager GM;
    PlayerController Player;
    private ParticleSystem PS_A, PS_B, PSFinal;
    private Animation KeyAnimA, KeyAnimB, KeyAnimC, DoorAnim;
    private bool IsDoorOpen;


    private void Awake()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        Player = GameObject.Find("Player").GetComponent<PlayerController>();

        KeyAnimA = GameObject.Find("2ndDungDoorIndicator (4)").GetComponent<Animation>();
        KeyAnimB = GameObject.Find("2ndDungDoorIndicator (3)").GetComponent<Animation>();
        KeyAnimC = GameObject.Find("KeyPointButtonC").GetComponent<Animation>();
        DoorAnim = GameObject.Find("Dung2AnimatedDoor").GetComponent<Animation>();
                
        PSFinal = GameObject.Find("ParticleSystemC").GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        KeyAnimA.Play("DoorRhombusOff");
        KeyAnimB.Play("DoorRhombusOff");
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Magnet"))
        {
            if (transform.name == "KeyPointA")
            {
                if (GM.DungeonTwoKeyPointAActive == false)
                {
                    GM.SoundManager.CreateSound(4);
                    GM.DungeonTwoKeyPointAActive = true;
                    KeyAnimA.Play("DoorRhombusOn");
                }
            }
            if (transform.name == "KeyPointB")
            {
                if (GM.DungeonTwoKeyPointBActive == false)
                {
                    GM.SoundManager.CreateSound(4);
                    GM.DungeonTwoKeyPointBActive = true;
                    KeyAnimB.Play("DoorRhombusOn");
                }
            }


            if (transform.name == "KeyPointA" || transform.name == "KeyPointB")
            {
                if (GM.DungeonTwoKeyPointAActive == true && GM.DungeonTwoKeyPointBActive == true)
                {
                    if (IsDoorOpen == false)
                    {
                        DoorAnim.Play("2ndDoorAnimOpen");
                        IsDoorOpen = true;
                    }
                }
                if (GM.DungeonTwoKeyPointAActive == false || GM.DungeonTwoKeyPointBActive == false)
                {
                    if (IsDoorOpen == true)
                    {
                        DoorAnim.Play("2ndDoorAnimClose");
                        IsDoorOpen = false;
                    }
                }
            }


        }
        if (other.CompareTag("Player"))
        {
            if (transform.name == "KeyPointC")
            {
                if (PlayerPrefs.GetInt("LastDangeonCompleted_") < 2)
                {
                    GM.SoundManager.CreateSound(4);
                    GameManager.LastDangeonCompleted = 2;
                    PlayerPrefs.SetInt("LastDangeonCompleted_", GameManager.LastDangeonCompleted);
                    GM.DungeonTwoKeyPointCActive = true;
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

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Magnet"))
        {
            if (transform.name == "KeyPointA")
            {
                GM.DungeonTwoKeyPointAActive = false;
                KeyAnimA.Play("DoorRhombusOff");
            }
            if (transform.name == "KeyPointB")
            {
                GM.DungeonTwoKeyPointBActive = false;
                KeyAnimB.Play("DoorRhombusOff");
            }

            if (transform.name == "KeyPointA" || transform.name == "KeyPointB")
            {
                if (GM.DungeonTwoKeyPointAActive == true && GM.DungeonTwoKeyPointBActive == true)
                {
                    if(IsDoorOpen == false)
                    {
                        DoorAnim.Play("2ndDoorAnimOpen");
                        IsDoorOpen = true;
                    }
                }
                if (GM.DungeonTwoKeyPointAActive == false || GM.DungeonTwoKeyPointBActive == false)
                {
                    if (IsDoorOpen == true)
                    {
                        DoorAnim.Play("2ndDoorAnimClose");
                        IsDoorOpen = false;
                    }
                }
            }

        }
    }
}
