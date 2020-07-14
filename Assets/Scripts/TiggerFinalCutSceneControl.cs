using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiggerFinalCutSceneControl : MonoBehaviour
{
    private PlayerController Player;
    private CanvasFinalCutSceneControl CanvasFC;

    private void Start()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerController>();
        CanvasFC = GameObject.Find("CanvasFinalCutSceneController").GetComponent<CanvasFinalCutSceneControl>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player.JumpDelayAfterPause = 0;
            Player.IsPlayerPausedForMagnetAndCamera = true;
            Player.Anim.enabled = false;

            Player.GM.IsRaiAwake = false;

            CanvasFC.ShowFinalScreen();
        }
    }


}
