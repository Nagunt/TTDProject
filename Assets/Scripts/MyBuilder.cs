using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyBuilder : MonoBehaviour
{
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Transform towerParent;

    private IEnumerator buildRoutine;

    public void Build(MyTower tower)
    {
        if (buildRoutine == null)
        {
            if (TimeManager.Instance.time >= tower.cost)
            {
                buildRoutine = BuildRoutine(tower);
                StartCoroutine(buildRoutine);
            }
        }
    }

    public void Remove()
    {
        if(buildRoutine == null)
        {
            buildRoutine = RemoveRoutine();
            StartCoroutine(buildRoutine);
        }
    }

    private IEnumerator RemoveRoutine()
    {
        bool isEnd = false;
        while (isEnd == false)
        {
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false);
            Collider2D[] colliderArray = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), LayerMask.GetMask("BuildArea"));
            if(colliderArray.Length > 0)
            {
                MyTower target = colliderArray[0].GetComponent<MyTower>();
                target?.Remove();
                isEnd = true;
            }
        }
        buildRoutine = null;
    }

    private IEnumerator BuildRoutine(MyTower tower)
    {
        WaitUntil uiCheck = new WaitUntil(() => EventSystem.current.IsPointerOverGameObject() == false);
        yield return uiCheck;
        MyTower newTower = Instantiate(tower, towerParent);
        bool isBuild = false;
        float angle = 0f;
        newTower.ShowBuildUI();
        while(isBuild == false)
        {
            yield return uiCheck;
            newTower.transform.localPosition = grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if(newTower.GetType() == typeof(MyTower_Laser))
            {
                if (Input.GetKeyDown(KeyCode.LeftBracket))
                {
                    angle += 90f;
                    if (angle < 0) angle += 360f;
                    if (angle >= 360) angle -= 360f;
                    MyTower_Laser laserTower = (MyTower_Laser)newTower;
                    laserTower.SetAngle(angle);
                }
                if (Input.GetKeyDown(KeyCode.RightBracket))
                {
                    angle -= 90f;
                    if (angle < 0) angle += 360f;
                    if (angle >= 360) angle -= 360f;
                    MyTower_Laser laserTower = (MyTower_Laser)newTower;
                    laserTower.SetAngle(angle);
                }
            }
            if (newTower.CheckArea())
            {
                if (Input.GetMouseButton(0))
                {
                    isBuild = true;
                    break;
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                break;
            }
            if(TimeManager.Instance.time < newTower.cost)
            {
                break;
            }
            yield return null;
        }

        if (isBuild)
        {
            newTower.HideBuildUI();
            newTower.Init();
            TimeManager.Instance.time -= newTower.cost;
        }
        else
        {
            Destroy(newTower.gameObject);
        }
        buildRoutine = null;
    }

}
