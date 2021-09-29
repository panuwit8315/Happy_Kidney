using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public static Game game;
    public static Game GetInstance(){return game;}

    public FridgeSpawner fridgeSpawner;
    public DataManager dataManager;
    public Timer timer;
    public bool isTestting;

    UIManager ui;
   
    private void Awake()
    {
        game = this;
    }

    private void Start()
    {
        ui = UIManager.GetUI();
        ui.OpenLobbyUI();
    }

    public void StartGame(PlayDifference playDiff)
    {
        ui.OpenGamePlayUI();
        timer.SesetTime();
        timer.SetRunTime(true);
        fridgeSpawner.ResetSpawner();
        fridgeSpawner.LetSpawner(playDiff);    
    }

    public void EndGame()
    {
        fridgeSpawner.ClearFridge();
        timer.SetRunTime(false);
        ui.UIGamePlay().Close();
        ui.OpenEndGameUI();
    }
}
