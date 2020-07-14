using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondDungKeyPointControl : MonoBehaviour {

    public GameObject RhombusA, RhombusB, Door;
    public Animation Anim;



    private void Start()
    {
        if (transform.name == "Dung2Door")
        {
            Anim = GetComponent<Animation>();
        }
    }

    void Update () {
		
	}
}
