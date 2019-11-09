using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySpawner : MonoBehaviour
{
    Vector3 spawnPoint;

    private void Awake()
    {
        MyMovePoint prevPoint = null;
        for(int i = 0; i < transform.childCount; ++i)
        {
            if(prevPoint != null)
            {
                prevPoint.destination = transform.position;
            }
            prevPoint = transform.GetChild(i).gameObject.AddComponent<MyMovePoint>();
            prevPoint.isLast = i == transform.childCount - 1;

            if (i == 0) spawnPoint = prevPoint.transform.position;
        }
    }

    public void Spawn(int id)
    {

    }

    public void Spawn(int id, float difficulty)
    {

    }
}
