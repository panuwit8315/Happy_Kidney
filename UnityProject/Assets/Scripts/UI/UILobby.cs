using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobby : MonoBehaviour
{
    [SerializeField] Button startBtn;
    [SerializeField] GameObject logo;

    Game game;

    public void Open()
    {
        game = Game.GetInstance();
        startBtn.onClick.AddListener(() =>
        {
            game.StartGame(PlayDifference.NORMAL);

            Close();
        });
    }

    public void Close()
    {
        GetComponent<Animator>().SetTrigger("MoveOut");
        Invoke("DestroyObj",3);
    }

    private void DestroyObj()
    {
        UIManager.GetUI().CloseLobbyUI();
        Destroy(gameObject);
    }
}
