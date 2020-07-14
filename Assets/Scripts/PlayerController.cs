using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //  Bienvenidos al delicioso script del player! Si hay algo que no entendais aun despues de leer mis increibles
    //  anotaciones, no dudeis en preguntarmelo. 
    //  PD: Si vais a añadir codigo propio, explicad para que sirve tambien!

    // Nota aclaratoria: En este script solo se encuentran los inputs de movimiento y del 1er y 4to poder. Los de los imanes son independientes de cada iman
    // (se que no es lo mejor optimizado pero no consume mucho y es menos lioso)


    public float Speed, JumpSpeed, SprintSpeed;  // Velocidades y fisica
    public float Gravity;                        // Gravedad

    public float JumpDelayAfterPause;            // Contador para que al reanudar la partida, Rai no salte automáticamente

    public int PlayerPowerLevel;                 // Nivel de poder actual del jugador. Este script le pregunta en el awake al GM cual es su poder actual
                                                 // Falta hacer que se pueda actualizar sin ser en el awake pero tampoco en el update para mejor performance

    public List<Material> PlayerRaiMat;
    public SkinnedMeshRenderer PlayerRenderer;

    private CharacterController Control;
    private Vector3 MoveDir = Vector3.zero;     // Cosas varias para que el jugador no se vuelva loco en el aire (mejor no tocar mucho de estos)
    private Vector3 JumpDir = Vector3.zero;
    private Quaternion RotationDirBeforeJump;
    private bool IsJumping = false;             // Bool que sirve para diferenciar una caida de un salto. Si saltas y estas en el aire no es lo mismo que
                                                // caer y estar en el aire

    public bool PlayerAboveMagnet;              // Bool para saber si el jugador esta sobre un iman para no hacer troll-physics

    public bool IsPlayerPausedForMagnetAndCamera;

  

    public Transform CameraTransform;

    private RaycastHit HitFoot;                 // Raycast que comprueba si estas sobre un iman, una plataforma, etc

    private float TimeForIdleB;

    private GameObject Platform;                // El jugador se vuelve hijo de las plataformas moviles sobre las que salta, este G.O. se encarga de almacenar
                                                // sobre cual esta
    public GameManager GM;
    public CanvasPausedControl CanvasPaused;
    public int IsMainMenuActive;

    public Animator Anim;

    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        CanvasPaused = GameObject.Find("CanvasPausedController").GetComponent<CanvasPausedControl>();
        
        UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        print(scene.buildIndex); //=====>
        
        Control = GetComponent<CharacterController>();
        Anim = GetComponentInChildren<Animator>();

        if (PlayerPrefs.HasKey("IsAnySaved_") == true) //si encuentra datos en el PlayerPrefs, carga esa escena guardada
        {
            GM.GlobalPlayerPowerLevel = PlayerPrefs.GetInt("PlayerPowerLevel_");
            GameManager.LastDangeonCompleted = PlayerPrefs.GetInt("LastDangeonCompleted_");
        }
        PlayerRenderer = GameObject.Find("Rai").GetComponent<SkinnedMeshRenderer>();
        ChangePlayerMat();
    }


    public void ChangePlayerMat()
    {
        PlayerPowerLevel = GM.GlobalPlayerPowerLevel;
        PlayerRenderer.sharedMaterial = PlayerRaiMat[PlayerPowerLevel];
    }

    void Update()
    {
        IsMainMenuActive = GM.IsMainMenuControl;

        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) // Moviendose el player, se activa la anim. de correr y el contador del IdleB se reinicia
        {
            Anim.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal")));
            TimeForIdleB = 0;

        }
        else
        {
            TimeForIdleB += Time.deltaTime;
            Anim.SetFloat("Speed", 0);
        }

        if(TimeForIdleB > 10 && TimeForIdleB < 13)
        {
            Anim.SetBool("IsIdleB", true);
        }
        else
        {
            Anim.SetBool("IsIdleB", false);
        }
        if (TimeForIdleB > 13)
            TimeForIdleB = 0;

        if(IsJumping == true)
        {
            Anim.SetBool("JumpAnim", true);
        }
        else
        {
            Anim.SetBool("JumpAnim", false);
        }
 

        if (Input.GetButtonDown("Pause") && IsMainMenuActive == 0) //para no poder abrir la pausa si estás en MainMenu
        {
            GM.SoundManager.CreateSound(0);
            CanvasPaused.IsGamePausedControl++; //controlamos que esté activo el menú de pausa con un int, gestionado en el CanvasPausedControl
        }

        if (CanvasPaused.IsGamePausedControl == 1 || IsMainMenuActive == 1)
        {
            JumpDelayAfterPause = 0;
            IsPlayerPausedForMagnetAndCamera = true;
            if (GM.IsRaiAwake == true)
            {
                Anim.enabled = false;
            }
        }
        else if (CanvasPaused.IsGamePausedControl == 0 || IsMainMenuActive == 0)
        {
            if (GM.IsRaiAwake == true)
            {
                Anim.enabled = true;
                JumpDelayAfterPause += Time.deltaTime;
                IsPlayerPausedForMagnetAndCamera = false;

                if (PlayerPowerLevel >= 1)  // Si el jugador tiene el primer nivel de carga, puede usar solo las dimensiones
                {
                    DimensionShifting();
                }

                if (PlayerPowerLevel >= 4) // Una vez que tenga el nivel 4 de carga, podra iluminar mas fuertemente para poder completar la 4ta dungeon
                {

                    if (Input.GetButton("Fire2"))
                    {
                        if (GM.BlueDimensionActive == false && GM.OrangeDimensionActive == false)
                        {
                            GM.RaiMat.SetColor("_EmissionColor", GM.RaiEmisMaximumStandard);
                            GM.RaiLight.intensity = GM.RaiLightMaximumStandardQuantity;
                        }
                    }
                    else
                    {
                        if (GM.BlueDimensionActive == false && GM.OrangeDimensionActive == false)
                        {
                            GM.RaiLight.intensity = GM.RaiLightStandardQuantity;
                        }
                    }
                }

                if (Control.isGrounded) //Si toca el suelo...
                {
                    //Esta linea hace que el vector Y del vector auxiliar de salto del player se resetee a 0 (sin esto, se buggea al subir cuestas)
                    JumpDir.y = 0;

                    IsJumping = false;

                    MoveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                    MoveDir = CameraTransform.TransformDirection(MoveDir);
                    MoveDir.y = 0;
                    MoveDir *= Speed;



                    //Al saltar...
                    if (JumpDelayAfterPause > 0.05f)
                    {
                        if (Input.GetButtonDown("Jump"))
                        {
                            GM.SoundManager.CreateSound(2);
                            RotationDirBeforeJump = transform.rotation;
                            IsJumping = true;
                            JumpDir.y = JumpSpeed;
                            MoveDir.y = JumpDir.y;
                        }
                    }


                }
                else //Si esta en el aire... (no necesariamente tiene que haber sido un salto)
                {
                    JumpDir.y -= Gravity * Time.deltaTime;
                    MoveDir = new Vector3(Input.GetAxis("Horizontal") * Speed, JumpDir.y, Input.GetAxis("Vertical") * Speed);
                    MoveDir = CameraTransform.TransformDirection(MoveDir);

                    //Arreglo de un fallo que giraba al player al saltar y no moverse
                    if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
                    {
                        RotationDirBeforeJump = transform.rotation;
                    }

                }

                Vector3 NewDir = MoveDir;
                NewDir.y = 0;
                if (NewDir != Vector3.zero)
                {
                    if (MoveDir.x != 0 || MoveDir.z != 0)
                    {
                        transform.rotation = Quaternion.LookRotation(NewDir);
                    }
                }

                //Llama a la funcion que activa la neblina y la funcion encargada de cambiar el color de esta [WORK IN PROGRESS]
                ActivateFog();
                ChangeFogColor();

                MoveDir.y -= Gravity * Time.deltaTime;
                Control.Move(MoveDir * Time.deltaTime);

                //Este if lo mantengo para arreglar lo del player girandose en el aire + por si en algun momento queremos distinguir
                // cuando el player esta en el aire habiendo o no pulsado salto
                if (IsJumping == true)
                {
                    if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
                    {
                        transform.rotation = RotationDirBeforeJump;
                    }

                }

                //Llama al sistema de vinculacion del player a las plataformas + arregla el ascensor iman troll 
                PlatfSystem();
            }
        }
    }



    void ActivateFog()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogStartDistance = 2.1f;
            RenderSettings.fogEndDistance = 161;
            RenderSettings.fogDensity = 0.0015f;
        }

    }

    void ChangeFogColor()
    {
        if (GM.BlueDimensionActive == true)
            RenderSettings.fogColor = new Color(68, 207, 255);
        if (GM.OrangeDimensionActive == true)
            RenderSettings.fogColor = new Color(255, 172, 67);
    }


    private void PlatfSystem()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out HitFoot, 1.5f))
        {

            if (HitFoot.collider.gameObject.CompareTag("Magnet"))
            {
                PlayerAboveMagnet = true;
            }
            else if (HitFoot.collider.gameObject.tag != "Magnet")
            {
                PlayerAboveMagnet = false;
            }
            if (HitFoot.collider.gameObject.CompareTag("Platform") && IsJumping == false)
            {
                Platform = HitFoot.collider.gameObject;
                transform.SetParent(Platform.transform);
            }
            else
            {
                Platform = null;
                transform.SetParent(null);
   
            }
            if (IsJumping == true)
            {
                Platform = null;
                transform.SetParent(null);

            }

        }
    }

    void DimensionShifting()
    {
        if (Input.GetButtonDown("Fire4"))
        {
            GM.SoundManager.CreateSound(3);
            if (GM.IsPlayerInsidePlatform == false)
            {
                GM.BlueDimensionActive = !GM.BlueDimensionActive;
                GM.OrangeDimensionActive = false;
            }

        }
        if (Input.GetButtonDown("Fire3"))
        {
            GM.SoundManager.CreateSound(3);
            if (GM.IsPlayerInsidePlatform == false)
            {
                GM.OrangeDimensionActive = !GM.OrangeDimensionActive;
                GM.BlueDimensionActive = false;
            }
        }
    }
}
