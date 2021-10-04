using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePlay : MonoBehaviour, IUI
{
    [SerializeField] Text scoreText;
    [SerializeField] Text timeText;
    [SerializeField] Text fridgeText;
    [SerializeField] Slider comboSlider;
    [SerializeField] Animator rubbishBinAnim;
    [SerializeField] GameObject[] itemsObj;
    [SerializeField] GameObject bonusPanel;
    [SerializeField] GameObject spawnScorePos;
    [SerializeField] Button pauseBtn;
    GameObject currentSpawnScore;
    List<string> scoreQ = new List<string>();

    //Game game;

    public void Open()
    {
        pauseBtn?.onClick.AddListener(() =>
        {
            UIManager.GetUI().OpenPauseUI();
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

    void SpawnScoreNotif()
    {
        if (scoreQ.Count < 1 || currentSpawnScore != null) return;
        GameObject g = Instantiate(spawnScorePos, spawnScorePos.GetComponentInParent<RectTransform>());
        g.SetActive(true);
        g.GetComponent<Text>().text = scoreQ[0];
        scoreQ.RemoveAt(0);
        currentSpawnScore = g;
        Invoke("ClearScoreNotif", 1);
    }

    void ClearScoreNotif()
    {
        Destroy(currentSpawnScore);
        currentSpawnScore = null;
        SpawnScoreNotif();
    }
}
