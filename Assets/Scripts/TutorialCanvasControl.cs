using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCanvasControl : MonoBehaviour
{
    GameManager GM;

    private Image X_Bttn, Y_Bttn, A_Bttn, B_Bttn, RT_Bttn, LT_Bttn;
    public CanvasGroup TutorialCanvas;
    private float _alphaValue0, _alphaValue1, _alphaValue2, _alphaValue3, _alphaValue4, _inc0, _inc1, _inc2, _inc3, _inc4;
    private bool IsDecreasing1, IsDecreasing2, IsDecreasing3, IsDecreasing4;

    private void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        X_Bttn = GameObject.Find("X-ButtonImg").GetComponent<Image>();
        Y_Bttn = GameObject.Find("Y-ButtonImg").GetComponent<Image>();
        A_Bttn = GameObject.Find("A-ButtonImg").GetComponent<Image>();
        B_Bttn = GameObject.Find("B-ButtonImg").GetComponent<Image>();
        RT_Bttn = GameObject.Find("RT-ButtonImg").GetComponent<Image>();
        LT_Bttn = GameObject.Find("LT-ButtonImg").GetComponent<Image>();
        X_Bttn.enabled = false;
        Y_Bttn.enabled = false;
        A_Bttn.enabled = false;
        B_Bttn.enabled = false;
        RT_Bttn.enabled = false;
        LT_Bttn.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (transform.name == "TriggerTutorialX-Y_D1")
            {
                if (GM.TutorialXYShowed == false)
                {
                    ShowTutorialXYButtons();
                }
            }
            if (transform.name == "TriggerTutorialA_D1")
            {
                if (GM.TutorialAShowed == false)
                {
                    ShowTutorialAButton();
                }
            }
            if (transform.name == "TriggerTutorialRT-Ctrl")
            {
                if (GM.TutorialRTShowed == false)
                {
                    ShowTutorialRTButton();
                }
            }
            if (transform.name == "TriggerTutorialLT-Ctrl")
            {
                if (GM.TutorialLTShowed == false)
                {
                    ShowTutorialLTButton();
                }
            }
            if (transform.name == "TriggerTutorialB-Ctrl")
            {
                if (GM.TutorialBShowed == false)
                {
                    ShowTutorialBButton();
                }
            }
        }
    }

    private void Update()
    {
        if (IsDecreasing1 == true)
        {
            if (Input.GetButtonDown("Fire3") || Input.GetButtonDown("Fire4"))
            {
                GM.TutorialXYShowed = true;
                HideTutorialXYButtons();
            }
        }

        if (IsDecreasing2 == true)
        {
            if (Input.GetAxis("RightTrigger") > 0)
            {
                GM.TutorialRTShowed = true;
                HideTutorialRTButton();
            }
        }

        if (IsDecreasing3 == true)
        {
            if (Input.GetAxis("LeftTrigger") > 0)
            {
                GM.TutorialLTShowed = true;
                HideTutorialLTButton();
            }
        }

        if (IsDecreasing4 == true)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                GM.TutorialBShowed = true;
                HideTutorialBButton();
            }
        }
    }

    public void ShowTutorialXYButtons()
    {
        X_Bttn.enabled = true;
        Y_Bttn.enabled = true;
        _inc0 = Time.deltaTime;
        _alphaValue0 = 0;

        StartCoroutine(TransitionTutorialButtonsXY_1());
    }
    public void HideTutorialXYButtons()
    {
        _inc0 = -Time.deltaTime;
        _alphaValue0 = 1;

        StartCoroutine(TransitionTutorialButtonsXY_2());
    }
    public IEnumerator TransitionTutorialButtonsXY_1()
    {
        yield return new WaitForEndOfFrame();

        _alphaValue0 += _inc0;
        
        if (_alphaValue0 >= 1)
        {
            IsDecreasing1 = true;
        }
        else
        {
            StartCoroutine(TransitionTutorialButtonsXY_1());
        }

        TutorialCanvas.alpha = _alphaValue0;
    }
    public IEnumerator TransitionTutorialButtonsXY_2()
    {
        yield return new WaitForEndOfFrame();

        _alphaValue0 += _inc0;

        if (_alphaValue0 > 0)
        {
            StartCoroutine(TransitionTutorialButtonsXY_2());
        }
        else if (_alphaValue0 < 0)
        {
            //print("Fin de la rutina 2");
            _inc0 = 0;
            X_Bttn.enabled = false;
            Y_Bttn.enabled = false;
            
        }
        else
        {
            StartCoroutine(TransitionTutorialButtonsXY_2());
        }

        TutorialCanvas.alpha = _alphaValue0;
    }



    public void ShowTutorialAButton()
    {
        A_Bttn.enabled = true;
        _inc1 = Time.deltaTime;
        _alphaValue1 = 0;

        StartCoroutine(TransitionTutorialButtonA());
    }
    public IEnumerator TransitionTutorialButtonA()
    {
        yield return new WaitForEndOfFrame();

        _alphaValue1 += _inc1;

        if (_alphaValue1 > 2)
        {
            _inc1 = -Time.deltaTime;
            StartCoroutine(TransitionTutorialButtonA());
        }
        else if (_alphaValue1 < 0)
        {
            //print("Fin de la rutina 2");
            _inc1 = 0;
            A_Bttn.enabled = false;
            GM.TutorialAShowed = true;
        }
        else
        {
            StartCoroutine(TransitionTutorialButtonA());
        }

        TutorialCanvas.alpha = _alphaValue1;
    }



    public void ShowTutorialRTButton()
    {
        RT_Bttn.enabled = true;
        _inc2 = Time.deltaTime;
        _alphaValue2 = 0;

        StartCoroutine(TransitionTutorialButtonRT_1());
    }
    public void HideTutorialRTButton()
    {
        _inc2 = -Time.deltaTime;
        _alphaValue2 = 1;

        StartCoroutine(TransitionTutorialButtonRT_2());
    }
    public IEnumerator TransitionTutorialButtonRT_1()
    {
        yield return new WaitForEndOfFrame();

        _alphaValue2 += _inc2;

        if (_alphaValue2 >= 1)
        {
            IsDecreasing2 = true;
        }
        else
        {
            StartCoroutine(TransitionTutorialButtonRT_1());
        }

        TutorialCanvas.alpha = _alphaValue2;
    }
    public IEnumerator TransitionTutorialButtonRT_2()
    {
        yield return new WaitForEndOfFrame();

        _alphaValue2 += _inc2;

        if (_alphaValue2 > 0)
        {
            StartCoroutine(TransitionTutorialButtonRT_2());
        }
        else if (_alphaValue2 < 0)
        {
            //print("Fin de la rutina");
            _inc2 = 0;
            RT_Bttn.enabled = false;
        }
        else
        {
            StartCoroutine(TransitionTutorialButtonRT_2());
        }

        TutorialCanvas.alpha = _alphaValue2;
    }



    public void ShowTutorialLTButton()
    {
        LT_Bttn.enabled = true;
        _inc3 = Time.deltaTime;
        _alphaValue3 = 0;

        StartCoroutine(TransitionTutorialButtonLT_1());
    }
    public void HideTutorialLTButton()
    {
        _inc3 = -Time.deltaTime;
        _alphaValue3 = 1;

        StartCoroutine(TransitionTutorialButtonLT_2());
    }
    public IEnumerator TransitionTutorialButtonLT_1()
    {
        yield return new WaitForEndOfFrame();

        _alphaValue3 += _inc3;

        if (_alphaValue3 >= 1)
        {
            IsDecreasing3 = true;
        }
        else
        {
            StartCoroutine(TransitionTutorialButtonLT_1());
        }

        TutorialCanvas.alpha = _alphaValue3;
    }
    public IEnumerator TransitionTutorialButtonLT_2()
    {
        yield return new WaitForEndOfFrame();

        _alphaValue3 += _inc3;

        if (_alphaValue3 > 0)
        {
            StartCoroutine(TransitionTutorialButtonLT_2());
        }
        else if (_alphaValue3 < 0)
        {
            //print("Fin de la rutina");
            _inc3 = 0;
            LT_Bttn.enabled = false;
        }
        else
        {
            StartCoroutine(TransitionTutorialButtonLT_2());
        }

        TutorialCanvas.alpha = _alphaValue3;
    }



    public void ShowTutorialBButton()
    {
        B_Bttn.enabled = true;
        _inc4 = Time.deltaTime;
        _alphaValue4 = 0;

        StartCoroutine(TransitionTutorialButtonB_1());
    }
    public void HideTutorialBButton()
    {
        _inc4 = -Time.deltaTime;
        _alphaValue4 = 1;

        StartCoroutine(TransitionTutorialButtonB_2());
    }
    public IEnumerator TransitionTutorialButtonB_1()
    {
        yield return new WaitForEndOfFrame();

        _alphaValue4 += _inc4;

        if (_alphaValue4 >= 1)
        {
            IsDecreasing4 = true;
        }
        else
        {
            StartCoroutine(TransitionTutorialButtonB_1());
        }

        TutorialCanvas.alpha = _alphaValue4;
    }
    public IEnumerator TransitionTutorialButtonB_2()
    {
        yield return new WaitForEndOfFrame();

        _alphaValue4 += _inc4;

        if (_alphaValue4 > 0)
        {
            StartCoroutine(TransitionTutorialButtonB_2());
        }
        else if (_alphaValue4 < 0)
        {
            //print("Fin de la rutina");
            _inc4 = 0;
            B_Bttn.enabled = false;
        }
        else
        {
            StartCoroutine(TransitionTutorialButtonB_2());
        }

        TutorialCanvas.alpha = _alphaValue4;
    }
}
