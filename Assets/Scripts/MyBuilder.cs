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

    public MyTower test;
    private IEnumerator buildRoutine;

    public void Test()
    {
        if (buildRoutine == null)
        {
            buildRoutine = BuildRoutine(test);
            StartCoroutine(buildRoutine);
        }
    }

    private IEnumerator BuildRoutine(MyTower tower)
    {
        WaitUntil uiCheck = new WaitUntil(() => EventSystem.current.IsPointerOverGameObject() == false);
        yield return uiCheck;
        MyTower newTower = Instantiate(tower, towerParent);
        bool isBuild = false;
        newTower.ShowBuildUI();
        while(isBuild == false)
        {
            yield return uiCheck;
            newTower.transform.localPosition = grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

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
            yield return null;
        }

        if (isBuild)
        {
            newTower.HideBuildUI();
            newTower.Init();
        }
        else
        {
            Destroy(newTower.gameObject);
        }
        buildRoutine = null;
    }

}
