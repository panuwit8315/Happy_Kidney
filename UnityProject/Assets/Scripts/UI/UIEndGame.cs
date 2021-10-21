using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEndGame : MonoBehaviour, IUI
{
    [SerializeField] Text scoreText;
    [SerializeField] Text coinText;
    [SerializeField] Text fridgeText;
    [SerializeField] Text ingredientText;
    SoundManager sound;
    UIManager ui;

    public void Open()
    {
        Game game = Game.GetInstance();
        sound = SoundManager.GetInstance();
        ui = UIManager.GetUI();
        ui.UIGamePlay().MoveOutBin();
        if (ui.UIHint() != null) ui.UIHint().Close();
        //sound.PlaySFXOneShot(SfxClipName.ENDGAME);
        int score = game.fridgeSpawner.GetScore();
        int fakeCoin = score / 100;
        scoreText.text = score.ToString("0");
        coinText.text = fakeCoin.ToString("0");
        fridgeText.text = game.fridgeSpawner.GetFridgeCount().ToString("0");
        ingredientText.text = game.fridgeSpawner.GetIngredientCount().ToString("0");
    }

    public void OnRestartBtn()
    {
        Game game = Game.GetInstance();
        sound.PlaySFXOneShot(SfxClipName.CLICK02);
        game.StartGame(game.fridgeSpawner.diff);
        //close this ui
        Close();
    }

    public void OnBackMenuBtn()
    {
        ui.OpenLobbyUI();
        sound.PlaySFXOneShot(SfxClipName.CLICK02);
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
        ui.CloseUI(this);
        Destroy(gameObject);
    }
}
