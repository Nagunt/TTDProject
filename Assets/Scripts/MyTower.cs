using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTower : MonoBehaviour
{
    protected float delay;

    private void Start()
    {
        delay = 0;
    }

    private void Update()
    {
        if (delay > 0) delay -= Time.deltaTime;
    }

    public virtual void Attack()
    {


    }

    public virtual void Remove()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }
}
