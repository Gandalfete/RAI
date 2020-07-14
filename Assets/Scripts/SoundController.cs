using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public List<GameObject> SoundEffects;

    public void CreateSound(int IndexSound)
    {
        GameObject NewSound = Instantiate(SoundEffects[IndexSound]);
        Destroy(NewSound, 6.5f);
    }
}
