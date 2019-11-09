using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTower : MonoBehaviour
{
    [SerializeField]
    protected Collider2D area;

    public virtual void Attack()
    {

    }

    public virtual void Remove()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }
}
