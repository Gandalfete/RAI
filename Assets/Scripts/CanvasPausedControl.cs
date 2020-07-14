using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

public class CanvasPausedControl : MonoBehaviour
{
    GameManager GM;
    CanvasMainMenuControl CanvasMM;

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
    public int IsGamePausedControl;                     // Con 1 el menú de pausa está activo, y con 0 inactivo

    private int CrossPadUp, CrossPadDown;               // contador para controlar el CrossPad del gamepad para controlar los desplazamientos de los menús
    
    void Awake()
    {
        IsEnglish = UIController.IsEnglish;
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        CanvasMM = GameObject.Find("CanvasMainMenuController").GetComponent<CanvasMainMenuControl>();
        PPProfile = Resources.Load<PostProcessingProfile>("PPP InDung");
        PPProfile.depthOfField.enabled = false;
        IsGamePausedControl = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGamePausedControl == 1)
        {
            PPProfile.depthOfField.enabled = true;
            Canvas[0].alpha = 1;
            Canvas[0].interactable = true;
            Canvas[0].blocksRaycasts = true;
            if (IsEnglish == false)
            {
                Canvas[1].alpha = 1;
                Canvas[1].interactable = true;
                Canvas[1].blocksRaycasts = true;
            }
            else
            {
                Canvas[2].alpha = 1;
                Canvas[2].interactable = true;
                Canvas[2].blocksRaycasts = true;
            }
            LerpToMoveIcon();
        }
        else if (IsGamePausedControl > 1)
        {
            Canvas[0].alpha = 0;
            Canvas[0].interactable = false;
            Canvas[0].blocksRaycasts = false;
            PPProfile.depthOfField.enabled = false;
            IsGamePausedControl = 0;
        }

        if (IsGamePausedControl == 1)
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
                        if (pointID > 4)
                        {
                            pointID = 4;
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
                SpListButtons[pointID].GetComponent<Button>().onClick.Invoke(); //busco el número de botón de la lista en base al pointID donde está la bola blanca
                                                                                //y ejecuto su función correspondiente del OnClick() 
            }
        }
    }

    #region IconMovement_Funtions

    public void LerpToMoveIcon()
    {
        BallIcon.transform.position = Vector3.Lerp(BallIcon.transform.position, BallIconPoints[pointID].transform.position, speedBall * Time.deltaTime);
    }
    public void OnContinueButton()
    {
        pointID = 0;
    }
    public void OnBackMenuButton()
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
    public void DesactivateBallMoving()
    {
        speedBall = 0;
    }
    #endregion

    #region UI_Buttons

    public void ContinueButton()
    {
        GM.SoundManager.CreateSound(0);
        Canvas[0].alpha = 0;
        Canvas[0].interactable = false;
        Canvas[0].blocksRaycasts = false;
        PPProfile.depthOfField.enabled = false;
        IsGamePausedControl = 0;
    }
    public void MainMenuButton()
    {
        GM.SoundManager.CreateSound(0);
        Canvas[0].alpha = 0;
        Canvas[0].interactable = false;
        Canvas[0].blocksRaycasts = false;
        IsGamePausedControl = 0;

        CanvasMM.Canvas[0].alpha = 1;
        CanvasMM.Canvas[0].interactable = true;
        CanvasMM.Canvas[0].blocksRaycasts = true;
        if (IsEnglish == false)
        {
            CanvasMM.Canvas[1].alpha = 1;
            CanvasMM.Canvas[1].interactable = true;
            CanvasMM.Canvas[1].blocksRaycasts = true;
            CanvasMM.Canvas[2].alpha = 0;
            CanvasMM.Canvas[2].interactable = false;
            CanvasMM.Canvas[2].blocksRaycasts = false;
        }
        else
        {
            CanvasMM.Canvas[2].alpha = 1;
            CanvasMM.Canvas[2].interactable = true;
            CanvasMM.Canvas[2].blocksRaycasts = true;
            CanvasMM.Canvas[1].alpha = 0;
            CanvasMM.Canvas[1].interactable = false;
            CanvasMM.Canvas[1].blocksRaycasts = false;
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

            if (IsEnglish == false) //panel controles español
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

            if (IsEnglish == false) //panel controles español
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
        Application.Quit();
    }

    #endregion

}
