using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    MonsterData data;
    Vector3 direction;
    Vector3 destVec;

    SpriteRenderer spriteRenderer;

    private void Update()
    {
        float spd = (float)data.speed;

        if (direction != null)
            transform.Translate(direction * spd * Time.deltaTime);

        if ((destVec - transform.position).magnitude < 0.1f)
            transform.position = destVec;
    }

    public void Move(Vector3 dest)
    {
        destVec = dest;

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
            spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = data.sprite;
        }
    }

    public void Init(MonsterData _data, float difficulty)
    {
        data = _data;
        data.SetDifficulty(difficulty);

        if (data.sprite)
        {
            spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = data.sprite;
        }
    }

    public void GetDamage(int damage)
    {

    }
}
