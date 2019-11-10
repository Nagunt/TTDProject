using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTower_Fire : MyTower
{
    private float delay = 0;
    [Header("- Attribute")]
    [SerializeField]
    private SpriteRenderer explodeEffect;

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
            for (int i = 0; i < collider2Ds.Count; ++i)
            {
                Monster target = collider2Ds[i].GetComponent<Monster>();
                target?.GetDamage(attack);
            }
        }
        explodeEffect.enabled = true;
        explodeEffect.color = new Color(explodeEffect.color.r, explodeEffect.color.g, explodeEffect.color.b, 1);

        yield return new WaitForSeconds(0.1f);
        float deltaTime = 0f;
        while (deltaTime < 0.5f)
        {
            deltaTime += Time.deltaTime;
            explodeEffect.color = new Color(explodeEffect.color.r, explodeEffect.color.g, explodeEffect.color.b, 1 - (deltaTime * 2f));
            yield return null;
        };

        explodeEffect.color = new Color(explodeEffect.color.r, explodeEffect.color.g, explodeEffect.color.b, 0);
        explodeEffect.enabled = false;
    }


    public override void Init()
    {
        attack = 30;
        attackDelay = 2f;
        explodeEffect.enabled = false;
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
