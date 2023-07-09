using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;


    private void Awake()
    {
        instance = this;
    }

    public int Timer;
    public int Counter;

    public int TotalCubes;
    public int CubeCollected;

    bool LevelFail;
    bool LevelWon;


    Coroutine TimerRun;

    private void Start()
    {
        TimerRun = StartCoroutine(CountTimer());
    }
    IEnumerator CountTimer()
    {
        Counter = Timer;
        while (Counter > 0)
        {
            Counter--;
            yield return new WaitForSeconds(1);
        }

        FailedLevel();
    }
    void FailedLevel()
    {
        LevelFail = true;
    }
    void WonLevel()
    {
        StopCoroutine(TimerRun);
        LevelWon = true;
    }
    public void AddCube()
    {
        CubeCollected++;
        if(CubeCollected >= TotalCubes)
        {
            WonLevel();
        }
    }
}
