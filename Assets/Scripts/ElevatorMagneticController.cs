using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorMagneticController : MonoBehaviour
{
  
    private Vector3 OrigPos, DestinatPos;
    public float maxHeight, Speed;
    private bool IsPlayerAboveMe, IsMagnetAboveMe;
    private Animation Anim;

   

  
    private void Start()
    {
        OrigPos = transform.position;
        DestinatPos = OrigPos;
        DestinatPos = new Vector3(transform.position.x, transform.position.y + maxHeight, transform.position.z);
        Anim = transform.GetComponent<Animation>();
        
    }

    void Update()
    {
    

        if (Input.GetAxis("RightTrigger") != 0)
        {

            if (IsPlayerAboveMe == true && IsMagnetAboveMe == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, DestinatPos, Time.deltaTime * Input.GetAxis("RightTrigger") * Speed);
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, OrigPos, Time.deltaTime * Speed);
        }       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsPlayerAboveMe = true;
            Anim.Play("ElevActive");

        }
        if (other.CompareTag("Magnet"))
        {
            IsMagnetAboveMe = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsPlayerAboveMe = false;
            Anim.Play("ElevInactive");

        }
        if (other.CompareTag("Magnet"))
        {
            IsMagnetAboveMe = false;
        }
    }
}
