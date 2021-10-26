using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    float timer = 120;
    public bool isRunTime = false;
    private bool isPlayTimeSfx = true;

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
            print("Press [R]: Set timer = 5"); //OnEditor
        }
        if (isRunTime && timer > 0)
        {
            timer -= Time.deltaTime;

            if(timer < 10f && isPlayTimeSfx)
            {
                UI.UIGamePlay().TimeAlert();
                SoundManager.GetInstance().PlaySFXLoop(SfxClipName.TIMELEFT);
                isPlayTimeSfx = false;
            }
            else if (timer <= 0)
            {
                timer = 0;
                isRunTime = false;
                SoundManager.GetInstance().StopSFXLoop();

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
        UI.UIGamePlay().SpawnAddTimeNotif("+" + second + " วินาที");
    }

    public void SesetTime()
    {
        timer = 120;
        isPlayTimeSfx = true;
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
        int minute = (int)timer / 60;
        int secound = (int)timer % 60;
        return minute.ToString("00") + ":" + secound.ToString("00");
    }

    void EndGame()
    {
        if(game.fridgeSpawner.GetFridgeCount() >= PlayerPrefs.GetInt("HighestFridgeLv", 0))
        {
            PlayerPrefs.SetInt("HighestFridgeLv", game.fridgeSpawner.GetFridgeCount());
        }
        game.GetComponent<RayCastManager>().Clear();
        game.EndGame();
    }
}
