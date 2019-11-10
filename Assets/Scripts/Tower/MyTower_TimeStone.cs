using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTower_TimeStone : MyTower
{
    private float delay = 0;
    [Header("- Attribute")]
    [SerializeField]
    private GameObject bulletPrefab;

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
        Physics2D.OverlapCollider(attackArea, contactFilter2D, collider2Ds);

        int shootCount = 0;
        for(int i = 0; i < collider2Ds.Count; ++i)
        {
            if (collider2Ds[i] == null) continue;
            if (shootCount >= 5) break;
            shootCount += 1;
            Monster target = collider2Ds[i].GetComponent<Monster>();
            StartCoroutine(Shoot(target));
            yield return new WaitForSeconds(0.1f);
        }

        IEnumerator Shoot(Monster _target)
        {
            GameObject newBullet = Instantiate(bulletPrefab, transform);
            Vector3 firstPos = transform.position;
            newBullet.transform.position = firstPos;
            newBullet.SetActive(true);

            float deltaTime = 0f;
            while (deltaTime < 0.5f)
            {
                yield return null;
                deltaTime += Time.deltaTime;
                if (_target == null) break;
                newBullet.transform.position = Vector3.Lerp(firstPos, _target.transform.position, deltaTime / 0.5f);
            }

            if(_target != null)
            {
                newBullet.transform.position = _target.transform.position;
                _target?.GetDamage(attack);
            }
            Destroy(newBullet);
        }
    }

    public override void Init()
    {
        attack = 7;
        attackDelay = 1.5f;
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
