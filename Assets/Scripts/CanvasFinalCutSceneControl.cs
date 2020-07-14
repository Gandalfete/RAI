using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

public class CanvasFinalCutSceneControl : MonoBehaviour
{
    public GameManager GM;

    public bool IsEnglish = false;
    public List<CanvasGroup> Canvas;
    public PostProcessingProfile PPProfile;             // El postprocessing profile almacenado para poder modificar parametros en tiempo real (como el DepthOfField)

    private GameObject ESBackToMainMenuFINAL, ENGBackToMainMenuFINAL;
    private Image ESFinalCutscene, ENGFinalCutscene/*, IconBallImg*/;
    public bool FinalCutSceneFix;

    private float _alphaValue, _inc;

    private void Start()
    {
        IsEnglish = UIController.IsEnglish;
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();

        ESFinalCutscene = GameObject.Find("CanvasGroupFinalCutscene").transform.GetChild(0).GetChild(0).GetComponent<Image>();
        ENGFinalCutscene = GameObject.Find("CanvasGroupFinalCutscene").transform.GetChild(0).GetChild(1).GetComponent<Image>();
        ESBackToMainMenuFINAL = GameObject.Find("CanvasGroupFinalCutscene").transform.GetChild(0).GetChild(2).gameObject;
        ENGBackToMainMenuFINAL = GameObject.Find("CanvasGroupFinalCutscene").transform.GetChild(0).GetChild(3).gameObject;
        //IconBallImg = GameObject.Find("CanvasGroupFinalCutscene").transform.GetChild(0).GetChild(4).GetComponent<Image>();

        PPProfile = Resources.Load<PostProcessingProfile>("PPP InDung");
        PPProfile.depthOfField.enabled = false;

        Canvas[0].alpha = 0;
        Canvas[0].interactable = false;
        Canvas[0].blocksRaycasts = false;
    }

    private void Update()
    {
        if (FinalCutSceneFix == true && Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            if (IsEnglish == false)
            {
                ESBackToMainMenuFINAL.GetComponent<Button>().onClick.Invoke();
            }
            else
            {
                ENGBackToMainMenuFINAL.GetComponent<Button>().onClick.Invoke();
            }
        }
    }

    public void ShowFinalScreen()
    {
        Canvas[0].alpha = 0;
        Canvas[0].interactable = true;
        Canvas[0].blocksRaycasts = true;
        if (IsEnglish == false)
        {
            ESFinalCutscene.enabled = true;
            ESBackToMainMenuFINAL.SetActive(true);
            ENGFinalCutscene.enabled = false;
            ENGBackToMainMenuFINAL.SetActive(false);
        }
        else
        {
            ESFinalCutscene.enabled = false;
            ESBackToMainMenuFINAL.SetActive(false);
            ENGFinalCutscene.enabled = true;
            ENGBackToMainMenuFINAL.SetActive(true);
        }

        _inc = Time.deltaTime;
        _alphaValue = 0;

        GM.SoundManager.CreateSound(4);
        StartCoroutine(FadeInFINALScreen());
    }

    private IEnumerator FadeInFINALScreen()
    {
        yield return new WaitForEndOfFrame();

        _alphaValue += _inc;
        if (_alphaValue > 1)
        {
            _inc = 1;
            PPProfile.depthOfField.enabled = true;
            FinalCutSceneFix = true;
        }
        else
        {
            StartCoroutine(FadeInFINALScreen());
        }

        var tempAlpha = Canvas[0].alpha;
        tempAlpha = _alphaValue; //1f makes it fully visible, 0f makes it fully transparent.
        Canvas[0].alpha = tempAlpha;
    }

    public void FinalBackToMainMenuBtnn()
    {
        GM.SoundManager.CreateSound(0);
        Canvas[0].alpha = 0;
        Canvas[0].interactable = false;
        Canvas[0].blocksRaycasts = false;
        GM.LevelChanger.FadeToLevel(1);
    }
}
