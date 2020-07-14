using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointControl : MonoBehaviour
{
    // Este script se asigna a cada checkpoint (o no, si se usa el prefab). Se encarga de detectar al player en el checkpoint y comunicarle
    // al GM sus coordenadas


    GameManager GM;

    private void Awake()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GM.PlayerCurrentCheckpoint = other.transform.position;
        }
    }
}
