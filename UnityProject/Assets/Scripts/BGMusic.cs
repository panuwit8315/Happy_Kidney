using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BGMusic : MonoBehaviour
{
    private static BGMusic bgMusic;

    //[SerializeField] AudioClip clickBT;
    //AudioSource audioSource;

    //void Start()
    //{
    //    audioSource = GetComponent<AudioSource>();
    //}

    public void Awake()
    {
        if (bgMusic == null)
        {
            bgMusic = this;
            DontDestroyOnLoad(bgMusic);
        }
        else
        {
            Destroy(gameObject);
        }


    }

    //public void ClickButton()
    //{
    //    audioSource.PlayOneShot(clickBT);
    //}
}
