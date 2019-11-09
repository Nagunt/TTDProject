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

        transform.Translate(direction * spd * Time.deltaTime);
    }

    public void Move(Vector3 dir)
    {
        direction = dir;
    }

    public void Init(MonsterData _data, Vector3 dir)
    {
        data = _data;
        data.SetDifficulty(1f);

        direction = dir;

        if (data.sprite)
        {
            GetComponent<SpriteRenderer>().sprite = data.sprite;
        }
    }

    public void Init(MonsterData _data, Vector3 dir, float difficulty)
    {
        data = _data;
        data.SetDifficulty(difficulty);

        direction = dir;

        if (data.sprite)
        {
            GetComponent<SpriteRenderer>().sprite = data.sprite;
        }
    }
}
