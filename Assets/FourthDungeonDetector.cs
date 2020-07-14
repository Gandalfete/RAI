using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourthDungeonDetector : MonoBehaviour {

    private int BallCounter;
    private List<GameObject> myBalls;
    private Vector3 RespawnPos = new Vector3(0.75f, 10.13f, 457.67f);

	void Update ()
    {
		if (BallCounter > 1)
        {
            if (myBalls.Count > 1)
            {
                for (int i = 0; i < myBalls.Count; i++)
                {
                    myBalls[i].transform.position = RespawnPos;
                }
                myBalls.Clear() ;
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (transform.name == "SteelBallDETECTOR_A")
        {
            if (other.CompareTag("Magnet"))
            {
                myBalls.Add(other.gameObject);
                BallCounter++;
            }
        }
        if (transform.name == "SteelBallDETECTOR_B")
        {
            if (other.CompareTag("Magnet"))
            {
                myBalls.Add(other.gameObject);
                BallCounter++;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Magnet"))
        {
            BallCounter--;
        }

    }
}
