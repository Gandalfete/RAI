using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathFall : MonoBehaviour
{
    // Este es el script del limite de caida del player. Se encarga de detectar cuando el jugador se ha caido al vacio (con un trigger) y 
    // de comprobar en el GM donde fue su ultima coordenada de checkpoint, y de paso teletransportarlo a dicha coordenada.

    GameManager GM;
    Collider _other;                            //variable para sacar el Collider other fuera del OnTriggerEnter

    private float _inc0,_alphaValue0;           //variables para regular el incremento del alpha del FadeIn/FadeOut
    private Image BlackWindow;                  //Image en negro para falsear el FadeIn/FadeOut

    private void Awake()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        BlackWindow = GameObject.Find("CanvasFadeToRespawn").transform.GetChild(0).GetComponent<Image>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GM.SoundManager.CreateSound(5);
            _other = other;
            _inc0 = Time.deltaTime;
            _alphaValue0 = 0;

            StartCoroutine(FadeInOut());
        }
    }

    public IEnumerator FadeInOut()
    {
        yield return new WaitForEndOfFrame();

        _alphaValue0 += _inc0;
        if (_alphaValue0 > 1)
        {
            _inc0 = -Time.deltaTime;
            _other.transform.position = GM.PlayerCurrentCheckpoint;
            StartCoroutine(FadeInOut());
        }
        else if (_alphaValue0 < 0)
        {
            _inc0 = 0;
        }
        else
        {
            StartCoroutine(FadeInOut());
        }

        var tempColor = BlackWindow.color;
        tempColor.a = _alphaValue0; //1f makes it fully visible, 0f makes it fully transparent.
        BlackWindow.color = tempColor;
    }
}
