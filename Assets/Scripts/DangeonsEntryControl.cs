using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangeonsEntryControl : MonoBehaviour
{
    public GameObject LevelChanger;
    public int SceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelChanger.GetComponent<LevelChanger>().FadeToLevel(SceneToLoad);
        }
    }
}
