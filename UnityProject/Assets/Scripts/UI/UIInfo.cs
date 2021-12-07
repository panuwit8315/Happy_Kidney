using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInfo : MonoBehaviour, IUI
{
    public Button closeBtn;
    [SerializeField] GameObject[] contents;
    [SerializeField] Button leftBtn, rightBtn;
    [SerializeField] int currentTab;

    [Header("Link Button")]
    [SerializeField] Button ramaLinkBtn;
    [SerializeField] Button itiLinkBtn;
    
    //Game game;
    SoundManager sound;

    public void Open()
    {
        //game = Game.GetInstance();
        sound = SoundManager.GetInstance();
        closeBtn.onClick.AddListener(() =>
        {
            sound.PlaySFXOneShot(SfxClipName.CLICK02);
            Close();
        });
        ramaLinkBtn.onClick.AddListener(() =>
        {
            sound.PlaySFXOneShot(SfxClipName.CLICK02);
            Application.OpenURL("https://www.rama.mahidol.ac.th/rama_hospital/th/services/knowledge");
        });
        itiLinkBtn.onClick.AddListener(() =>
        {
            sound.PlaySFXOneShot(SfxClipName.CLICK02);
            Application.OpenURL("https://www.bu.ac.th/th/it-innovation");
        });

        OpenTab(0);
    }

    void OpenTab(int tabIndex)
    {
        currentTab = tabIndex;print(" OpenTab(int tabIndex=" + tabIndex +")");
        for (int i = 0; i < contents.Length; i++)
        {
            contents[i].SetActive(i == currentTab);
            print("SetActive contents[" + i + "] = "+ (i == currentTab));
        }

        //SetUp LeftButton
        if (currentTab > 0)
        {
            leftBtn.interactable = true;
            leftBtn.onClick.RemoveAllListeners();
            leftBtn.onClick.AddListener(() =>
            {
                sound.PlaySFXOneShot(SfxClipName.CLICK02);
                OpenTab(currentTab - 1);
            });
        }
        else
        {
            leftBtn.interactable = false;
        }

        //SetUp RightButton
        if (currentTab < (contents.Length-1))
        {
            rightBtn.interactable = true;
            rightBtn.onClick.RemoveAllListeners();
            rightBtn.onClick.AddListener(() =>
            {
                sound.PlaySFXOneShot(SfxClipName.CLICK02);
                OpenTab(currentTab + 1);
            });
        }
        else
        {
            rightBtn.interactable = false;
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
