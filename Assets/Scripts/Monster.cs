using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    MonsterData data;
    Vector3 direction;
    Vector3 destVec;

    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (data.hp <= 0)
            DieCallback();

        float spd = (float)data.speed;

        if (direction != null)
            transform.Translate(direction * spd * Time.deltaTime);

        if ((destVec - transform.position).magnitude < 0.1f)
            transform.position = destVec;
    }

    void DieCallback()
    {
        Destroy(gameObject);
    }

    public void Move(Vector3 dest)
    {
        destVec = dest;

        Vector3 dir = dest - transform.position;
        dir.Normalize();

        direction = dir;

        LookAt(spriteRenderer.transform, dest);
    }

    private void LookAt(Transform tr, Vector3 dest)
    {
        Vector3 dir = dest - tr.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        tr.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void Init(MonsterData _data)
    {
        data = _data;
        data.SetDifficulty(1f);

        if (data.sprite)
        {
            spriteRenderer.sprite = data.sprite;
        }
    }

    public void Init(MonsterData _data, float difficulty)
    {
        data = _data;
        data.SetDifficulty(difficulty);

        if (data.sprite)
        {
            spriteRenderer.sprite = data.sprite;
        }
    }

    public void GetDamage(int damage)
    {
        data.hp -= damage;
    }
}
