using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneController : MonoBehaviour
{
    private Camera CamCutScene, CamRai;

    CanvasMainMenuControl CanvasMM;

    private void Start()
    {
        CamCutScene = GameObject.Find("CutsceneCamera").GetComponent<Camera>();
        CamRai = GameObject.Find("Main Camera").GetComponent<Camera>();
        CamCutScene.enabled = true;
        CamRai.enabled = false;
        CanvasMM = GameObject.Find("CanvasMainMenuController").GetComponent<CanvasMainMenuControl>();
    }

    public void AdjustCameras()
    {
        CamCutScene.enabled = false;
        CamRai.enabled = true;
        CanvasMM.AppearMainMenu = true;
    }
}
