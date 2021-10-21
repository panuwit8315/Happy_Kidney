using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPause : MonoBehaviour, IUI
{
    [SerializeField] Button closeBtn;
    [SerializeField] Button backToMenuBtn;
    [SerializeField] Button menuBtn;

    public Slider bgmSlider;
    public Slider sfxSlider;

    SoundManager sound;

    public void Open()
    {
        Game game = Game.GetInstance();
        sound = SoundManager.GetInstance();
        sound.bgmMixer.audioMixer.GetFloat("volume", out float bgm);
        if (bgm < -70) 
        {
            bgmSlider.GetComponent<Animator>().SetTrigger("Close");
            bgmSlider.value = -80;
            //DebugCtrl.GetDebug().Log("UIPauseOpen-->BGM:OFF");
        } 
        else 
        {
            bgmSlider.GetComponent<Animator>().SetTrigger("Open");
            bgmSlider.value = 0;
            //DebugCtrl.GetDebug().Log("UIPauseOpen-->BGM:ON");
        } 
        sound.sfxMixer.audioMixer.GetFloat("volume", out float sfx);
        if (sfx < -70)
        {
            sfxSlider.GetComponent<Animator>().SetTrigger("Close");
            sfxSlider.value = -80;
            //DebugCtrl.GetDebug().Log("UIPauseOpen-->SFX:OFF");
        }
        else
        {
            sfxSlider.GetComponent<Animator>().SetTrigger("Open");
            sfxSlider.value = 0;
            //DebugCtrl.GetDebug().Log("UIPauseOpen-->SFX:ON");
        }

        if (game.scene == SceneState.LOBBY)
        {            
            menuBtn.gameObject.SetActive(true);
            closeBtn.gameObject.SetActive(false);
            backToMenuBtn.gameObject.SetActive(false);
            menuBtn?.onClick.AddListener(() =>
            {
                Close();
                sound.PlaySFXOneShot(SfxClipName.CLICK02);
                UIManager.GetUI().OpenLobbyUI();
            });
        }
        else if (game.scene == SceneState.GAMEPLAY)
        {
            game.timer.SetRunTime(false);
            game.GetComponent<RayCastManager>().enabled = false;
            menuBtn.gameObject.SetActive(false);
            closeBtn.gameObject.SetActive(true);
            backToMenuBtn.gameObject.SetActive(true);
            closeBtn?.onClick.AddListener(() =>
            {
                Close();
                sound.PlaySFXOneShot(SfxClipName.CLICK02);
                game.timer.SetRunTime(true);
                game.GetComponent<RayCastManager>().enabled = true;
            });
            backToMenuBtn?.onClick.AddListener(() =>
            {
                Close();
                sound.PlaySFXOneShot(SfxClipName.CLICK02);
                UIManager.GetUI().OpenLobbyUI();
                UIManager.GetUI().UIGamePlay().Close();
                game.fridgeSpawner.ClearFridge();
                game.GetComponent<RayCastManager>().enabled = true;
            });            
        }        
    }

    public void SetVolumeBGM()
    {
        sound.PlaySFXOneShot(SfxClipName.CLICK02);
        if (sound.IsOnBGMSettingInSave())
        {
            bgmSlider.GetComponent<Animator>().SetTrigger("Close");
            sound.SwitchOnBGMMixer(false);
        }
        else
        {
            bgmSlider.GetComponent<Animator>().SetTrigger("Open");
            sound.SwitchOnBGMMixer(true);
        }        
    }

    public void SetVolumeSFX()
    {
        sound.PlaySFXOneShot(SfxClipName.CLICK02);
        if (sound.IsOnSFXSettingInSave())
        {
            sfxSlider.GetComponent<Animator>().SetTrigger("Close");
            sound.SwitchOnSFXMixer(false);
        }
        else
        {
            sfxSlider.GetComponent<Animator>().SetTrigger("Open");
            sound.SwitchOnSFXMixer(true);
        }
    }

    public void Close()
    {
        GetComponent<Animator>().SetTrigger("MoveOut");
        Invoke("DestroyObj", 0.5f);
    }

    public void DestroyObj()
    {
        UIManager.GetUI().CloseUI(this);
        Destroy(gameObject);
    }     
}
