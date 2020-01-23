using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{

    Image cover;

    public void Start()
    {
        cover = GetComponentInChildren<Image>();
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            FadeFromBlack();
        }
        cover.color = Color.black;
    }

    public void FadeToBlack(string levelToLoad)
    {
        FadeToBlack();
        StartCoroutine(LoadLevel(levelToLoad));
    }
    IEnumerator LoadLevel(string level)
    {
        yield return new WaitForSecondsRealtime(0.3f);
        SceneManager.LoadScene(level);
    }

    public void FadeToBlack()
    {
        cover.canvasRenderer.SetAlpha(0);
        cover.CrossFadeAlpha(1f, 0.3f, true);
    }


    public void FadeFromBlack()
    {
        cover.canvasRenderer.SetAlpha(1);
        StartCoroutine(TrueFade());
    }

    IEnumerator TrueFade()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        cover.CrossFadeAlpha(0f, 0.3f, true);
    }
}
