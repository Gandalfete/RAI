using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourthDungeonBallDispenser : MonoBehaviour {

    private float Timer_Ball_A, Timer_Ball_B;
    private bool ActiveCounter_A, ActiveCounter_B;
    GameObject myMagnetBall_One, myMagnetBall_Two;
    private Vector3 SpawnPos_One = new Vector3(-9.86f, -1.785f , 475.062f), SpawnPos_Two = new Vector3(10.917f, -1.806f, 437.579f);

    private void Awake()
    {
        myMagnetBall_One = GameObject.Find("SteelBallMagnetA");
        myMagnetBall_Two = GameObject.Find("SteelBallMagnetB");

    }

    private void Update()
    {
        if (ActiveCounter_A == true)
        {
            Timer_Ball_A += Time.deltaTime;
        }
        if (ActiveCounter_B == true)
        {
            Timer_Ball_B += Time.deltaTime;
        }

        if (Timer_Ball_A >= 8)
        {
            myMagnetBall_One.transform.position = SpawnPos_One;
            Timer_Ball_A = 0;
        }
        if (Timer_Ball_B >= 8)
        {
            myMagnetBall_Two.transform.position = SpawnPos_Two;
            Timer_Ball_B = 0;
        }


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Magnet") && other.name == "SteelBallMagnetA")
        {
            ActiveCounter_A = false;
        }
        if (other.CompareTag("Magnet") && other.name == "SteelBallMagnetB")
        {
            ActiveCounter_B = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Magnet") && other.name == "SteelBallMagnetA")
        {
            ActiveCounter_A = true;
        }
        if (other.CompareTag("Magnet") && other.name == "SteelBallMagnetB")
        {
            ActiveCounter_B = true;
        }
    }
}
