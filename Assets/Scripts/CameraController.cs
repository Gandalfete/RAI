using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    //Script de la camara (ADVERTENCIA: TERRENO MUY MUY DELICADO. RECOMENDADO DEJARLO COMO ESTA)

    public float FollowSpeed, DistZ, Height;
    public GameObject Target, TempRot;

    public float RotationSpeed;

    void Update()
    {
        if (!Target)
            return;

        //Movimiento de seguir al player original de la cámara

        /*transform.position = Vector3.Lerp(transform.position,
            new Vector3(Target.transform.position.x, Target.transform.position.y + Height, Target.transform.position.z - DistZ),
            FollowSpeed * Time.deltaTime);*/

        //Giro original de la cámara

        TempRot.transform.LookAt(Target.transform);
        transform.rotation = Quaternion.Lerp(transform.rotation, TempRot.transform.rotation, RotationSpeed * Time.deltaTime);
    }
}
