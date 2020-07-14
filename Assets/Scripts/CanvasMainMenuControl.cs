using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

public class CanvasMainMenuControl : MonoBehaviour
{
    public GameManager GM;
    public PlayerController Player;
    private bool isChangingLanguage = false;
    private LevelChanger LevelChanger;

    private float _alphaValue1, _inc1;                  //Variables para controlar la activación del MainMenu cuando termina la CutScene
    public bool AppearMainMenu, MainMenuFix;
    private float _alphaValue0 = 1, _inc0 = 1;          //Variables para controlar la desactivación del MainMenu cuando empiezas por primera vez la partida

    //Variables para mover icono bolita
    public List<CanvasGroup> Canvas;                    //lista de canvas para mostrar y ocultar segun corresponda
    public List<Button> SpListButtons, EngListButtons;  //listados de los botones de menú de pausa en español e inglés
    public GameObject BallIcon;                         //GameObject de la bola blanca
    public List<GameObject> BallIconPoints;             //lista de los puntos por los que se va a mover la bola blanca
    public List<Sprite> BallIconImages;                 //lista de sprites azul y blanca (por el momento)
    private float speedBall = 10;                       //velocidad a la que se desplaza la bola blanca entre los puntos
    private int pointID = 0;                            //ID del punto que esta junto al botón que tenemos seleccionado

    private bool IsControlsActive = false, IsCreditsActive = false;
    private bool CanUseCrosspad = true;                 //bool para habilitar el aumento o disminución del pointID y por tanto mover la bola blanca

    public bool IsEnglish = false;                      //variable booleana para guardar el idioma elegido

    public PostProcessingProfile PPProfile;             // El postprocessing profile almacenado para poder modificar parametros en tiempo real (como el DepthOfField)
    

    private int CrossPadUp, CrossPadDown;               // contador para controlar el CrossPad del gamepad para controlar los desplazamientos de los menús

