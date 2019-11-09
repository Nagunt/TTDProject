using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTower_Cannon : MyTower
{
    private float delay = 0;
    private Monster target;

    [SerializeField]
    private Transform barrel;
    [SerializeField]
    private Transform bulletPos;
    [SerializeField]
    private GameObject bulletPrefab;

    public override void Attack()
    {
        if (target != null) StartCoroutine(_Attack(target));
    }

    private IEnumerator _Attack(Monster _target)
    {
        delay = 1f;
        GameObject newBullet = Instantiate(bulletPrefab);
        Vector3 firstPos = bulletPos.position;
        newBullet.transform.position = firstPos;

        float deltaTime = 0f;
        while (deltaTime < 0.05f)
        {
            yield return null;
            deltaTime += Time.deltaTime;
            if (_target == null) break;
            newBullet.transform.position = Vector3.Lerp(firstPos, _target.transform.position, deltaTime / 0.2f);
        }

        if(_target != null)
        {
            newBullet.transform.position = _target.transform.position;
            _target?.GetDamage(30);
        }

        Destroy(newBullet);
    }

    private void LookAt(Monster _target)
    {
        Vector3 targetPos = _target.transform.position;
        Vector3 dir = targetPos - barrel.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        barrel.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void Update()
    {
        if (delay > 0) delay -= Time.deltaTime;
        if (delay < 0) delay = 0;

        List<Collider2D> collider2Ds = new List<Collider2D>();
        ContactFilter2D contactFilter2D = new ContactFilter2D();
        contactFilter2D.SetLayerMask(LayerMask.GetMask("Monster"));
        if (Physics2D.OverlapCollider(area, contactFilter2D, collider2Ds) > 0)
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
            LookAt(target);
            if (delay == 0)
            {
                Attack();
            }
        }
        else
        {
            target = null;
            //barrel.Rotate(new Vector3(0, 0, 2f));
        }
    }

}
