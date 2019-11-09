using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTower_Cannon : MyTower
{
    private Transform barrel;

    private void Update()
    {
        List<Collider2D> collider2Ds = new List<Collider2D>();
        ContactFilter2D contactFilter2D = new ContactFilter2D();
        contactFilter2D.SetLayerMask(LayerMask.GetMask("Monster"));
        if (Physics2D.OverlapCollider(area, contactFilter2D, collider2Ds) > 0)
        {
            
        }

    }

}
