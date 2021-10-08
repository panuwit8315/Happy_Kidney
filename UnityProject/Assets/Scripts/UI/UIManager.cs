using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public static UIManager GetUI(){ return instance; }
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
    public UIGamePlay UIGamePlay() { if (uiGamePlay == null) OpenGamePlayUI(); return uiGamePlay; } //Game.GetInstance().fridgeSpawner.diff
    public UILobby UILobby() { if (uiLobby == null) OpenLobbyUI(); return uiLobby; }
    public UIEndGame UIEndGame() { if (uiEndGame == null) OpenEndGameUI(); return uiEndGame; }
    public UISelectMode UISelectMode() { if (uiSelectMode == null) OpenSelectModeUI(); return uiSelectMode; }
    public UIHint UIHint() { if (uiHint == null) OpenHinsUI(); return uiHint; }
    public UILeaderboard UILeaderboard() { if (uiLeaderboard == null) OpenLeaderboardUI(); return uiLeaderboard; }
    public UIPause UIPause() { if (uiPause == null) OpenPauseUI(); return uiPause; }
    public UITutorial UITutorial() { if (uiTutorial == null) OpenTutorialUI(); return uiTutorial; }

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
        if (GetComponentInChildren<UILobby>() != null) return;

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
        g.GetComponent<UIEndGame>().Open();
        uiEndGame = g.GetComponent<UIEndGame>();
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

    public void OpenHinsUI()
    {
        if (GetComponentInChildren<UIHint>() != null) return;

        GameObject g = SpawnObj("Prefabs/UI/UIHins");
        if (g == null) return;
        g.GetComponent<UIHint>().Open();
        uiHint = g.GetComponent<UIHint>();
    }
    public void CloseUI(UIHint ui)
    {
        uiHint = null;
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
        g.GetComponent<UITutorial>().Open();
        uiTutorial = g.GetComponent<UITutorial>();
    }
    public void CloseUI(UITutorial ui)
    {
        uiTutorial = null;
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
