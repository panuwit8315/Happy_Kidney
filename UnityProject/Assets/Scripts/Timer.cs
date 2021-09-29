using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    float timer = 121;
    public bool isRunTime = false;

    Game game;
    UIManager UI;

    private void Start()
    {
        game = Game.GetInstance();
        UI = UIManager.GetUI();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) //debug
        {
            timer = 5;
            print("R");
        }
        if (isRunTime && timer > 0)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                timer = 0;
                isRunTime = false;
                //do something
                EndGame();
            }

            //update UI timer
            UI.UIGamePlay().SetTimeUI(GetTimeStr());          
        }
    }

    public void AddTime(float second)
    {
        timer += second;
        UI.UIGamePlay().SetTimeUI(GetTimeStr());
    }

    public void SesetTime()
    {
        timer = 121;
        UI.UIGamePlay().SetTimeUI(GetTimeStr());
    }

    public void SetTime(float set)
    {
        timer = set;
        UI.UIGamePlay().SetTimeUI(GetTimeStr());
    }

    public void SetRunTime(bool set)
    {
        isRunTime = set;
    }

    private string GetTimeStr() //Convert time float to time 00:00 format
    {
        int minute = minute = (int)timer / 60;
        int secound = secound = (int)timer % 60;
        return minute.ToString("00") + ":" + secound.ToString("00");
    }

    void EndGame()
    {
        game.EndGame();
    }
}
