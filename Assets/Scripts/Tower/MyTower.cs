using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTower : MonoBehaviour
{
    [Header("- Attack Area")]
    [SerializeField]
    protected Collider2D area;

    protected float attackDelay;
    protected int attack;

    public virtual void Attack()
    {

    }

    public virtual void Remove()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }
}
