using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    MonsterData data;
    Vector3 direction;

    private void Update()
    {
        int spd = data.speed;

        if (direction != null)
            transform.Translate(direction * spd * Time.deltaTime);
    }

    public void Move(Vector3 dest)
    {
        Vector3 dir = dest - transform.position;
        dir.Normalize();

        direction = dir;
    }

    public void Init(MonsterData _data)
    {
        data = _data;
        data.SetDifficulty(1f);

        if (data.sprite)
        {
            GetComponent<SpriteRenderer>().sprite = data.sprite;
        }
    }

    public void Init(MonsterData _data, float difficulty)
    {
        data = _data;
        data.SetDifficulty(difficulty);

        if (data.sprite)
        {
            GetComponent<SpriteRenderer>().sprite = data.sprite;
        }
    }

    public void GetDamage(int damage)
    {

    }
}
