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

    public Sprite sprite;

    public MonsterData(MonsterData data)
    {
        id = data.id;
        monsterName = data.monsterName;
        description = data.description;
        hp = data.hp;
        speed = data.speed;
        damage = data.damage;

        sprite = data.sprite;
    }

    public void SetData(Dictionary<string, object> parsedData)
    {
        id = (int)parsedData["id"];
        monsterName = (string)parsedData["name"];
        description = (string)parsedData["description"];
        hp = (int)parsedData["hp"];
        speed = (int)parsedData["speed"];
        damage = (int)parsedData["damage"];

        string spriteName = (string)parsedData["sprite"];
        sprite = Resources.Load<Sprite>($"Sprites/{spriteName}");
    }

    public void SetDifficulty(float difficulty)
    {
        hp = (int)(hp * difficulty);
        damage = (int)(damage * difficulty);
    }
}
