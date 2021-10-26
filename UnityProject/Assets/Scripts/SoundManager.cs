using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SfxClipName {COMBO, PICKOUTCORRECT01, PICKOUTCORRECT02, PICKOUTFAIL01, PICKOUTFAIL02, TIMEOUT, FRIDGECOMPLETE, CLICK01, CLICK02, ENDGAME, TIMELEFT }

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager GetInstance() { return instance; }
    AudioSource bgmTrack01, bgmTrack02;
    AudioSource sfxTrack01, sfxTrack02;

    public AudioMixerGroup bgmMixer;
    public AudioMixerGroup sfxMixer;

    bool isPlayingBgmTrack01;
    [SerializeField] AudioClip defaultClip;
    [SerializeField] AudioClip[] clipsSFX;
    [SerializeField] AudioClip[] clipsTest;
    [SerializeField] int countClipsTest = 0;

    int volumeSfxOn = 0;
    [SerializeField]int volumeBgmOn = 0;
    int volumeOff = -80;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        bgmTrack01 = gameObject.AddComponent<AudioSource>();
        bgmTrack02 = gameObject.AddComponent<AudioSource>();
        sfxTrack01 = gameObject.AddComponent<AudioSource>();
        sfxTrack02 = gameObject.AddComponent<AudioSource>();
        
        bgmTrack01.outputAudioMixerGroup = bgmMixer; bgmTrack01.loop = true;
        bgmTrack02.outputAudioMixerGroup = bgmMixer; bgmTrack02.loop = true;
        sfxTrack01.outputAudioMixerGroup = sfxMixer; //For OneShot
        sfxTrack02.outputAudioMixerGroup = sfxMixer; sfxTrack02.loop = true; //For Loop

        isPlayingBgmTrack01 = true;
        ChangeBGMClip(defaultClip);

        if (!IsOnBGMSettingInSave()) SwitchOnBGMMixer(false);
        else SwitchOnBGMMixer(true);
        if (!IsOnSFXSettingInSave()) SwitchOnSFXMixer(false);
        else SwitchOnSFXMixer(true);
    }

    public void ChangeBGMClip(AudioClip newClip)
    {
        StopAllCoroutines();
        StartCoroutine(FadeTrack(newClip));

        isPlayingBgmTrack01 = !isPlayingBgmTrack01;
    }

    private IEnumerator FadeTrack(AudioClip clip)
    {
        float timeToFade = 1.25f;
        float timeElapsed = 0;

        if (isPlayingBgmTrack01)
        {
            bgmTrack02.clip = clip;
            bgmTrack02.Play();

            while (timeElapsed < timeToFade)
            {
                bgmTrack02.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                bgmTrack01.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            bgmTrack01.Stop();
        }
        else
        {
            bgmTrack01.clip = clip;
            bgmTrack01.Play();

            while (timeElapsed < timeToFade)
            {
                bgmTrack01.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                bgmTrack02.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            bgmTrack02.Stop();
        }
    }

    public void SwitchOnBGMMixer(bool isOn)
    {
        int volume;
        if (isOn) volume = volumeBgmOn;
        else volume = volumeOff;

        bgmMixer.audioMixer.SetFloat("volume", volume);
        PlayerPrefs.SetInt("BgmVolume", volume);
        DebugCtrl.GetDebug().Log("SoundMN-->Set BGM:" + volume);
    }

    public void SwitchOnSFXMixer(bool isOn)
    {
        int volume;
        if (isOn) volume = volumeSfxOn;
        else volume = volumeOff;

        sfxMixer.audioMixer.SetFloat("volume", volume);
        PlayerPrefs.SetInt("SfxVolume", volume);
        DebugCtrl.GetDebug().Log("SoundMN-->Set SFX:" + volume);
    }

    public bool IsOnBGMSettingInSave()
    {
        if (PlayerPrefs.GetInt("BgmVolume", volumeBgmOn) == volumeBgmOn) return true;
        else return false;
    }

    public bool IsOnSFXSettingInSave()
    {
        if (PlayerPrefs.GetInt("SfxVolume", volumeSfxOn) == volumeSfxOn) return true;
        else return false;
    }

    public void PlaySFXOneShot(SfxClipName clipName)
    {
        sfxTrack01.PlayOneShot(clipsSFX[(int)clipName]);
    }
    public void PlaySFXLoop(SfxClipName clipName)
    {
        sfxTrack02.clip = clipsSFX[(int)clipName];
        sfxTrack02.Play();
    }
    public void StopSFXLoop()
    {
        if(sfxTrack02.isPlaying) sfxTrack02.Stop();
    }

    //For Test Only
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            countClipsTest++;
            if (countClipsTest >= clipsTest.Length) countClipsTest = 0;
            ChangeBGMClip(clipsTest[countClipsTest]);
        }
    }
    //End For Test Only
}
