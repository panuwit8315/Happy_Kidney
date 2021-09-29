using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEndGame : MonoBehaviour
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
        Invoke("DestroyObj", 3);
    }

    private void DestroyObj()
    {
        UIManager.GetUI().CloseEndGameUI();
        Destroy(gameObject);
    }
}
