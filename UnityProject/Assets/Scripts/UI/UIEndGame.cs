using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEndGame : MonoBehaviour, IUI
{
    [SerializeField] Text scoreText;

    public void Open()
    {
        scoreText.text = Game.GetInstance().fridgeSpawner.GetScore().ToString("0");
    }

    public void OnRestartBtn()
    {
        Game game = Game.GetInstance();
        game.StartGame(game.fridgeSpawner.diff);
        //close this ui
        Close();
    }

    public void OnBackMenuBtn()
    {
        UIManager.GetUI().OpenLobbyUI();
        //close this ui
        Close();
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
