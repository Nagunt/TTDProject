using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTower : MonoBehaviour
{
    [Header("- UI")]
    [SerializeField]
    protected SpriteRenderer buildUI;
    [Header("- Area")]
    [SerializeField]
    protected Collider2D buildArea;
    [SerializeField]
    protected Collider2D attackArea;

    protected float attackDelay;
    protected int attack;

    private IEnumerator uiRoutine;

    public virtual void Init()
    {

    }

    public virtual void Attack()
    {

    }

    public virtual void Remove()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }
    
    public void ShowBuildUI()
    {
        buildUI.enabled = true;
        if(uiRoutine == null)
        {
            uiRoutine = UIRoutine();
            StartCoroutine(uiRoutine);
        }
    }

    public void HideBuildUI()
    {
        buildUI.enabled = false;
        if(uiRoutine != null)
        {
            StopCoroutine(uiRoutine);
            uiRoutine = null;
        }
    }

    private IEnumerator UIRoutine()
    {
        while (buildUI.enabled)
        {
            yield return null;
            buildUI.color = CheckArea() ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);
        }
    }

    public bool CheckArea()
    {
        ContactFilter2D contactFilter2D = new ContactFilter2D();
        contactFilter2D.SetLayerMask(LayerMask.GetMask("Terrain", "BuildArea"));
        return Physics2D.OverlapCollider(buildArea, contactFilter2D, new List<Collider2D>()) <= 0;
    }


}
