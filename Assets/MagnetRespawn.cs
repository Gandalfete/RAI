using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetRespawn : MonoBehaviour {

    private Vector3 OrigPos;

	void Start () {

        OrigPos = transform.position;
	}
	
	void Update () {
		if (transform.position.y < -60)
        {
            transform.position = OrigPos;
        }
	}
}
