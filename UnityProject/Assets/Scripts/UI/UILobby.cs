using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobby : MonoBehaviour, IUI
{
    [SerializeField] Button startBtn;
    [SerializeField] Button settingBtn;    
    [SerializeField] Text nameTx;    

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

        string playerName = PlayerPrefs.GetString("PlayerName");
        nameTx.text = "ยินดีต้อนรับ\nคุณ " + playerName;
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
