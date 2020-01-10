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
        FadeFromBlack();
        cover.color = Color.black;
    }

    public void FadeToBlack(string levelToLoad)
    {
        cover.canvasRenderer.SetAlpha(0);
        cover.CrossFadeAlpha(1f, 0.5f, true);
        StartCoroutine(LoadLevel(levelToLoad));
    }
    IEnumerator LoadLevel(string level)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene(level);
    }

    public void FadeToBlack()
    {
        cover.canvasRenderer.SetAlpha(0);
        cover.CrossFadeAlpha(1f, 0.5f, true);
    }


    public void FadeFromBlack()
    {
        cover.canvasRenderer.SetAlpha(1);
        cover.CrossFadeAlpha(0f, 0.5f, true);
    }

}
