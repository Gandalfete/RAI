using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeletransportationInDungeon : MonoBehaviour
{
    public int TPNumber;
    private Vector3 TPLocation;
    private float Timer;

    private void OnTriggerStay(Collider other)
    {
        Timer += Time.deltaTime;

        if (other.CompareTag("Player"))
        {


            if (TPNumber == 0)
            {
                if (Timer > 0.55f)
                {
                    TPLocation = new Vector3(-980.87f, 2.16f, 82.69f);
                    other.transform.position = TPLocation;
                    Timer = 0;
                }
            }

            if (TPNumber == 1)
            {
                Timer += Time.deltaTime;
                if (Timer > 0.55f)
                {
                    TPLocation = new Vector3(-95, -7.4f, 75.56f);
                    other.transform.position = TPLocation;
                    Timer = 0;
                }                
            }
            if (TPNumber == 2)
            {
                Timer += Time.deltaTime;
                if (Timer > 0.55f)
                {
                    TPLocation = new Vector3(1156.96f, 1.2f, 3.91f);
                    other.transform.position = TPLocation;
                    Timer = 0;
                }
            }
            if (TPNumber == 3)
            {
                Timer += Time.deltaTime;
                if (Timer > 0.55f)
                {
                    TPLocation = new Vector3(20.89f, 14.19f, 42.098f);
                    other.transform.position = TPLocation;
                    Timer = 0;
                }
            }

        }
        
    }




}
