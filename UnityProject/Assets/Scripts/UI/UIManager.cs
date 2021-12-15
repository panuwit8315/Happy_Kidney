using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public static UIManager GetUI(){ return instance; }

    private System.DateTime startTime, stopTime;

    void Awake() 
    { 
        instance = this;
        foreach(Transform t in transform.GetComponentInChildren<Transform>())
        {
            if (t != this.transform) Destroy(t.gameObject);
        }
    }

    private UIGamePlay uiGamePlay;
    private UILobby uiLobby;
    private UIEndGame uiEndGame;
    private UISelectMode uiSelectMode;
    private UIHint uiHint;
    private UILeaderboard uiLeaderboard;
    private UIPause uiPause;
    private UITutorial uiTutorial;
    private UIThrowAway uiThrowAway;
    private UIInputName uiInputName;
    private UIInfo uiInfo;
    public UIGamePlay UIGamePlay() { return uiGamePlay; } //Game.GetInstance().fridgeSpawner.diff
    public UILobby UILobby() { return uiLobby; }
    public UIEndGame UIEndGame() { return uiEndGame; }
    public UISelectMode UISelectMode() { if (uiSelectMode == null) OpenSelectModeUI(); return uiSelectMode; }
    public UIHint UIHint() {  return uiHint; }
    public UILeaderboard UILeaderboard() { if (uiLeaderboard == null) OpenLeaderboardUI(); return uiLeaderboard; }
    public UIPause UIPause() { if (uiPause == null) OpenPauseUI(); return uiPause; }
    public UITutorial UITutorial() { return uiTutorial; }
    public UIThrowAway UIThrowAway() { return uiThrowAway; }
    public UIInputName UIInputName() { return uiInputName; }
    public UIInfo UIInfo() { return uiInfo; }

    public void OpenGamePlayUI()//PlayDifference currentDiff)
    {
        if (GetComponentInChildren<UIGamePlay>() != null) return;

        GameObject g = SpawnObj("Prefabs/UI/UIGamePlay");
        if (g == null) return;
        g.GetComponent<UIGamePlay>().Open();// currentDiff);
        uiGamePlay = g.GetComponent<UIGamePlay>();
    }
    public void CloseUI(UIGamePlay ui)
    {
        uiGamePlay = null;
    }

    public void OpenLobbyUI()
    {
        //if (GetComponentInChildren<UILobby>() != null) return;

        GameObject g = SpawnObj("Prefabs/UI/UILobby");
        if (g == null) return;
        g.GetComponent<UILobby>().Open();
        uiLobby = g.GetComponent<UILobby>();
    }
    public void CloseUI(UILobby ui)
    {
        uiLobby = null;
    }

    public void OpenEndGameUI()
    {
        if (GetComponentInChildren<UIEndGame>() != null) return;

        GameObject g = SpawnObj("Prefabs/UI/UIEndGame");
        if (g == null) return;       
        uiEndGame = g.GetComponent<UIEndGame>();
        g.GetComponent<UIEndGame>().Open();
    }
    public void CloseUI(UIEndGame ui)
    {
        uiEndGame = null;
    }

    public void OpenSelectModeUI()
    {      
        if (GetComponentInChildren<UISelectMode>() != null) return;

        GameObject g = SpawnObj("Prefabs/UI/UISelectMode");
        if (g == null) return;
        g.GetComponent<UISelectMode>().Open();
        uiSelectMode = g.GetComponent<UISelectMode>();
    }
    public void CloseUI(UISelectMode ui)
    {
        uiSelectMode = null;
    }

    public void OpenHintUI()
    {
        if (GetComponentInChildren<UIHint>() != null) return;

        GameObject g = SpawnObj("Prefabs/UI/UIHint");
        if (g == null) return;
        g.GetComponent<UIHint>().Open();
        uiHint = g.GetComponent<UIHint>();

        //เริ่มนับเวลา
        //startTime = System.DateTime.UtcNow;
    }
    public void CloseUI(UIHint ui)
    {
        uiHint = null;
        /*
         *
        //ทำให้หยุดนับเวลาและเซ็ตไว้ในค่า ts
        stopTime = System.DateTime.UtcNow;
        System.TimeSpan ts = stopTime - startTime;

        //เช็คว่าเปิดดูปุ่ม Hint กี่วิใน UnityDashboard
        AnalyticsResult analyticsResult = Analytics.CustomEvent("OpenHint", new Dictionary<string, object> { { "Seconds", ts.Seconds.ToString() } });
        Debug.Log("analyticResult(OpenHint): " + analyticsResult);
        //Debug.Log(ts);
        *
        */
    }

    public void OpenLeaderboardUI()
    {
        if (GetComponentInChildren<UILeaderboard>() != null) return;

        GameObject g = SpawnObj("Prefabs/UI/UILeaderboard");
        if (g == null) return;
        g.GetComponent<UILeaderboard>().Open();
        uiLeaderboard = g.GetComponent<UILeaderboard>();
    }
    public void CloseUI(UILeaderboard ui)
    {
        uiLeaderboard = null;
    }

    public void OpenPauseUI()
    {
        if (GetComponentInChildren<UIPause>() != null) return;

        GameObject g = SpawnObj("Prefabs/UI/UIPause");
        if (g == null) return;
        g.GetComponent<UIPause>().Open();
        uiPause = g.GetComponent<UIPause>();
    }
    public void CloseUI(UIPause ui)
    {
        uiPause = null;
    }

    public void OpenTutorialUI()
    {
        if (GetComponentInChildren<UITutorial>() != null) return;

        GameObject g = SpawnObj("Prefabs/UI/UITutorial");
        if (g == null) return;
        uiTutorial = g.GetComponent<UITutorial>();
        g.GetComponent<UITutorial>().Open();
        Game.GetInstance().timer.isRunTime = false;
    }
    public void CloseUI(UITutorial ui)
    {
        uiTutorial = null;
    }

    public void OpenThrowAwayUI()
    {
        if (GetComponentInChildren<UIThrowAway>() != null) return;

        GameObject g = SpawnObj("Prefabs/UI/UIThrowAway");
        if (g == null) return;
        g.GetComponent<UIThrowAway>().Open();
        uiThrowAway = g.GetComponent<UIThrowAway>();
    }
    public void CloseUI(UIThrowAway ui)
    {
        uiThrowAway = null;
    }

    public void OpenInputNameUI()
    {
        //if (GetComponentInChildren<UIInputName>() != null) return;

        GameObject g = SpawnObj("Prefabs/UI/UIInputName"); //print("OpenInputNameUI");
        if (g == null) return;
        g.GetComponent<UIInputName>().Open();
        uiInputName = g.GetComponent<UIInputName>();
    }
    public void CloseUI(UIInputName ui)
    {
        uiInputName = null;
    }

    public void OpenInfoUI()
    {
        GameObject g = SpawnObj("Prefabs/UI/UIInfo");
        if (g == null) return;
        g.GetComponent<UIInfo>().Open();
        uiInfo = g.GetComponent<UIInfo>();
    }
    public void CloseUI(UIInfo ui)
    {
        uiInfo = null;
    }

    private GameObject SpawnObj(string path)
    {
        GameObject prefab = Resources.Load(path) as GameObject;
        if(prefab == null)
        {
            Debug.LogError("ไม่มี ["+ path + "] ในโฟลเดอร์ Resouce");
            return null;
        }
        GameObject g = Instantiate(prefab, transform);
        return g;
    }
}
