using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelectMode : MonoBehaviour, IUI
{
    [SerializeField] Button closeBtn;
    [SerializeField] Button easyBtn;
    [SerializeField] Button hardBtn;
    public void Open()
    {
        Game game = Game.GetInstance();
        closeBtn?.onClick.AddListener(() =>
        {
            Close();
        });
        easyBtn?.onClick.AddListener(() =>
        {
            game.StartGame(PlayDifference.NORMAL);
            UIManager.GetUI().UILobby().Close();
            Close();
        });
        hardBtn?.onClick.AddListener(() =>
        {
            game.StartGame(PlayDifference.HARD);
            UIManager.GetUI().UILobby().Close();
            Close();
        });
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
