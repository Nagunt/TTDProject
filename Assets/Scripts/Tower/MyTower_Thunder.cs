using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTower_Thunder : MyTower
{
    private float delay = 0;
    private Monster target;
    [Header("- Attribute")]
    [SerializeField]
    private LineRenderer thunderEffect;

    public override void Attack()
    {
        StartCoroutine(_Attack());
    }

    private IEnumerator _Attack()
    {
        delay = attackDelay;
        if(target != null)
        {
            thunderEffect.enabled = true;
            thunderEffect.positionCount = 2;
            thunderEffect.SetPositions(new Vector3[] { transform.position, target.transform.position });
            target?.GetDamage(attack);
            yield return new WaitForSeconds(0.1f);
            thunderEffect.positionCount = 0;
            thunderEffect.enabled = false;
        }
    }

    public override void Init()
    {
        attack = 150;
        attackDelay = 3f;
        StartCoroutine(Routine());
    }

    private IEnumerator Routine()
    {
        while (true)
        {
            if (delay > 0) delay -= Time.deltaTime;
            if (delay < 0) delay = 0;

            List<Collider2D> collider2Ds = new List<Collider2D>();
            ContactFilter2D contactFilter2D = new ContactFilter2D();
            contactFilter2D.SetLayerMask(LayerMask.GetMask("Monster"));
            if (Physics2D.OverlapCollider(attackArea, contactFilter2D, collider2Ds) > 0)
            {
                if (target == null)
                {
                    target = collider2Ds[0].GetComponent<Monster>();
                }
                else
                {
                    bool isTarget = false;
                    // 타겟이 범위 안에 계속 존재하면 유지한다.
                    for (int i = 0; i < collider2Ds.Count; ++i)
                    {
                        Monster tempMonster = collider2Ds[i].GetComponent<Monster>();
                        if (tempMonster == null) continue;
                        if (tempMonster == target)
                        {
                            isTarget = true;
                            break;
                        }
                    }
                    if (isTarget == false)
                    {
                        target = collider2Ds[0].GetComponent<Monster>();
                    }
                }
                if (delay <= 0)
                {
                    Attack();
                }
            }
            yield return 0;
        }
    }
}
