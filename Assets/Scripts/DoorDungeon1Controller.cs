using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDungeon1Controller : MonoBehaviour {

    GameManager GM;
    bool ActivatedDoorA, ActivatedDoorB;
    private Material MyMat;
    private Animation MyAnim;
    private Color MyOpenColor = new Color(1.3f,1.3f,1.3f);
    

    private void Awake()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        MyMat = transform.GetComponent<MeshRenderer>().material;
        MyMat.DisableKeyword("_EMISSION");
        MyAnim = transform.GetComponent<Animation>();

    }

    void Update () {

        if (GM.DungeonOneKeyPointAActive == true)
        {
            ActivatedDoorA = true;
        }
        if (GM.DungeonOneKeyPointBActive == true)
        {
            ActivatedDoorB = true;
        }


        if (ActivatedDoorA == true)
        {
            if (transform.name == "DoorA")
            {                
                MyMat.EnableKeyword("_EMISSION");
                MyAnim.Play("DoorA_Test");
            }
        }
        //else
        //{
        //    if (transform.name == "DoorA")
        //    {
        //        MyMat.SetColor("_EmissionColor", Color.black);
        //    }
        //}

        if (ActivatedDoorB == true)
        {
            if (transform.name == "DoorB")
            {
                MyMat.EnableKeyword("_EMISSION");
                if(ActivatedDoorA == false)
                    MyAnim.Play("DoorA_Test");
            }
        }
        //else
        //{
        //    if (transform.name == "DoorB")
        //    {
        //        MyMat.SetColor("_EmissionColor", Color.black);
        //    }
        //}

        if (ActivatedDoorA == true && ActivatedDoorB == true)
        {
            if (transform.name == "DoorA")
            {
                transform.position = new Vector3(-22.34f, 3.5f, 49.03f);
                MyAnim.Play("DoorB_Test");
            }
            if (transform.name == "DoorB")
            {
                transform.position = new Vector3(-14f, 3.51f, 49.07f);
                MyAnim.Play("DoorB_Test");
            }
        }
    }
}
