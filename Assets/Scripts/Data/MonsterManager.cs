﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEditor;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance { get; private set; } = null;

    private void Awake()
    {
        Instance = this;
        database = Resources.Load<MonsterDatabase>("Database/monsterDB");


        Init();
    }

    public GameObject monsterPrefab;
    public static MonsterDatabase database;
    public MySpawner[] spawner;

    public Text text;

    public Monster InstantiateMonster()
    {
        Monster monster = Instantiate(monsterPrefab).GetComponent<Monster>();
        return monster;
    }

    //Wave, Turn, spawner/monsterId/delay
    Dictionary<int, Dictionary<int, List<int>>> waves;

    [MenuItem("Database/Build Monster DB")]
    public static void BuildDB()
    {
        database = Resources.Load<MonsterDatabase>("Database/monsterDB");

        Dictionary<int, Dictionary<int, List<int>>> waves = new Dictionary<int, Dictionary<int, List<int>>>();
        Dictionary<int, int> maxTurn = new Dictionary<int, int>();
        using (StreamReader reader = new StreamReader("Assets/Resources/Database/CSVFiles/waveManagement.csv"))
        {
            string nextSign = "#Next";
            string endSign = "#End";

            int wave = 1;
            int turn = 1;

            bool isEnd = false;

            while (true)
            {
                Dictionary<int, List<int>> waveInfo = new Dictionary<int, List<int>>();

                turn = 1;

                while (true)
                {
                    string readLine = reader.ReadLine();
                    int offset = (wave - 1) * 3;

                    string[] splitedLine = readLine.Split(',');

                    if (splitedLine[offset].Equals(endSign))
                    {
                        isEnd = true;
                        break;
                    }
                    else if (splitedLine[offset].Equals(nextSign))
                    {
                        break;
                    }

                    List<int> turnInfo = new List<int>();
                    turnInfo.Add(int.Parse(splitedLine[offset]));               //spawnerId
                    turnInfo.Add(int.Parse(splitedLine[offset + 1]));           //monsterId
                    turnInfo.Add(int.Parse(splitedLine[offset + 2]));           //delay

                    Debug.Log($"wave {wave} turn {turn} : {turnInfo[0]}, {turnInfo[1]}, {turnInfo[2]}");

                    waveInfo.Add(turn, turnInfo);

                    turn++;
                }
                turn--;
                maxTurn.Add(wave, turn);

                waves.Add(wave, waveInfo);

                if (isEnd) break;

                wave++;

                reader.BaseStream.Position = 0;
            }

            database.maxWave = wave;
        }

        database.waves = waves;
        database.maxTurn = maxTurn;
    }

    private void Init()
    {
        
        StartCoroutine(GameRoutine()); //임시
    }

    int maxWave;
    Dictionary<int, int> maxTurn;           
    IEnumerator GameRoutine()
    {
        maxWave = database.maxWave;
        maxTurn = database.maxTurn;
        waves = database.waves;

        yield return new WaitForSeconds(1f);

        int wave = 1;
        int turn = 1;

        int spawnerId = 0;
        int monsterId = 0;
        int spawnDelay = 0;

        while(true)
        {
            Debug.Log($"Now Wave : {wave}");

            if (wave > maxWave)
            {
                wave = 1;
            }

            turn = 1;

            while(true)
            {
                if (turn > maxTurn[wave])
                {
                    break;
                }
                Debug.Log($"turn : {turn} / maxTurn : {maxTurn[wave]}");

                List<int> turnInfo = waves[wave][turn];

                spawnerId = turnInfo[0];
                monsterId = turnInfo[1];
                spawnDelay = turnInfo[2];

                spawner[spawnerId].Spawn(database.GetData(monsterId));

                yield return new WaitForSeconds(spawnDelay);

                turn++;
            }

            wave++;
            TimeManager.Instance.time += 100;

            yield return null;
            //yield return WaitUntil 준비 기간 동안 정지
        }
    }

    public void GameOver()
    {
        StopAllCoroutines();
    }

    public void GameStart()
    {
        Init();
    }
}
