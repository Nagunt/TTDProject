using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance { get; private set; } = null;

    private void Awake()
    {
        Instance = this;
    }

    public MonsterDatabase database;
}
