using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTower_Thunder : MyTower
{
    private float delay = 0;
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
        List<Collider2D> collider2Ds = new List<Collider2D>();
        ContactFilter2D contactFilter2D = new ContactFilter2D();
        contactFilter2D.SetLayerMask(LayerMask.GetMask("Monster"));
        if (Physics2D.OverlapCollider(attackArea, contactFilter2D, collider2Ds) > 0)
        {
            Monster target = collider2Ds[0].GetComponent<Monster>();
            target?.GetDamage(attack);

            thunderEffect.enabled = true;
            thunderEffect.positionCount = 2;
            thunderEffect.SetPositions(new Vector3[] { transform.position, target.transform.position });
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
                if (delay <= 0)
                {
                    Attack();
                }
            }
            yield return 0;
        }
    }
}
