using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTower_Cannon : MyTower
{
    private float delay = 0;
    private Monster target;
    [Header("- Attribute")]
    [SerializeField]
    private Transform barrel;
    [SerializeField]
    private Transform bulletPos;
    [SerializeField]
    private GameObject bulletEffect;
    [SerializeField]
    private GameObject bulletPrefab;

    public override void Attack()
    {
        if (target != null) StartCoroutine(_Attack(target));
    }

    private IEnumerator _Attack(Monster _target)
    {
        delay = attackDelay;

        GameObject newBullet = Instantiate(bulletPrefab);
        Vector3 firstPos = bulletPos.position;
        newBullet.transform.position = firstPos;
        bulletEffect.SetActive(true);
        newBullet.SetActive(true);

        float deltaTime = 0f;
        while (deltaTime < 0.1f)
        {
            yield return null;
            deltaTime += Time.deltaTime;
            if (_target == null) break;
            newBullet.transform.position = Vector3.Lerp(firstPos, _target.transform.position, deltaTime / 0.2f);
            LookAt(newBullet.transform, _target);
        }

        if(_target != null)
        {
            newBullet.transform.position = _target.transform.position;
            _target?.GetDamage(attack);
            // 몹을 느려지게 하는 코드 삽입.
        }
        bulletEffect.SetActive(false);
        Destroy(newBullet);
    }

    private void LookAt(Transform tr, Monster _target)
    {
        Vector3 targetPos = _target.transform.position;
        Vector3 dir = targetPos - tr.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        tr.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public override void Init()
    {
        attack = 30;
        attackDelay = 1f;
        StartCoroutine(Routine());
    }

    private IEnumerator Routine()
    {
        if (delay > 0) delay -= Time.deltaTime;
        if (delay < 0) delay = 0;

        List<Collider2D> collider2Ds = new List<Collider2D>();
        ContactFilter2D contactFilter2D = new ContactFilter2D();
        contactFilter2D.SetLayerMask(LayerMask.GetMask("Monster"));
        if (Physics2D.OverlapCollider(attackArea, contactFilter2D, collider2Ds) > 0)
        {
            if(target == null)
            {
                target = collider2Ds[0].GetComponent<Monster>();
            }
            else
            {
                bool isTarget = false;
                // 타겟이 범위 안에 계속 존재하면 유지한다.
                for(int i = 0; i < collider2Ds.Count; ++i)
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
            LookAt(barrel, target);
            if (delay <= 0)
            {
                Attack();
            }
        }
        else
        {
            target = null;
            //barrel.Rotate(new Vector3(0, 0, 2f));
        }
        yield return 0;
    }

}
