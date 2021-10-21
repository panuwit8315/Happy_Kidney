﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    private static Game game;
    public static Game GetInstance(){return game;}

    public FridgeSpawner fridgeSpawner;
    public RayCastManager rayCast;
    public DataManager dataManager;
    public Timer timer;
    public bool isTestting;

    UIManager ui;
    public SceneState scene;
    public PlayDifference diff; //use send to gameplayUI only
   
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
        diff = playDiff;
        timer.SesetTime();
        timer.SetRunTime(true);
        fridgeSpawner.ResetSpawner();
        fridgeSpawner.LetSpawner(diff);
        ui.OpenGamePlayUI();// playDiff);
    }

    public void EndGame()
    {
        fridgeSpawner.ClearFridge();
        timer.SetRunTime(false);
        ui.UIGamePlay().Close();
        ui.OpenEndGameUI();
    }
}

public enum SceneState {LOBBY, GAMEPLAY }
