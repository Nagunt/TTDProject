using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterData : ScriptableObject, IData
{
    public int id;
    public string monsterName;
    public string description;
    public int hp;
    public int speed;
    public int damage;

    public void SetData(Dictionary<string, object> parsedData)
    {
        id = (int)parsedData["id"];
        monsterName = (string)parsedData["name"];
        description = (string)parsedData["description"];
        hp = (int)parsedData["hp"];
        speed = (int)parsedData["speed"];
        damage = (int)parsedData["damage"];
    }
}
