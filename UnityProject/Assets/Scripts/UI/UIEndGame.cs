using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class UIEndGame : MonoBehaviour, IUI
{
    [SerializeField] Button close;
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
        sound.PlaySFXOneShot(SfxClipName.TIMEOUT);
        int score = game.fridgeSpawner.GetScore();
        int fakeCoin = score / 100;
        scoreText.text = score.ToString("0");
        coinText.text = fakeCoin.ToString("0");
        fridgeText.text = game.fridgeSpawner.GetFridgeCount().ToString("0");                
        ingredientText.text = game.fridgeSpawner.GetIngredientCount().ToString("0");
        close.onClick.AddListener(() =>
        {
            ui.OpenThrowAwayUI();
            sound.PlaySFXOneShot(SfxClipName.CLICK02);
            close.gameObject.SetActive(false);
            //close this ui
            Close();
        });

        //เช็คว่าปัดไปกี่ตู้ใน UnityDashboard
        AnalyticsResult analyticsResult = Analytics.CustomEvent("FridgeCount", new Dictionary<string, object> { { "Fridge", game.fridgeSpawner.GetFridgeCount() } });
        Debug.Log("analyticResult(FridgeCount): " + analyticsResult);
        Debug.Log(game.fridgeSpawner.GetFridgeCount());
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
