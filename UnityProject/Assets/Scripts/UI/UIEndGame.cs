using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class UIEndGame : MonoBehaviour, IUI
{
    public Button close;
    [SerializeField] Text scoreText;
    [SerializeField] Text coinText;
    [SerializeField] Text fridgeText;
    [SerializeField] Text ingredientText;
    [SerializeField] Text bonusText;

    [Header("Mask To Tutorial UI")]
    public GameObject mask_coin_UI;
    public GameObject mask_fridge_UI;
    public GameObject mask_ingre_UI;

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
        int bonus = PlayerPrefs.GetInt("FurnitureBonus", 0);
        bonusText.text = "คะแนน +" + bonus + "%";
        score = (score / 100) * (100+bonus);
        game.fridgeSpawner.SetScore(score);
        int coin = score / 100;
        scoreText.text = score.ToString("0");
        coinText.text = coin.ToString("0");
        fridgeText.text = game.fridgeSpawner.GetFridgeCount().ToString("0");                
        ingredientText.text = game.fridgeSpawner.GetIngredientCount().ToString("0");

        int myCoin = PlayerPrefs.GetInt("PlayerCoin", 0);
        myCoin += coin;
        PlayerPrefs.SetInt("PlayerCoin", myCoin);

        int myHighScore = PlayerPrefs.GetInt("PlayerHighScore", 0);
        if(score > myHighScore) myHighScore = score;
        PlayerPrefs.SetInt("PlayerHighScore", myHighScore);

        close.onClick.AddListener(() =>
        {
            ui.OpenThrowAwayUI();
            sound.PlaySFXOneShot(SfxClipName.CLICK02);
            close.gameObject.SetActive(false);
            //close this ui
            Close();
        });

        //OpenTutorialUI
        if (game.fridgeSpawner.diff == PlayDifference.NORMAL && PlayerPrefs.GetString("TutorialComplete", "No") != "Yes") //OpenTutorialUI
        {
            PlayerPrefs.SetInt("TutorialStep", 10);
            ui.OpenTutorialUI();
            close.enabled = false;
        }

        //เช็คว่าปัดไปกี่ตู้ใน UnityDashboard
        AnalyticsResult analyticsResult = Analytics.CustomEvent("FridgeCount", new Dictionary<string, object> { { "Fridge", game.fridgeSpawner.GetFridgeCount() } });
        Debug.Log("analyticResult(FridgeCount): " + analyticsResult);
        //Debug.Log(game.fridgeSpawner.GetFridgeCount());
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
