using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTower_Laser : MyTower
{
    private float delay = 0;
    [Header("- Attribute")]
    [SerializeField]
    private Transform barrel;
    [SerializeField]
    private SpriteRenderer laserEffect;

    public void SetAngle(float angle)
    {
        barrel.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public override void Attack()
    {
        StartCoroutine(_Attack());
    }

    private IEnumerator _Attack()
    {
        delay = attackDelay;

        yield return new WaitForSeconds(1f);

        laserEffect.enabled = true;
        laserEffect.color = new Color(laserEffect.color.r, laserEffect.color.g, laserEffect.color.b, 1);

        List<Collider2D> collider2Ds = new List<Collider2D>();
        ContactFilter2D contactFilter2D = new ContactFilter2D();
        contactFilter2D.SetLayerMask(LayerMask.GetMask("Monster"));
        if (Physics2D.OverlapCollider(attackArea, contactFilter2D, collider2Ds) > 0)
        {
            for(int i = 0; i < collider2Ds.Count; ++i)
            {
                Monster target = collider2Ds[i].GetComponent<Monster>();
                target?.GetDamage(attack);
            }
        }

        yield return new WaitForSeconds(0.25f);

        float deltaTime = 0f;
        while(deltaTime < 1f)
        {
            deltaTime += Time.deltaTime;
            laserEffect.color = new Color(laserEffect.color.r, laserEffect.color.g, laserEffect.color.b, 1 - deltaTime);
            yield return null;
        };
        laserEffect.color = new Color(laserEffect.color.r, laserEffect.color.g, laserEffect.color.b, 0);
        laserEffect.enabled = false;
    }

    public override void Init()
    {
        attack = 50;
        attackDelay = 5f;
        laserEffect.enabled = false;
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
