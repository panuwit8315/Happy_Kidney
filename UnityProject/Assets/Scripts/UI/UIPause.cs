using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class UIPause : MonoBehaviour, IUI
{
    [SerializeField] Button closeBtn;
    [SerializeField] Button backToMenuBtn;
    [SerializeField] Button menuBtn;

    [SerializeField] AudioMixer audioMixerBG;
    [SerializeField] AudioMixer audioMixerSFX;
    [SerializeField] Slider AudioBGM;
    [SerializeField] Slider AudioSFX;

    
    int volumeMax = 0;
    int volumeMin = -80;

    public void Open()
    {
        audioMixerBG.GetFloat("volume", out float v);
        if (v <= -50)
        {
            AudioBGM.value = -80;        
        }

        Game game = Game.GetInstance();
        game.timer.SetRunTime(false);
        game.GetComponent<RayCastManager>().enabled = false;        

        if (GameObject.Find("Canvas/UILobby(Clone)") != null)
        {            
            menuBtn.gameObject.SetActive(true);
            closeBtn.gameObject.SetActive(false);
            backToMenuBtn.gameObject.SetActive(false);
            menuBtn?.onClick.AddListener(() =>
            {
                Close();
                UIManager.GetUI().OpenLobbyUI();
                game.GetComponent<RayCastManager>().enabled = true;
            });
        }

        else if (GameObject.Find("Canvas/UIGamePlay(Clone)") != null) 
        {
            menuBtn.gameObject.SetActive(false);
            closeBtn.gameObject.SetActive(true);
            backToMenuBtn.gameObject.SetActive(true);
            closeBtn?.onClick.AddListener(() =>
            {
                Close();
                game.timer.SetRunTime(true);
                game.GetComponent<RayCastManager>().enabled = true;
            });
            backToMenuBtn?.onClick.AddListener(() =>
            {
                Close();
                UIManager.GetUI().OpenLobbyUI();
                UIManager.GetUI().UIGamePlay().Close();
                game.fridgeSpawner.ClearFridge();
                game.GetComponent<RayCastManager>().enabled = true;
            });            
        }        
    }

    public void SetVolumeBGM()
    {
        //int volume = 0;
        audioMixerBG.SetFloat("volume", volumeMax);
        if (AudioBGM.value == 0)
        {
            AudioBGM.GetComponent<Animator>().SetTrigger("Close");
            //volumeMin;
            audioMixerBG.SetFloat("volume", volumeMin);
            Debug.Log(volumeMin);
        }

        else if (AudioBGM.value == -80)
        {
            AudioBGM.GetComponent<Animator>().SetTrigger("Open");
            //volume = 0;
            audioMixerBG.SetFloat("volume", volumeMax);
            Debug.Log(volumeMax);
        }        
    }

    public void SetVolumeSFX()
    {
        int volume = 0;
        audioMixerSFX.SetFloat("volume2", volume);
        Debug.Log(volume);

        if (AudioSFX.value == 0)
        {
            AudioSFX.GetComponent<Animator>().SetTrigger("Close");
            volume = -80;
        }

        else if (AudioSFX.value == -80)
        {
            AudioSFX.GetComponent<Animator>().SetTrigger("Open");
            volume = 0;
        }
    }

    public void Close()
    {
        GetComponent<Animator>().SetTrigger("MoveOut");
        Invoke("DestroyObj", 1);
    }

    public void DestroyObj()
    {
        UIManager.GetUI().CloseUI(this);
        Destroy(gameObject);
    }     
}
