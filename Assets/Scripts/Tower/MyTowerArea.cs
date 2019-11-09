using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTowerArea : MonoBehaviour
{
    MyTower tower;

    private void Awake()
    {
        tower = GetComponentInParent<MyTower>();
    }
}
