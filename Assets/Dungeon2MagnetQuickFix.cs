using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon2MagnetQuickFix : MonoBehaviour {

    private Vector3 OrigPos, QuickFixPos, QuickFixPos2;
    private float myCounter;

	void Start () {
        OrigPos = transform.position;
        QuickFixPos = new Vector3(OrigPos.x, OrigPos.y, 58.62f);
        QuickFixPos2 = new Vector3(-7.27f, 1.84f, 5.23f);
        transform.position = QuickFixPos;
        
	}
	
	void Update () {
        myCounter += Time.deltaTime;
        if(myCounter > 0.1f && myCounter < 0.3f)
        {
            transform.GetComponent<MeshRenderer>().enabled = false;
            transform.position = QuickFixPos2;

        }
        if (myCounter > 0.3f && myCounter < 0.5f)
        {

            transform.position = OrigPos;
            transform.GetComponent<MeshRenderer>().enabled = true;

        }

    }
}
