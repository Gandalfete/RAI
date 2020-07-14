using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetArea : MonoBehaviour
{

    //Este script solo se encarga de registrar en una lista todos los imanes que esten en el area del player
    // y le dice al MagnetControl si cada objeto esta o no en el area de atraccion

    public List<GameObject> MagneticObjectsInArea;



    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Magnet"))
        {
            MagneticObjectsInArea.Add(other.gameObject);
            other.GetComponent<MagnetControl>().IsInArea = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Magnet"))
        {
            other.GetComponent<MagnetControl>().IsInArea = false;
            MagneticObjectsInArea.Remove(other.gameObject);
        }
    }
}
