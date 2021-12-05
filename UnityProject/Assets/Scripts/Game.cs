using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;
using SimpleJSON;

public class Game : MonoBehaviour
{
    private static Game game;
    public static Game GetInstance() { return game; }

    public FridgeSpawner fridgeSpawner;
    public RayCastManager rayCast;
    public DataManager dataManager;
    public Timer timer;
    public bool isTestting;
    public SpriteRenderer bg;

    UIManager ui;
    public SceneState scene;
    public PlayDifference diff; //use send to gameplayUI only

    private void Awake()
    {
        game = this;
        GetHighScore();
    }

    private void Start()
    {
        ui = UIManager.GetUI();
        if (PlayerPrefs.GetString("PlayerName", "UnkownName") == "UnkownName") //เข้าครั้งแรก ยังไม่ได้ตั้งชื่อ
        {
            ui.OpenInputNameUI();//print("เข้าครั้งแรก ยังไม่ได้ตั้งชื่อ");
        }
        else
        {
            ui.OpenLobbyUI(); //print("ไม่เข้าครั้งแรก ได้ตั้งชื่อแล้ว");
        }
    }

    public void StartGame(PlayDifference playDiff)
    {
        diff = playDiff;
        ui.OpenGamePlayUI();
        timer.SesetTime();
        timer.SetRunTime(true);       
        fridgeSpawner.ResetSpawner();
        fridgeSpawner.LetSpawner(diff);     
    }

    public void EndGame()
    {
        fridgeSpawner.ClearFridge();
        timer.SetRunTime(false);
        ui.UIGamePlay().Close();
        ui.OpenEndGameUI();
    }

    public void GetHighScore()
    {
        string urlData = $"{dataManager.url}/User/userData.json?auth={dataManager.secret}";
        string playerTag = PlayerPrefs.GetString("PlayerTag");
        User user = new User();
        user.userData = new List<User.UserData>();

        RestClient.Get(urlData).Then(response =>
        {
            Debug.Log(response.Text);
            JSONNode jsonNode = JSONNode.Parse(response.Text);

            for (int i = 0; i < jsonNode.Count; i++)
            {
                user.userData.Add(new User.UserData(jsonNode[i]["name"], jsonNode[i]["score"], jsonNode[i]["tag"]));
            }

            for (int i = 0; i < user.userData.Count; i++)
            {
                if (user.userData[i].tag == playerTag)
                {
                    PlayerPrefs.SetInt("PlayerHighScore", user.userData[i].score);
                    UILobby uILobby = ui.UILobby();
                    if (uILobby != null) uILobby.RefreshHighScore();
                    print("Save high score from server");
                }
            }

            Debug.Log("GetData" + user.userData.Count);

        }).Catch(error =>
        {
            Debug.Log("GetHighScore Not Found Data");           
        });
    }
}

public enum SceneState {LOBBY, GAMEPLAY }
