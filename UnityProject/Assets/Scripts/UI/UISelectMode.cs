using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelectMode : MonoBehaviour, IUI
{
    [SerializeField] Button closeBtn;
    [SerializeField] Button easyBtn;
    [SerializeField] Button hardBtn;
    [SerializeField] GameObject lockHardModeTx;
    bool isBtnAlredy = true;
    public void Open()
    {
        Game game = Game.GetInstance();
        SoundManager sound = SoundManager.GetInstance();
        closeBtn?.onClick.AddListener(() =>
        {
            sound.PlaySFXOneShot(SfxClipName.CLICK02);
            Close();
        });
        easyBtn?.onClick.AddListener(() =>
        {
            if (isBtnAlredy)
            {
                isBtnAlredy = false;
                sound.PlaySFXOneShot(SfxClipName.CLICK02);
                game.StartGame(PlayDifference.NORMAL);
                UIManager.GetUI().UILobby().Close();
                Close();
            }            
        });

        int highestFridgeLv = PlayerPrefs.GetInt("HighestFridgeLv",0);
        if (highestFridgeLv > 25)
        {
            lockHardModeTx.SetActive(false);
            hardBtn?.onClick.AddListener(() =>
            {
                if (isBtnAlredy)
                {
                    isBtnAlredy = false;
                    sound.PlaySFXOneShot(SfxClipName.CLICK02);
                    game.StartGame(PlayDifference.HARD);
                    UIManager.GetUI().UILobby().Close();
                    Close();
                }
            });
        }
        else
        {
            hardBtn.interactable = false;
            lockHardModeTx.SetActive(true);
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
