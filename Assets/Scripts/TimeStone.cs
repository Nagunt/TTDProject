using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStone : MonoBehaviour
{
    public Collider2D detectCol;

    private void Update()
    {
        List<Collider2D> collider2Ds = new List<Collider2D>();
        ContactFilter2D contactFilter2D = new ContactFilter2D();
        contactFilter2D.SetLayerMask(LayerMask.GetMask("Monster"));
        if (Physics2D.OverlapCollider(detectCol, contactFilter2D, collider2Ds) > 0)
        {
            foreach (var col in collider2Ds)
            {
                Monster monster = col.GetComponent<Monster>();

                TimeManager.Instance.time -= monster.data.damage;

                Destroy(monster.gameObject);
            }
        }
    }
}