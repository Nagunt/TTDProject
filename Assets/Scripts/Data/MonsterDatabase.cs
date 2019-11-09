using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDatabase : ScriptableObject, IDatabase<MonsterData>
{
    public List<MonsterData> monsters = new List<MonsterData>();

    public void ClearDatabase()
    {
        monsters.Clear();
    }

    public void AddData(MonsterData data)
    {
        monsters.Add(data);
    }

    public MonsterData GetData(int id)
    {
        return monsters[id];
    }
}
