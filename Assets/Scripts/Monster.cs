using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    MonsterData data;
    Vector3 direction;
    Vector3 destVec;

    public SpriteRenderer spriteRenderer;
    public Image hp_bar;
    public Image hp_bar_unfilled;
    bool isDamaged = false;

    float max_hp;

    private void Awake()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isDamaged)
        {
            hp_bar.fillAmount = (float)data.hp / max_hp;
        }

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

        max_hp = data.hp;

        if (data.sprite)
        {
            spriteRenderer.sprite = data.sprite;
        }
    }

    public void Init(MonsterData _data, float difficulty)
    {
        data = _data;
        data.SetDifficulty(difficulty);

        max_hp = data.hp;

        if (data.sprite)
        {
            spriteRenderer.sprite = data.sprite;
        }
    }

    public void GetDamage(int damage)
    {
        data.hp -= damage;

        if (!isDamaged)
        {
            isDamaged = true;
            hp_bar.gameObject.SetActive(true);
            hp_bar_unfilled.gameObject.SetActive(true);
        }
    }

    public void GetDamageByTime(int damage, float time)
    {
        StartCoroutine(DotDamage(damage, time));
    }

    IEnumerator DotDamage(int damage, float time)
    {
        float timer = 0f;
        while(timer <= time)
        {
            timer += 0.1f;

            float percentage = timer / time;
            int dotDamage = (int)(damage * percentage);

            GetDamage(dotDamage);

            yield return new WaitForSeconds(0.1f);
        }
    }
}
