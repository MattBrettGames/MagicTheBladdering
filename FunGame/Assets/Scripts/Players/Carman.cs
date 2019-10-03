using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carman : PlayerBase
{

    private int curseHunter;
    private List<GameObject> curseList = new List<GameObject>();
    public Weapons[] weapons = new Weapons[2];

    [Header("Dagger Slash")]
    public int xDamage;
    private bool canDaggerSlash;

    [Header("Headbutt")]
    public int headbuttKnockback;

    [Header("Tattoo Trap")]
    public float secsToTrapFade;
    private ObjectPooler pooler;

    [Header("Dodge Roll")]
    public float dodgeImpulse;
    private bool iFrames;

    [Header("DemonSlayer")]
    public int slayerDamage;
    private bool canComboToSlayer;


    public override void XAction()
    {
        anim.SetTrigger("XAction");
    }
    public void OpenComboToDaggerSlash() { canDaggerSlash = true; }
    public void CloseComboToDaggerSlash() { canDaggerSlash = false; }

    public override void YAction()
    {
        if (curseHunter == 0)
        {
            anim.SetTrigger("Headbutt");
        }
        else if (curseHunter != 0)
        {
            ShadowStep();
        }
        else if (curseHunter != 0 && canComboToSlayer)
        {
            DemonSlayer();
        }
    }

    public override void BAction()
    {
        anim.SetTrigger("BAction");
    }

    public void PLACETRAP()
    {
        GameObject trap = pooler.curseTrapList[0];
        trap.transform.position = transform.position;
        trap.SetActive(true);
        TRAPPASSER(trap);
    }

    private IEnumerator TRAPPASSER(GameObject trap)
    {
        yield return new WaitForSeconds(secsToTrapFade);
        TRAPFADE(trap);
    }

    private void TRAPFADE(GameObject trap)
    {
        trap.GetComponent<MeshRenderer>().enabled = false;
    }

    public override void AAction()
    {
        float hori = Input.GetAxis(horiPlayerInput);
        float vert = Input.GetAxis(vertPlayerInput);
        anim.SetTrigger("AAction");
        rb2d.AddForce(new Vector3(hori, 0, vert) * dodgeImpulse, ForceMode.Impulse);
    }
    public void GAINIFRAMES() { iFrames = true; }
    public void LOSEIFRAMES() { iFrames = false; }
    
    private void ShadowStep()
    {
        float dis = Vector3.Distance(transform.position, curseList[0].transform.position);
        Vector3 dire = transform.position - curseList[0].transform.position;

        transform.Translate(dire * (dis + 1));
    }

    private void DemonSlayer()
    {
        ShadowStep();
        anim.SetTrigger("DemonSlayer");
    }

    public override void TakeDamage(int damageInc)
    {
        if (!iFrames) { base.TakeDamage(damageInc); }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "CurseCircle")
        {
            curseHunter++;
            speed += curseHunter;
            curseList.Add(other.gameObject);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "CurseCircle")
        {
            curseHunter--;
            speed -= curseHunter;
            curseList.Remove(other.gameObject);
        }
    }


}
