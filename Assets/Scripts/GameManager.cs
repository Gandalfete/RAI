using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private bool _initialized = false;

    public LevelChanger LevelChanger;

    public bool DelayAwakeRutine, IsRaiAwake;
    public float DelayAwake;

    public CanvasPausedControl CanvasPaused;
    public int IsMainMenuControl;                       // Con 1 el MainMenu está activo, y con 0 inactivo

    public CanvasMainMenuControl CanvasMM;
    private float _alphaValue, _inc;            //Variables para controlar la aparición/desaparición del icono de la bola al guardar partida
    public bool StartingBallIcon = false;

    public bool BlueDimensionActive;                    // Bools que se encargan de las dimensiones azul y naranja, y de saber si el jugador
    public bool OrangeDimensionActive;                  // se encuentra dentro de una plataforma grande. Estos bools son importantes ya que
    public bool IsPlayerInsidePlatform;                 // comunican con el resto de scripts

    public bool EntranceDung1Open;
    public bool DungeonOneKeyPointAActive, DungeonOneKeyPointBActive, DungeonOneKeyPointCActive;
    public bool DungeonTwoKeyPointAActive, DungeonTwoKeyPointBActive, DungeonTwoKeyPointCActive;
    public bool DungeonThreeKeyPointAActive, DungeonThreeKeyPointBActive, DungeonThreeKeyPointCActive;
    public static int LastDangeonCompleted;

    public bool TutorialXYShowed, TutorialAShowed, TutorialBShowed, TutorialRTShowed, TutorialLTShowed;

    public float RaiLightStandardQuantity = 0.55f;
    public float RaiLightMaximumStandardQuantity = 1.85f; // Floats de cantidad de intensidad de la luz de Rai
    private float RaiLightColoredQuantity = 1;

    public int GlobalPlayerPowerLevel;                  // Int global del nivel del player (falta programar que el player compruebe tambien cuando
                                                        // consigue un poder nuevo, y no solo en el Awake()

    public Vector3 PlayerCurrentCheckpoint;             // Vector que almacena la ultima posicion de checkpoint del player

    public GameObject RaiGameObject;                    // GameObject que almacena al propio MODELO de Rai para poder acceder comodamente a cualquier componente suyo
    public Light RaiLight;
    public Material RaiMat;
    public SkinnedMeshRenderer RaiMatRenderer;

    [HideInInspector] public Color RaiEmisBlue, RaiEmisOrange, RaiEmisStandard, RaiEmisMaximumStandard;  // Valores de vector de los colores (luz y emissive) para reutilizarlos

    public SoundController SoundManager;

    #region Sistema Singleton de arranque del GameManager

    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject newGameObj = new GameObject("GameManager");
                _instance = newGameObj.AddComponent<GameManager>();

                DontDestroyOnLoad(newGameObj);
            }
            return _instance;
        }
    }

    public bool initialized { get { return _initialized; } }

    public void Init()
    {
        _initialized = true;
        CanvasPaused = GameObject.Find("CanvasPausedController").GetComponent<CanvasPausedControl>();
        IsMainMenuControl = 1;  //al levantar el GameManager por primera vez, le decimos que muestre el MainMenu
        CanvasMM = GameObject.Find("CanvasMainMenuController").GetComponent<CanvasMainMenuControl>();

        SoundManager = GameObject.Find("SoundController").GetComponent<SoundController>();

        LevelChanger = GameObject.Find("LevelChanger").GetComponent<LevelChanger>();

        RaiGameObject = GameObject.Find("Rai");
        RaiLight = GameObject.Find("PlayerLight").GetComponent<Light>();
        RaiMat = RaiGameObject.GetComponent<SkinnedMeshRenderer>().material;
        RaiMatRenderer = RaiGameObject.GetComponent<SkinnedMeshRenderer>();
        if (PlayerPrefs.HasKey("IsAnySaved_") == true)
        {
            RaiMatRenderer.sharedMaterial = GameObject.Find("Player").GetComponent<PlayerController>().PlayerRaiMat[PlayerPrefs.GetInt("PlayerPowerLevel_")];
        }
        else
        {
            RaiMatRenderer.sharedMaterial = GameObject.Find("Player").GetComponent<PlayerController>().PlayerRaiMat[0];
        }

        RaiEmisOrange = new Color(2.118f, 0.455f, 0);
        RaiEmisBlue = new Color(0, 1.226f, 2.118f);
        RaiEmisStandard = new Color(1.153f, 1.153f, 1.153f);
        RaiEmisMaximumStandard = new Color(2.1f, 2.1f, 2.1f);
    }
    #endregion

    void Update()
    {
        if (CanvasPaused.IsGamePausedControl == 0 && GameManager.instance.IsMainMenuControl == 0)
        {
            Cursor.visible = false;            
        }
        else if (CanvasPaused.IsGamePausedControl == 1)
        {
            Cursor.visible = true;
        }
        else if (GameManager.instance.IsMainMenuControl == 1)
        {
            Cursor.visible = true;
        }
        Cursor.lockState = CursorLockMode.Confined;

        RaiMat = RaiGameObject.GetComponent<SkinnedMeshRenderer>().sharedMaterial;
        if (BlueDimensionActive == true)
        {
            RaiMat.SetColor("_EmissionColor", RaiEmisBlue);
            RaiLight.intensity = RaiLightColoredQuantity;
            RaiLight.color = RaiEmisBlue;
        }

        if (OrangeDimensionActive == true)
        {
            RaiMat.SetColor("_EmissionColor", RaiEmisOrange);
            RaiLight.intensity = RaiLightColoredQuantity;
            RaiLight.color = RaiEmisOrange;
        }

        if (OrangeDimensionActive == false && BlueDimensionActive == false)
        {
            RaiMat.SetColor("_EmissionColor", RaiEmisStandard);
            RaiLight.intensity = RaiLightStandardQuantity;
            RaiLight.color = RaiEmisStandard;
        }

        if (DelayAwakeRutine == true)
        {
            DelayAwake += Time.deltaTime;
            if (DelayAwake > 2.9f)
            {
                IsRaiAwake = true;
                DelayAwake = 0;
                DelayAwakeRutine = false;
            }
        }
    }

    public void SaveGame(int _lastDungeon)
    {
        PlayerPrefs.SetInt("IsAnySaved_", 1);
        PlayerPrefs.SetInt("PlayerPowerLevel_", GlobalPlayerPowerLevel);
        PlayerPrefs.SetInt("LastDangeonCompleted_", _lastDungeon);
    }

    public void DeactivateSaveBallIcon()
    {
        _inc = Time.deltaTime;
        _alphaValue = 0;

        StartCoroutine(BallIconAppears());
    }
    public IEnumerator BallIconAppears()
    {
        yield return new WaitForEndOfFrame();
       
        _alphaValue += _inc;
        if (_alphaValue > 3)
        {
            _inc = -Time.deltaTime;
            StartCoroutine(BallIconAppears());
        }
        else if (_alphaValue < 0)
        {
            //print("Fin de la rutina 2");
            _inc = 0;
        }
        else
        {
            StartCoroutine(BallIconAppears());
        }

        CanvasMM.Canvas[6].alpha = _alphaValue;      
    }
        
}
