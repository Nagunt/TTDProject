using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGameManager : MonoBehaviour
{
    public int wave;
    public float difficulty;
    public MySpawner[] spawners;
    public int time;

    // Start is called before the first frame update
    void Start()
    {
        wave = 1;
        time = 300;
        StartCoroutine(Routine());
        StartCoroutine(TimeRoutine());
    }

    IEnumerator Routine()
    {
        int timeWave;
        switch (wave)   // 웨이브 별 구현
        {
            case 1:
                {
                    timeWave = 200;
                    while(time > 0 && timeWave > 0)
                    {
                        for(int i = 0; i < spawners.Length; ++i)
                        {
                            //스포너
                            spawners[i].Spawn(1);
                        }
                        yield return new WaitForSeconds(1f);
                        timeWave--;
                    }
                    
                    break;
                }
        }
    }
    
    IEnumerator TimeRoutine()
    {
        WaitForSeconds waitOneSecond = new WaitForSeconds(1f);
        while(time > 0)
        {
            yield return waitOneSecond;
            time--;
        }
    }
}
