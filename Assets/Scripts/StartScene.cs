using UnityEngine;

public class StartScene : MonoBehaviour
{
    void Awake()
    {
        if (!GameManager.instance.initialized)
        {
            GameManager.instance.Init();
        }

    }

    public void Start()
    {
        GameManager.instance.CanvasPaused = GameObject.Find("CanvasPausedController").GetComponent<CanvasPausedControl>();
        GameManager.instance.CanvasMM = GameObject.Find("CanvasMainMenuController").GetComponent<CanvasMainMenuControl>();

        GameManager.instance.SoundManager = GameObject.Find("SoundController").GetComponent<SoundController>();

        GameManager.instance.RaiGameObject = GameObject.Find("Rai");
        GameManager.instance.RaiLight = GameObject.Find("PlayerLight").GetComponent<Light>();
        GameManager.instance.RaiMat = GameManager.instance.RaiGameObject.GetComponent<Renderer>().material;

        GameManager.instance.RaiEmisOrange = new Color(2.118f, 0.455f, 0);
        GameManager.instance.RaiEmisBlue = new Color(0, 1.226f, 2.118f);
        GameManager.instance.RaiEmisStandard = new Color(1.153f, 1.153f, 1.153f);

        if (PlayerPrefs.HasKey("IsAnySaved_") == true)
        {
            GameManager.instance.IsRaiAwake = true;
        }

        if (GameManager.instance.IsRaiAwake == true)
        {
            GameObject.Find("RaiANIMfixedDefNOTPOSE").GetComponent<Animator>().Play("Idle");
        }

        UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if (scene.name == "OVERWORLD")
        {
            GameManager.instance.GlobalPlayerPowerLevel = 0;
        }
    }
}
