using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTower : MonoBehaviour
{
    [Header("- Area")]
    [SerializeField]
    protected Collider2D buildArea;
    [SerializeField]
    protected Collider2D attackArea;

    protected float attackDelay;
    protected int attack;

    public virtual void Init()
    {

    }

    public virtual void Attack()
    {

    }

    public virtual void Remove()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }

    public bool CheckArea()
    {
        List<Collider2D> collider2Ds = new List<Collider2D>();
        ContactFilter2D contactFilter2D = new ContactFilter2D();
        contactFilter2D.SetLayerMask(LayerMask.GetMask("Terrain", "Tower"));
        return Physics2D.OverlapCollider(attackArea, contactFilter2D, collider2Ds) == 0;
    }
}