    void Awake()
    {
        IsEnglish = UIController.IsEnglish;
        PPProfile = Resources.Load<PostProcessingProfile>("PPP InDung");
        PPProfile.depthOfField.enabled = false;
        LevelChanger = GameObject.Find("LevelChanger").GetComponent<LevelChanger>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        Player = GameObject.Find("Player").GetComponent<PlayerController>();

        if (GM.IsMainMenuControl == 1)
        {
            //Canvas[0].alpha = 1;
            //Canvas[0].interactable = true;
            //Canvas[0].blocksRaycasts = true;
            if (IsEnglish == false)
            {
                Canvas[1].alpha = 1;
                Canvas[1].interactable = true;
                Canvas[1].blocksRaycasts = true;
                Canvas[2].alpha = 0;
                Canvas[2].interactable = false;
                Canvas[2].blocksRaycasts = false;
            }
            else
            {
                Canvas[2].alpha = 1;
                Canvas[2].interactable = true;
                Canvas[2].blocksRaycasts = true;
                Canvas[1].alpha = 0;
                Canvas[1].interactable = false;
                Canvas[1].blocksRaycasts = false;
            }
        }
        else
        {
            Canvas[0].alpha = 0;
            Canvas[0].interactable = false;
            Canvas[0].blocksRaycasts = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        LerpToMoveIcon();

        if (GM.IsMainMenuControl == 1)
        {
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
                        if (pointID > 5)
                        {
                            pointID = 5;
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

            if (PlayerPrefs.HasKey("IsAnySaved_") == false) //si NO encuentra datos en el PlayerPrefs, desactiva el botón Load y baja el alpha
            {
                if (IsEnglish == false)
                {
                    SpListButtons[1].GetComponent<Button>().enabled = false;
                    var tempColor = SpListButtons[1].GetComponent<Image>().color;
                    tempColor.a = 0.5f;
                    SpListButtons[1].GetComponent<Image>().color = tempColor;
                }
                else
                {
                    EngListButtons[1].GetComponent<Button>().enabled = false;
                    var tempColor = EngListButtons[1].GetComponent<Image>().color;
                    tempColor.a = 0.5f;
                    EngListButtons[1].GetComponent<Image>().color = tempColor;
                }
            }
            else
            {
                if (IsEnglish == false)
                {
                    SpListButtons[1].GetComponent<Button>().enabled = true;
                    var tempColor = SpListButtons[1].GetComponent<Image>().color;
                    tempColor.a = 1;
                    SpListButtons[1].GetComponent<Image>().color = tempColor;
                }
                else
                {
                    EngListButtons[1].GetComponent<Button>().enabled = true;
                    var tempColor = EngListButtons[1].GetComponent<Image>().color;
                    tempColor.a = 1;
                    EngListButtons[1].GetComponent<Image>().color = tempColor;
                }
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton0) && SpListButtons[pointID].GetComponent<Button>().enabled == true)
            {
                SpListButtons[pointID].GetComponent<Button>().onClick.Invoke(); //busco el número de botón de la lista en base al pointID donde está la bola blanca
                                                                                //y ejecuto su función correspondiente del OnClick() 
            }
        }

        if (AppearMainMenu == true && MainMenuFix == false)
        {
            _inc1 = Time.deltaTime;
            _alphaValue1 = 0;

            if (IsEnglish == false)
            {
                Canvas[1].alpha = 1;
                Canvas[1].interactable = true;
                Canvas[1].blocksRaycasts = true;
                Canvas[2].alpha = 0;
                Canvas[2].interactable = false;
                Canvas[2].blocksRaycasts = false;
            }
            else
            {
                Canvas[2].alpha = 1;
                Canvas[2].interactable = true;
                Canvas[2].blocksRaycasts = true;
                Canvas[1].alpha = 0;
                Canvas[1].interactable = false;
                Canvas[1].blocksRaycasts = false;
            }
            
            StartCoroutine(FadeInMainMenuStart());
        }
    }

    #region IconMovement_Funtions

    public void LerpToMoveIcon()
    {
        BallIcon.transform.position = Vector3.Lerp(BallIcon.transform.position, BallIconPoints[pointID].transform.position, speedBall * Time.deltaTime);
    }
    public void OnNewPlayButton()
    {
        pointID = 0;
    }
    public void OnLoadButton()
    {
        pointID = 1;
    }
    public void OnControlsButton()
    {
        pointID = 2;
    }
    public void OnCreditsButton()
    {
        pointID = 3;
    }
    public void OnExitButton()
    {
        pointID = 4;
    }
    public void OnChangeLanguage()
    {
        pointID = 5;
    }
    public void DesactivateBallMoving()
    {
        speedBall = 0;
    }
    #endregion

    #region UI_Buttons

    public void NewPlayButton()
    {
        GM.SoundManager.CreateSound(1);
        PlayerPrefs.DeleteAll();
        GM.GlobalPlayerPowerLevel = 0;
        GameObject.Find("Player").GetComponent<PlayerController>().ChangePlayerMat();
        if (PlayerPrefs.HasKey("IsAnySaved_") == false) //si no hay partida guardada, solo quito el MainMenu
        {
            _inc0 = -Time.deltaTime;
            _alphaValue0 = 1;

            StartCoroutine(FadeOutMainMenuStart());
        }
        else //si hay partida guardada, hago el fundido para el primer OVERWORLD
        {
            LevelChanger.FadeToLevel(1);
        }
    }
    public void LoadButton()
    {
        //Cargar partida guardada
        GM.SoundManager.CreateSound(1);
        Canvas[0].alpha = 0;
        Canvas[0].interactable = false;
        Canvas[0].blocksRaycasts = false;
        GM.IsMainMenuControl = 0;
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
    public void ControlsButton()
    {
        GM.SoundManager.CreateSound(0);
        if (IsControlsActive == false)
        {
            IsControlsActive = true;
            CanUseCrosspad = false;
            speedBall = 0;
            BallIcon.transform.GetChild(0).GetComponent<Image>().sprite = BallIconImages[1];
        }
        else
        {
            IsControlsActive = false;
            CanUseCrosspad = true;
            speedBall = 10;
            BallIcon.transform.GetChild(0).GetComponent<Image>().sprite = BallIconImages[0];
        }

        if (IsControlsActive == true)
        {
            CanUseCrosspad = false;
            Canvas[5].alpha = 1;
            Canvas[5].interactable = true;
            Canvas[5].blocksRaycasts = true;

            if (IsEnglish == false) //panel controls español
            {
                for (int i = 0; i < SpListButtons.Count; i++)
                {
                    if (SpListButtons[i].interactable == true)
                    {
                        SpListButtons[i].interactable = false;
                    }

                    if (SpListButtons[i] == SpListButtons[2])
                    {
                        SpListButtons[2].interactable = true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < EngListButtons.Count; i++)
                {
                    if (EngListButtons[i].interactable == true)
                    {
                        EngListButtons[i].interactable = false;
                    }

                    if (EngListButtons[i] == EngListButtons[2])
                    {
                        EngListButtons[2].interactable = true;
                    }
                }
            }
        }
        else
        {
            OnControlsButton();
            CanUseCrosspad = true;
            Canvas[5].alpha = 0;
            Canvas[5].interactable = false;
            Canvas[5].blocksRaycasts = false;

            if (IsEnglish == false) //panel controls español
            {
                for (int i = 0; i < SpListButtons.Count; i++)
                {
                    if (SpListButtons[i].interactable == false)
                    {
                        SpListButtons[i].interactable = true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < EngListButtons.Count; i++)
                {
                    if (EngListButtons[i].interactable == false)
                    {
                        EngListButtons[i].interactable = true;
                    }
                }
            }
        }
    }
    public void CreditsButton()
    {
        GM.SoundManager.CreateSound(0);
        if (IsCreditsActive == false)
        {
            IsCreditsActive = true;
            CanUseCrosspad = false;
            speedBall = 0;
            BallIcon.transform.GetChild(0).GetComponent<Image>().sprite = BallIconImages[1];
        }
        else
        {
            IsCreditsActive = false;
            CanUseCrosspad = true;
            speedBall = 10;
            BallIcon.transform.GetChild(0).GetComponent<Image>().sprite = BallIconImages[0];
        }

        if (IsCreditsActive == true)
        {
            CanUseCrosspad = false;
            if (IsEnglish == false) //panel créditos español
            {
                Canvas[3].alpha = 1;
                Canvas[3].interactable = true;
                Canvas[3].blocksRaycasts = true;

                for (int i = 0; i < SpListButtons.Count; i++)
                {
                    if (SpListButtons[i].interactable == true)
                    {
                        SpListButtons[i].interactable = false;
                    }

                    if (SpListButtons[i] == SpListButtons[3])
                    {
                        SpListButtons[3].interactable = true;
                    }
                }
            }
            else //panel créditos inglés
            {
                Canvas[4].alpha = 1;
                Canvas[4].interactable = true;
                Canvas[4].blocksRaycasts = true;

                for (int i = 0; i < EngListButtons.Count; i++)
                {
                    if (EngListButtons[i].interactable == true)
                    {
                        EngListButtons[i].interactable = false;
                    }

                    if (EngListButtons[i] == EngListButtons[3])
                    {
                        EngListButtons[3].interactable = true;
                    }
                }
            }
        }
        else
        {
            OnCreditsButton();
            if (IsEnglish == false) //panel créditos español
            {
                Canvas[3].alpha = 0;
                Canvas[3].interactable = false;
                Canvas[3].blocksRaycasts = false;

                for (int i = 0; i < SpListButtons.Count; i++)
                {
                    if (SpListButtons[i].interactable == false)
                    {
                        SpListButtons[i].interactable = true;
                    }
                }
            }
            else //panel créditos inglés
            {
                Canvas[4].alpha = 0;
                Canvas[4].interactable = false;
                Canvas[4].blocksRaycasts = false;

                for (int i = 0; i < EngListButtons.Count; i++)
                {
                    if (EngListButtons[i].interactable == false)
                    {
                        EngListButtons[i].interactable = true;
                    }
                }
            }
            CanUseCrosspad = true;
        }
    }
    public void ExitButton()
    {
        //Salir de la aplicación
        GM.SoundManager.CreateSound(0);
        Application.Quit();
    }
    public void ChangeLanguage()
    {
        GM.SoundManager.CreateSound(0);
        isChangingLanguage = true;

        if (isChangingLanguage == true)
        {
            if (Canvas[1].alpha == 1) //Spanish está activo
            {
                Canvas[1].alpha = 0;
                Canvas[1].interactable = false;
                Canvas[1].blocksRaycasts = false;
                Canvas[2].alpha = 1;
                Canvas[2].interactable = true;
                Canvas[2].blocksRaycasts = true;
                IsEnglish = true;
                isChangingLanguage = false;
            }
            else if (Canvas[2].alpha == 1) //English está activo
            {
                Canvas[1].alpha = 1;
                Canvas[1].interactable = true;
                Canvas[1].blocksRaycasts = true;
                Canvas[2].alpha = 0;
                Canvas[2].interactable = false;
                Canvas[2].blocksRaycasts = false;
                IsEnglish = false;
                isChangingLanguage = false;
            }
        }
    }

    private IEnumerator FadeInMainMenuStart()
    {
        yield return new WaitForEndOfFrame();

        _alphaValue1 += _inc1;
        if (_alphaValue1 > 1)
        {
            _inc1 = 1;
            GM.IsMainMenuControl = 1;
            MainMenuFix = true;
        }
        else
        {
            StartCoroutine(FadeInMainMenuStart());
        }

        var tempAlpha = Canvas[0].alpha;
        tempAlpha = _alphaValue1; //1f makes it fully visible, 0f makes it fully transparent.
        Canvas[0].alpha = tempAlpha;
    }

    private IEnumerator FadeOutMainMenuStart()
    {
        yield return new WaitForEndOfFrame();

        _alphaValue0 += _inc0;
        if (_alphaValue0 > 0)
        {
            StartCoroutine(FadeOutMainMenuStart());
        }
        else if (_alphaValue0 < 0)
        {
            GM.GlobalPlayerPowerLevel = 0;
            Canvas[0].interactable = false;
            Canvas[0].blocksRaycasts = false;
            GM.IsMainMenuControl = 0;
            GameObject.Find("RaiANIMfixedDefNOTPOSE").GetComponent<Animator>().Play("WakeUp");
            GM.DelayAwakeRutine = true;
        }
        else
        {
            StartCoroutine(FadeOutMainMenuStart());
        }

        var tempAlpha = Canvas[0].alpha;
        tempAlpha = _alphaValue0; //1f makes it fully visible, 0f makes it fully transparent.
        Canvas[0].alpha = tempAlpha;
    }

    #endregion
}