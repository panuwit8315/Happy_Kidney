using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeSpawner : MonoBehaviour
{
    [SerializeField] GameObject fridgePrefab;
    [SerializeField] Transform spawnPos;
    [SerializeField] int fridgeCount = 0;
    [SerializeField] int IngredientCount = 0;
    [SerializeField] int score = 0;
    [SerializeField] int combo = 0;
    [SerializeField] int maxCombo = 0;
    [SerializeField] int stepComboComplete = 0;
    [SerializeField] BonusType bonusType = BonusType.X1;
    public PlayDifference diff;

    public GameObject currentFridgeObj;
    public List<IngredientData> allCurrentIngredientNotEat = new List<IngredientData>();

    //Game game;
    UIManager UI;
    DataManager Data;
    SoundManager sound;

    [Header("Setup Score")]
    [SerializeField] int correctIngredient;
    [SerializeField] int incorrectIngredient;
    [SerializeField] int correctItem;
    [SerializeField] int incorrectItem;

    private void Start()
    {
        //game = Game.GetInstance();
        UI = UIManager.GetUI();
        Data = Game.GetInstance().dataManager;
        sound = SoundManager.GetInstance();
    }
    public void LetSpawner()
    {
        LetSpawner(diff);
        //Invoke("DelayFridgeSFX", 0.025f);
    }
    void DelayFridgeSFX()
    {
        //sound.PlaySFXOneShot(SfxClipName.FRIDGECOMPLETE);
    }

    public void LetSpawner(PlayDifference diff)
    {
        this.diff = diff;print("Spawn Set Diff");
        GameObject g;
        if (currentFridgeObj == null) g = Instantiate(fridgePrefab, spawnPos.position, spawnPos.rotation);
        else g = currentFridgeObj;
        currentFridgeObj = g;
        fridgeCount++;
        UI.UIGamePlay().SetFridgeUI(fridgeCount);
        g.GetComponent<Fridge>().Setup(fridgeCount,this.diff);
    }

    public void AddDataIngredientNotEat(IngredientData ingredientData)
    {
        if(allCurrentIngredientNotEat.Contains(ingredientData)) return;
        allCurrentIngredientNotEat.Add(ingredientData);
        UI.UIGamePlay().SetHintNoft(true);
    }

    public int GetFridgeCount()
    {
        return fridgeCount;
    }
    public void SetFridgeCount(int set)
    {
        fridgeCount = set;
    }

    public int GetIngredientCount()
    {
        return IngredientCount;
    }
    public void AddIngredientCount(int add)
    {
        IngredientCount += add;
    }

    public void ResetSpawner()
    {
        fridgeCount = 0;
        IngredientCount = 0;
        score = 0;
        combo = 0;
        stepComboComplete = 0;
        SetNewMaxCombo();
        SetBonus(BonusType.X1);
        UI.UIGamePlay().SetScoreUI(score);
        UI.UIGamePlay().SetSliderValue(combo);
        allCurrentIngredientNotEat.Clear();
    }

    void AddScore(int add, bool useBonus = false)
    {
        int total = add;
        if (add > 0 && useBonus) total = add * (int)bonusType;
        score += total;
        if (score < 0) score = 0;
        string showStr = "";
        if (total > 0) showStr = "+" + total;
        else if (total < 0) showStr = total.ToString();
        UI.UIGamePlay().SetScoreUI(score, showStr);
    }

    public void AddScoreAfterDropIngredient(IngredientType type)
    {
        if (type == IngredientType.CAN_EAT)
        {
            AddScore(incorrectIngredient);
            FailCombo();
        }
        else if (type == IngredientType.SHOULD_NOT_EAT)
        {
            AddScore(correctIngredient, true);
            Combo();
        }
    }

    public void AddScoreAfterUseItem(bool isCorrect)
    {
        if (isCorrect) AddScore(correctItem, true);
        else AddScore(incorrectItem);
    }

    public Fridge GetFridgeObj()
    {
        return currentFridgeObj.GetComponent<Fridge>();
    }

    void Combo()
    {
        combo++;
        UI.UIGamePlay().SetSliderValue(combo);
        if(combo >= maxCombo)
        {
            CompleteCombo();
            sound.PlaySFXOneShot(SfxClipName.COMBO);
        }
        else
        {
            sound.PlaySFXOneShot(SfxClipName.PICKOUTCORRECT02);
        }
    }

    void FailCombo()
    {
        combo = 0;
        sound.PlaySFXOneShot(SfxClipName.PICKOUTFAIL01);
        stepComboComplete = 0;
        SetNewMaxCombo();
        SetBonus(BonusType.X1);
        UI.UIGamePlay().SetSliderValue(combo);
        UI.UIGamePlay().ShowRedNoft();
    }

    void SetNewMaxCombo()
    {
        ComboDifference diff = Data.GetComboDiff(stepComboComplete);
        maxCombo = diff.maxCombo;
        UI.UIGamePlay().SetSliderMaxValue(maxCombo);
    }

    void CompleteCombo()
    {
        combo = 0;              
        UI.UIGamePlay().SetSliderValue(combo);
        //Action
        ComboDifference comboDiff = Data.GetComboDiff(stepComboComplete);
        Game.GetInstance().timer.AddTime(comboDiff.addTime);
        AddScore(comboDiff.addScore);
        SetBonus(comboDiff.bonusType);
        //Invoke("DefaultBonus", 5);

        stepComboComplete++;
        SetNewMaxCombo();
    }

    void SetBonus(BonusType bonus)
    {
        bonusType = bonus;
        UI.UIGamePlay().SetBonus(bonus);
    }


    public int GetScore()
    {
        return score;
    }

    public void ClearFridge()
    {
        if (currentFridgeObj != null)
        {
            currentFridgeObj.GetComponent<Fridge>().nextSpawn = false;
            currentFridgeObj.GetComponent<Fridge>().Out();
        }
    }
}
