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
    public UIGamePlay UIGamePlay() { if (uiGamePlay == null) OpenGamePlayUI(); return uiGamePlay; }
    public UILobby UILobby() { if (uiLobby == null) OpenLobbyUI(); return uiLobby; }
    public UIEndGame UIEndGame() { if (uiEndGame == null) OpenLobbyUI(); return uiEndGame; }
    
    public void OpenGamePlayUI()
    {
        GameObject g;
        if(GetComponentInChildren<UIGamePlay>() != null)
        {
            g = GetComponentInChildren<UIGamePlay>().gameObject;
        }
        else
        {
            g = SpawnObj("Prefabs/UI/UIGamePlay");
        }
        if (g == null) return;
        g.GetComponent<UIGamePlay>().Open();
        uiGamePlay = g.GetComponent<UIGamePlay>();
    }
    public void CloseGamePlayUI()
    {
        uiGamePlay = null;
    }

    public void OpenLobbyUI()
    {
        GameObject g;
        if (GetComponentInChildren<UILobby>() != null)
        {
            g = GetComponentInChildren<UILobby>().gameObject;
        }
        else
        {
            g = SpawnObj("Prefabs/UI/UILobby");
        }
        if (g == null) return;
        g.GetComponent<UILobby>().Open();
        uiLobby = g.GetComponent<UILobby>();
    }
    public void CloseLobbyUI()
    {
        uiLobby = null;
    }

    public void OpenEndGameUI()
    {
        GameObject g;
        if (GetComponentInChildren<UIEndGame>() != null)
        {
            g = GetComponentInChildren<UIEndGame>().gameObject;
        }
        else
        {
            g = SpawnObj("Prefabs/UI/UIEndGame");
        }
        if (g == null) return;
        g.GetComponent<UIEndGame>().Open();
        uiEndGame = g.GetComponent<UIEndGame>();
    }
    public void CloseEndGameUI()
    {
        uiEndGame = null;
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
