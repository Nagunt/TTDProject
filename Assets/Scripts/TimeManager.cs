using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; } = null;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameStart();
    }

    private float _time;
    public float time
    {
        get
        {
            return _time;
        }

        set
        {
            _time = value;

            timeText.text = ((int)_time).ToString();

            if (_time <= 0f)
            {
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        MonsterManager.Instance.GameOver();

        var monsters = FindObjectsOfType<Monster>();

        foreach(var m in monsters)
        {
            Destroy(m.gameObject);
        }

        var towers = FindObjectsOfType<MyTower>();

        foreach(var t in towers)
        {
            Destroy(t.gameObject);
        }
        SceneManager.LoadScene("Scene_Main");
    }

    public void GameStart()
    {
        _time = 300f;
    }

    private void Update()
    {
        time -= Time.deltaTime;
    }

    public Text timeText;
}
