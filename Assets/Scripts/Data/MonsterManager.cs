using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance { get; private set; } = null;

    private void Awake()
    {
        Instance = this;

        Init();
    }

    public MonsterDatabase database;
    public MySpawner[] spawner;

    //Wave, Turn, spawner/monsterId/delay
    Dictionary<int, Dictionary<int, List<int>>> waves;

    private void Init()
    {
        using (StreamReader reader = new StreamReader("Assets/Resources/Database/CSVFiles/waveManagement.csv"))
        {
            waves = new Dictionary<int, Dictionary<int, List<int>>>();
            maxTurn = new Dictionary<int, int>();

            string nextSign = "#Next";
            string endSign = "#End";

            int wave = 1;
            int turn = 1;

            bool isEnd = false;

            while(true)
            {
                Dictionary<int, List<int>> waveInfo = new Dictionary<int, List<int>>();
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

                    waveInfo.Add(turn, turnInfo);

                    turn++;
                }
                maxTurn.Add(wave, turn);

                if (isEnd) break;

                waves.Add(wave, waveInfo);

                wave++;

                reader.BaseStream.Position = 0;
            }

            maxWave = wave;
        }
    }

    int maxWave;
    Dictionary<int, int> maxTurn;           
    IEnumerator GameRoutine()
    {
        int wave = 1;
        int turn = 1;

        int spawnerId = 0;
        int monsterId = 0;
        int spawnDelay = 0;

        while(true)
        {
            while(true)
            {
                //탈출 조건 만들어야 함
                List<int> turnInfo = waves[wave][turn];

                spawnerId = turnInfo[0];
                monsterId = turnInfo[1];
                spawnDelay = turnInfo[2];

                spawner[spawnerId].Spawn(database.GetData(monsterId));

                yield return new WaitForSeconds(spawnDelay);
            }
            //이어서 만들어야 함

        }
    }
}
