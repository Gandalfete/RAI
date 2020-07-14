using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public Animator animator;

    private int levelToLoad;

    public void FadeToLevel(int levelID)
    {
        levelToLoad = levelID;
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        print(levelToLoad); //=====>
        SceneManager.LoadScene(levelToLoad);
    }
}
