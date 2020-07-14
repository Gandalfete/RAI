using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetBallDispenserController : MonoBehaviour
{
    private float Timer;
    private bool ActiveCounter;
    GameObject myMagnetBall;
    private Vector3 SpawnPos = new Vector3(-95.15f, 36.78f, 44.47f);

    private void Awake()
    {
        myMagnetBall = GameObject.Find("SteelBallMagnetA");
    }

    private void Update()
    {
        if (ActiveCounter == true)
        {
            Timer += Time.deltaTime;
        }

        if (Timer >= 8)
        {
            myMagnetBall.transform.position = SpawnPos;
            Timer = 0;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Magnet") && other.name == "SteelBallMagnetA")
        {
            ActiveCounter = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Magnet") && other.name == "SteelBallMagnetA")
        {
            ActiveCounter = true;
        }
    }
}
