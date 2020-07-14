using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTriggersCtrl : MonoBehaviour
{
    public GameObject LevelChanger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.LastDangeonCompleted == 1)
            {
                LevelChanger.GetComponent<LevelChanger>().FadeToLevel(3);
            }
            else if (transform.name == "Trigger1Exit")
            {
                if (GameManager.LastDangeonCompleted == 2)
                {
                    LevelChanger.GetComponent<LevelChanger>().FadeToLevel(5);
                }
                else if (GameManager.LastDangeonCompleted == 3)
                {
                    LevelChanger.GetComponent<LevelChanger>().FadeToLevel(7);
                }
                else if (GameManager.LastDangeonCompleted == 4)
                {
                    LevelChanger.GetComponent<LevelChanger>().FadeToLevel(9);
                }
                else
                {
                    LevelChanger.GetComponent<LevelChanger>().FadeToLevel(10);
                }
            }
            else if (GameManager.LastDangeonCompleted == 2)
            {
                LevelChanger.GetComponent<LevelChanger>().FadeToLevel(5);
            }
            else if (transform.name == "Trigger2Exit")
            {
                if (GameManager.LastDangeonCompleted == 1)
                {
                    LevelChanger.GetComponent<LevelChanger>().FadeToLevel(10);
                }
                else if (GameManager.LastDangeonCompleted == 3)
                {
                    LevelChanger.GetComponent<LevelChanger>().FadeToLevel(7);
                }
                else if (GameManager.LastDangeonCompleted == 4)
                {
                    LevelChanger.GetComponent<LevelChanger>().FadeToLevel(9);
                }
            }
            else if (GameManager.LastDangeonCompleted == 3)
            {
                LevelChanger.GetComponent<LevelChanger>().FadeToLevel(7);
            }
            else if (transform.name == "Trigger3Exit")
            {
                if (GameManager.LastDangeonCompleted == 2)
                {
                    LevelChanger.GetComponent<LevelChanger>().FadeToLevel(5);
                }
                else if (GameManager.LastDangeonCompleted == 4)
                {
                    LevelChanger.GetComponent<LevelChanger>().FadeToLevel(9);
                }
            }
            else if (GameManager.LastDangeonCompleted == 4)
            {
                LevelChanger.GetComponent<LevelChanger>().FadeToLevel(9);
            }
        }
    }
}
