using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireJet : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particleSystem;

    private GameObject collisionBox;
    private Animator anim;

    public FireJetHandler fjh;

    [SerializeField]
    private GameObject lightSource;

    //[System.NonSerialized]
    public  bool isActive;
    void Start()
    {
        anim = GetComponent<Animator>();
        isActive = false;
    }
    public void setActive(float Duration)
    {
        isActive = true;
        StartCoroutine(fire(Duration));
    }

    IEnumerator fire(float duration)
    {
        Debug.Log("fire");
        //allow light and emission to fade in;
        //make emission fade in first;
        anim.SetBool("FadeIn", true);

        lightSource.SetActive(true);

        yield return new WaitForSeconds(1f);

        anim.SetBool("FadeIn", false);
        //activate all visuals etc.
        collisionBox.SetActive(true);
        particleSystem.Play();
        yield return new WaitForSeconds(duration - 1);
        particleSystem.Stop();
        collisionBox.SetActive(false);
        anim.SetBool("FadeOut", true);
        yield return new WaitForSeconds(1f);
        anim.SetBool("FadeOut", false);

        Debug.Log("stopFire");

        isActive = false;
        //set emission to fade

        lightSource.SetActive(false);

        fjh.SetInactive();
        

        //StopCoroutine(fire(0));
    }

}
