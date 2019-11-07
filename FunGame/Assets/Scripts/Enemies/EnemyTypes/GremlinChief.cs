using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GremlinChief : EnemyBase
{
    [Header("Uniques")]
    private GameObject warBanner;
    public float bannerBuff;
    private ObjectPooler pooler;

    public override void Update()
    {
        base.Update();
    }

    public override void ReTarget()
    {
        base.ReTarget();
        actionTwo();
    }

    public override void actionOne()
    {
        base.actionOne();
        playerCode.TakeDamage(Mathf.RoundToInt(attackPower * atkMult));
    }

    public override void actionTwo()
    {
        warBanner = pooler.warBanners[pooler.warBanners.Count];
        warBanner.transform.position = transform.position;
        warBanner.SetActive(true);
    }
}