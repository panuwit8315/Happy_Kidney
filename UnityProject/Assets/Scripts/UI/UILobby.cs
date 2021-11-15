using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobby : MonoBehaviour, IUI
{
    [SerializeField] Button startBtn;
    [SerializeField] Button settingBtn;    
    [SerializeField] Button leaderbordBtn;    
    [SerializeField] Button shopBtn;    
    [SerializeField] Button infoBtn;    
    [SerializeField] Text nameTx;
    [SerializeField] Text coinTx;
    [SerializeField] Text highScoreTx;

    //UILeaderboard uiLeaderboard;
    //[SerializeField] GameObject logo;

    Game game;

    public void Open()
    {
        game = Game.GetInstance();
        SoundManager sound = SoundManager.GetInstance();
        game.scene = SceneState.LOBBY;
        startBtn?.onClick.AddListener(() =>
        {
            sound.PlaySFXOneShot(SfxClipName.CLICK02);
            UIManager.GetUI().OpenSelectModeUI();
        });
        settingBtn?.onClick.AddListener(() =>
        {
            sound.PlaySFXOneShot(SfxClipName.CLICK02);
            UIManager.GetUI().OpenPauseUI();            
        });
        leaderbordBtn?.onClick.AddListener(() =>
        {
            sound.PlaySFXOneShot(SfxClipName.CLICK02);
            UIManager.GetUI().OpenLeaderboardUI();
        });

        //string playerName = PlayerPrefs.GetString("PlayerName");
        nameTx.text = PlayerPrefs.GetString("PlayerName");
        coinTx.text = PlayerPrefs.GetInt("PlayerCoin",0).ToString();
        highScoreTx.text = PlayerPrefs.GetInt("PlayerHighScore",0).ToString();

        //uiLeaderboard.SetData();
    }

    public void RefreshHighScore()
    {
        highScoreTx.text = PlayerPrefs.GetInt("PlayerHighScore", 0).ToString();
    }

    public void Close()
    {
        GetComponent<Animator>().SetTrigger("MoveOut");
        Invoke("DestroyObj",0.5f);
    }

    public void DestroyObj()
    {
        UIManager.GetUI().CloseUI(this);
        Destroy(gameObject);
    }
}
