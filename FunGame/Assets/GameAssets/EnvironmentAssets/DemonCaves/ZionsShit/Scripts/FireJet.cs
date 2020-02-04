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

        yield return new WaitForSeconds(1f);

        
        //activate all visuals etc.
       // collisionBox.SetActive(true);
       // particleSystem.Play();
        yield return new WaitForSeconds(duration - 1f);
        // particleSystem.Stop();
        // collisionBox.SetActive(false);
        anim.SetBool("FadeIn", false);
        anim.SetBool("FadeOut", true);
        Debug.Log("stopFire");

        yield return new WaitForSeconds(1f);
        anim.SetBool("FadeOut", false);

       

        isActive = false;
        //set emission to fade

        fjh.SetInactive();
        

        //StopCoroutine(fire(0));
    }

}
