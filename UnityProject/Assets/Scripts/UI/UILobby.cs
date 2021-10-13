using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobby : MonoBehaviour, IUI
{
    [SerializeField] Button startBtn;
    [SerializeField] Button settingBtn;    

    //[SerializeField] GameObject logo;

    Game game;    

    public void Open()
    {
        game = Game.GetInstance();
        startBtn?.onClick.AddListener(() =>
        {
            UIManager.GetUI().OpenSelectModeUI();
        });
        settingBtn?.onClick.AddListener(() =>
        {
            UIManager.GetUI().OpenPauseUI();            
        });
    }

    public void Close()
    {
        GetComponent<Animator>().SetTrigger("MoveOut");
        Invoke("DestroyObj",1);
    }

    public void DestroyObj()
    {
        UIManager.GetUI().CloseUI(this);
        Destroy(gameObject);
    }
}
