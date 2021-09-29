using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePlay : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] Text timeText;
    [SerializeField] Text fridgeText;
    [SerializeField] Slider comboSlider;
    [SerializeField] Animator rubbishBinAnim;
    [SerializeField] GameObject[] itemsObj;

    //Game game;

    public void Open()
    {
        //do something
    }
    public void Close()
    {
        GetComponent<Animator>().SetTrigger("MoveOut");
        Invoke("DestroyObj", 3);
    }

    private void DestroyObj()
    {
        UIManager.GetUI().CloseGamePlayUI();
        Destroy(gameObject);
    }

    public void SetTimeUI(string timeStr)
    {
        timeText.text = timeStr;
    }

    public void SetScoreUI(int score)
    {
        scoreText.text = score.ToString();
    }

    public void SetFridgeUI(int fridgeCount)
    {
        fridgeText.text = "ตู้เย็น x" + fridgeCount.ToString();
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
}
