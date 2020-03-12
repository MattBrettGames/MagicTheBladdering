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
      //  fjh = GameObject.Find("FireJetMaster").GetComponent<FireJetHandler>();
        anim = GetComponent<Animator>();
        isActive = false;
    }
    public void setActive(float Duration)
    {
        isActive = true;
        StartCoroutine(fire(Duration));
    }
    public void ThisSetInactive()
    {
            isActive = false;
            fjh.SetInactive();
    }

    IEnumerator fire(float duration)
    {
        Debug.Log("fire");
        //allow light and emission to fade in;
        //make emission fade in first;
        anim.SetBool("FadeIn", true);
        yield return new WaitForSeconds(duration - 1);
        anim.SetBool("FadeIn", false);
        anim.SetBool("FadeOut", true);
        Debug.Log("stopFire");

        yield return new WaitForSeconds(1);
        anim.SetBool("FadeOut", false);
        yield return null;

        ThisSetInactive();
        

       // StopCoroutine(fire(0));
    }

}
