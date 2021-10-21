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
