using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public List<CanvasGroup> Canvas;
    
    private float _alphaValue0, _alphaValue1, _alphaValue2, _inc0, _inc1, _inc2;
    private float StartingFadeInAlpha;
    //paneles del CanvasGroupStart
    [HideInInspector] public Image StartBkgrd, StartBlackWindow;
    private bool IsStartCanvas;
    //paneles del CanvasGroupInitial
    [HideInInspector] public Image ImageBckgdES, InitText1ES, InitText2ES, BlackWindowES;
    [HideInInspector] public Image ImageBckgdENG, InitText1ENG, InitText2ENG, BlackWindowENG;
    //Variables para mover icono bolita
    [HideInInspector] public List<GameObject> BallIconPoints;
    [HideInInspector] public List<Button> LngListButtons;
    [HideInInspector] public GameObject BallIcon;
    [HideInInspector] public List<Sprite> BallIconImages;
    private float speedBall = 10;
    private int pointID;

    [HideInInspector] public static bool IsEnglish = false;

    private int CrossPadUp, CrossPadDown;               // contador para controlar el CrossPad del gamepad para controlar los desplazamientos de los menús
    private bool CanUseCrosspad = true;                 //bool para habilitar el aumento o disminución del pointID y por tanto mover la bola blanca

    private LevelChanger LevelChanger;
    private int LastDangeonCompleted;

    private SoundController SoundManager;

    void Start()
    {
        PlayerPrefs.DeleteAll();
        SoundManager = GameObject.Find("SoundController").GetComponent<SoundController>();
        LevelChanger = GameObject.Find("LevelChanger").GetComponent<LevelChanger>();

        if (PlayerPrefs.HasKey("IsAnySaved_") == true)
        {
            LastDangeonCompleted = PlayerPrefs.GetInt("LastDangeonCompleted_");
        }

        _alphaValue0 = 0; //controla el fadeIn y fadeOut de la BlackWindow que lleva al Init1 (jugar con mando)
        _alphaValue1 = 0; //controla el fadeIn y fadeOut del Init1 (jugar con mando)
        _alphaValue2 = 0; //controla el fadeIn y fadeOut del Init2 (info guardado)

        Canvas[0].alpha = 1;
        Canvas[0].interactable = true;
        Canvas[0].blocksRaycasts = true;

        StartCoroutine(FadeInStart(1, 0));
    }

    void Update()
    {
        if (IsStartCanvas == true) //Menú selección de idioma inicial
        {
            LerpToMoveIcon();

            if (CanUseCrosspad == true)
            {
                if (Input.GetAxisRaw("CrossPad") == 1) //Con GetAxisRaw convertimos cualquier graduación positiva en un 1
                {
                    if (CrossPadUp == 0) //según entra lo pongo a cero para que solo avance 1 fotograma y solo sume/reste 1.
                    {
                        pointID--;
                        if (pointID < 0)
                        {
                            pointID = 0;
                        }
                        CrossPadDown = 0;
                        CrossPadUp++;
                    }
                }
                else if (Input.GetAxisRaw("CrossPad") == -1) //Con GetAxisRaw convertimos cualquier graduación negativa en un -1
                {
                    if (CrossPadDown == 0) //según entra lo pongo a cero para que solo avance 1 fotograma y solo sume/reste 1.
                    {
                        pointID++;
                        if (pointID > 1)
                        {
                            pointID = 1;
                        }
                        CrossPadUp = 0;
                        CrossPadDown++;
                    }
                }
                else
                {
                    CrossPadUp = 0;
                    CrossPadDown = 0;
                }
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                SoundManager.CreateSound(0);
                LngListButtons[pointID].GetComponent<Button>().onClick.Invoke(); //busco el número de botón de la lista en base al pointID donde está la bola blanca
                                                                                  //y ejecuto su función correspondiente del OnClick()
            }
        }
    }

    #region UI_ButtonsFuntions

    public void FirstScreenBttn1() //Español
    {
        SoundManager.CreateSound(0);
        IsEnglish = false;
        CanUseCrosspad = false;
        _inc0 = Time.deltaTime;
        _alphaValue0 = 0;

        StartCoroutine(LanguageToInit1Corrut());
    }

    public void FirstScreenBttn2() //Inglés
    {
        SoundManager.CreateSound(0);
        IsEnglish = true;
        CanUseCrosspad = false;
        _inc0 = Time.deltaTime;
        _alphaValue0 = 0;

        StartCoroutine(LanguageToInit1Corrut());
    }

    public void ShowInit1()
    {
        _inc1 = Time.deltaTime;
        _alphaValue1 = 0;

        StartCoroutine(ShowInit1Corrut());
    }

    public void ShowInit2()
    {
        _inc2 = Time.deltaTime;
        _alphaValue2 = 0;

        StartCoroutine(ShowInit2Corrut());
    }

    #endregion

    #region IconMovement_Funtions

    public void LerpToMoveIcon()
    {
        BallIcon.transform.position = Vector3.Lerp(BallIcon.transform.position, BallIconPoints[pointID].transform.position, speedBall * Time.deltaTime);
    }

    public void OnSpanishButton()
    {
        pointID = 0;
    }
    public void OnEnghlisButton()
    {
        pointID = 1;
    }

    #endregion

    #region UI_Coroutines

    private IEnumerator FadeInStart(float startAlpha, float endAlpha)
    {
        IsStartCanvas = true;
        CanUseCrosspad = true;
        float elapsedTime = 0.0f;

        while (elapsedTime < 2f)
        {
            elapsedTime += Time.deltaTime;
            StartingFadeInAlpha = Mathf.Lerp(startAlpha, endAlpha, Mathf.Clamp01(elapsedTime / 2f));

            var tempColor = StartBlackWindow.color;
            tempColor.a = StartingFadeInAlpha; //1f makes it fully visible, 0f makes it fully transparent.
            StartBlackWindow.color = tempColor;

            yield return new WaitForEndOfFrame();
        }
        //print("Fin de rutina StartFadeIn");
    }
    private IEnumerator LanguageToInit1Corrut()
    {
        yield return new WaitForEndOfFrame();

        _alphaValue0 += _inc0;
        if (_alphaValue0 > 1)
        {
            IsStartCanvas = false;
            _inc0 = -Time.deltaTime;
            //oculto el CanvasGroupStart
            Canvas[0].alpha = 0;
            Canvas[0].interactable = false;
            Canvas[0].blocksRaycasts = false;
                //BallIcon.SetActive(false);
            //Muestro el CanvasGroupInitial
            Canvas[1].alpha = 1;
            Canvas[1].interactable = true;
            Canvas[1].blocksRaycasts = true;
            if (IsEnglish == false)
            {
                //Muestro el CanvasGroupInitialES
                Canvas[2].alpha = 1;
                Canvas[2].interactable = true;
                Canvas[2].blocksRaycasts = true;
            }
            else
            {
                //Muestro el CanvasGroupInitialENG
                Canvas[3].alpha = 1;
                Canvas[3].interactable = true;
                Canvas[3].blocksRaycasts = true;
            }

            StartCoroutine(LanguageToInit1Corrut());
        }
        else if (_alphaValue0 < 0)
        {
            //print("Fin de la rutina 1");
            _inc0 = 0;
            ShowInit1();
        }
        else
        {
            StartCoroutine(LanguageToInit1Corrut());
        }

        if (IsEnglish == false)
        {
            if (IsStartCanvas == true)
            {
                //Manejo el FadeOut del BlackWindow inicial
                var tempColor = StartBlackWindow.color;
                tempColor.a = _alphaValue0; //1f makes it fully visible, 0f makes it fully transparent.
                StartBlackWindow.color = tempColor;
            }
            else
            {
                //Manejo el FadeIn español
                var tempColor = BlackWindowES.color;
                tempColor.a = _alphaValue0; //1f makes it fully visible, 0f makes it fully transparent.
                BlackWindowES.color = tempColor;
            }
        }
        else
        {
            if (IsStartCanvas == true)
            {
                //Manejo el FadeOut del BlackWindow inicial
                var tempColor = StartBlackWindow.color;
                tempColor.a = _alphaValue0; //1f makes it fully visible, 0f makes it fully transparent.
                StartBlackWindow.color = tempColor;
            }
            else
            {
                //Manejo el FadeIn inglés
                var tempColor = BlackWindowENG.color;
                tempColor.a = _alphaValue0; //1f makes it fully visible, 0f makes it fully transparent.
                BlackWindowENG.color = tempColor;
            }
        }
    }
    private IEnumerator ShowInit1Corrut()
    {
        yield return new WaitForEndOfFrame();

        _alphaValue1 += _inc1;
        if (_alphaValue1 > 4)
        {
            _inc1 = -Time.deltaTime;
            StartCoroutine(ShowInit1Corrut());
        }
        else if (_alphaValue1 < 0)
        {
            //print("Fin de la rutina 2");
            _inc1 = 0;
            ShowInit2();
        }
        else
        {
            StartCoroutine(ShowInit1Corrut());
        }

        if (IsEnglish == false)
        {
            var tempColor = InitText1ES.color;
            tempColor.a = _alphaValue1; //1f makes it fully visible, 0f makes it fully transparent.
            InitText1ES.color = tempColor;
        }
        else
        {
            var tempColor = InitText1ENG.color;
            tempColor.a = _alphaValue1; //1f makes it fully visible, 0f makes it fully transparent.
            InitText1ENG.color = tempColor;
        }
    }
    private IEnumerator ShowInit2Corrut()
    {
        yield return new WaitForEndOfFrame();

        _alphaValue2 += _inc2;
        if (_alphaValue2 > 4)
        {
            _inc2 = -Time.deltaTime;
            StartCoroutine(ShowInit2Corrut());
        }
        else if (_alphaValue2 < 0)
        {
            //print("Fin de la rutina 3");
            _inc2 = 0;
            if (PlayerPrefs.HasKey("IsAnySaved_") == true) //si encuentra datos en el PlayerPrefs, carga esa escena guardada
            {
                if (PlayerPrefs.GetInt("LastDangeonCompleted_") == 1)
                {
                    LevelChanger.FadeToLevel(3);
                }
                else if (PlayerPrefs.GetInt("LastDangeonCompleted_") == 2)
                {
                    LevelChanger.FadeToLevel(5);
                }
                else if (PlayerPrefs.GetInt("LastDangeonCompleted_") == 3)
                {
                    LevelChanger.FadeToLevel(7);
                }
                else if (PlayerPrefs.GetInt("LastDangeonCompleted_") == 4)
                {
                    LevelChanger.FadeToLevel(9);
                }
            }
            else
            {
                LevelChanger.FadeToLevel(1);
            }
        }
        else
        {
            StartCoroutine(ShowInit2Corrut());
        }

        if (IsEnglish == false)
        {
            var tempColor = InitText2ES.color;
            tempColor.a = _alphaValue2; //1f makes it fully visible, 0f makes it fully transparent.
            InitText2ES.color = tempColor;
        }
        else
        {
            var tempColor = InitText2ENG.color;
            tempColor.a = _alphaValue2; //1f makes it fully visible, 0f makes it fully transparent.
            InitText2ENG.color = tempColor;
        }
    }
    
    #endregion
}
