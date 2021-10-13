using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePlay : MonoBehaviour, IUI
{
    [SerializeField] Text scoreText;
    [SerializeField] Text timeText;
    [SerializeField] Text fridgeText;
    [SerializeField] LevelUnlockItem lvUnlockItem;
    [SerializeField] Slider comboSlider;
    [SerializeField] Animator rubbishBinAnim;
    [SerializeField] GameObject[] itemsObj;
    [SerializeField] GameObject bonusPanel;
    [SerializeField] GameObject addScoreNoftPos;
    [SerializeField] GameObject addTimeNotfPos;
    [SerializeField] Button pauseBtn;
    bool nextSpawnScoreNotf = true;
    List<string> scoreQ = new List<string>();    

    public void Open()
    {
        pauseBtn?.onClick.AddListener(() =>
        {
            UIManager.GetUI().OpenPauseUI();
        });

        PlayDifference currentDiff = Game.GetInstance().fridgeSpawner.diff;print(currentDiff);
        if (currentDiff == PlayDifference.NORMAL)
        {
            lvUnlockItem = Game.GetInstance().dataManager.GetLevelUnlockItem();
        }
        else
        {
            lvUnlockItem = new LevelUnlockItem(1, 1, 1);
            print("OpenUIGamPlay"+currentDiff);
        }
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

    public void SetTimeUI(string timeStr)
    {
        timeText.text = timeStr;
    }

    public void SetScoreUI(int score, string textToShow = null)
    {
        scoreText.text = score.ToString();
        if (!string.IsNullOrEmpty(textToShow)) //SpawnScoreNotif(textToShow);
        {
            scoreQ.Add(textToShow);
            SpawnScoreNotif();
        }
    }

    public void SetFridgeUI(int fridgeCount)
    {
        fridgeText.text = fridgeCount.ToString();
        SetActiveItemByFridgeLv(fridgeCount);
    }

    void SetActiveItemByFridgeLv(int fridgeLv)
    {
        if(fridgeLv == lvUnlockItem.unlockFire)
        {
            itemsObj[0].GetComponent<ItemBtn>().EnableItem();
        }
        if (fridgeLv == lvUnlockItem.unlockBroom)
        {
            itemsObj[1].GetComponent<ItemBtn>().EnableItem();
        }
        if (fridgeLv == lvUnlockItem.unlockCrowbar)
        {
            itemsObj[2].GetComponent<ItemBtn>().EnableItem();
        }
    }

    public void MoveInRubbishBin()
    {
        rubbishBinAnim.SetTrigger("MoveIn");
    }

    public void MoveOutRubbishBin()
    {
        rubbishBinAnim.SetTrigger("MoveOut");
    }

    public void EnableColAllItem(bool set)
    {
        foreach(GameObject item in itemsObj)
        {
            item.GetComponent<CircleCollider2D>().enabled = set;
        }
    }

    public void DisableColOtherItem(string name)
    {
        foreach(GameObject item in itemsObj)
        {
            if (name == item.name) continue;

            item.GetComponent<CircleCollider2D>().enabled = false;
        }
    }

    public void SetSliderValue(int value)
    {
        comboSlider.value = value;
    }

    public void SetSliderMaxValue(int value)
    {
        comboSlider.maxValue = value;
    }

    public void SetBonus(BonusType bonus)
    {
        bonusPanel.GetComponentInChildren<Text>().text = "โบนัส x" + (int)bonus;
        bonusPanel.SetActive((int)bonus != 1);
    }

    private void SpawnScoreNotif()
    {
        if (scoreQ.Count < 1 || !nextSpawnScoreNotf) return; //|| currentScoreNotf.Count > 1

        nextSpawnScoreNotf = false;
        GameObject g = Instantiate(addScoreNoftPos, addScoreNoftPos.GetComponentInParent<RectTransform>());
        g.SetActive(true);
        g.GetComponent<Text>().text = scoreQ[0];
        scoreQ.RemoveAt(0);
        Invoke("ScoreNotfDelay", 0.4f);
        Invoke("SpawnScoreNotif", 0.5f);     
    }

    void ScoreNotfDelay()
    {
        nextSpawnScoreNotf = true;
    }

    public void SpawnAddTimeNotif(string text)
    {        
        GameObject g = Instantiate(addTimeNotfPos, addTimeNotfPos.GetComponentInParent<RectTransform>());
        g.SetActive(true);
        g.GetComponent<Text>().text = text;
        Destroy(g, 1f);
    }
}
