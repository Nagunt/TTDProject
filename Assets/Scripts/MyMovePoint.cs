﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMovePoint : MonoBehaviour
{
    public bool isLast;
    public Vector3 destination;

    private void Update()
    {
        if (isLast == false)
        {
            Collider2D[] colliderArray = Physics2D.OverlapPointAll(transform.position);
            if (colliderArray.Length > 0)
            {
                for(int i = 0; i < colliderArray.Length; ++i)
                {
                    if (colliderArray[i].transform.position == transform.position)
                    {
                        Monster target = colliderArray[i].GetComponent<Monster>();
                        target?.Move(destination);
                    }
                }
            }
        }
    }

}
