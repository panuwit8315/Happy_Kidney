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
    [SerializeField] Sprite[] binSprite;
    [SerializeField] GameObject[] itemsObj;
    [SerializeField] GameObject bonusPanel;
    [SerializeField] GameObject addScoreNoftPos;
    [SerializeField] GameObject addTimeNotfPos;
    [SerializeField] Button pauseBtn;
    public Button hintBtn;
    [SerializeField] GameObject hintNoft;

    [Header("Mask To Tutorial UI")]
    public GameObject mask_frigeOpen_UI;
    public GameObject mask_time_score_UI;
    public GameObject mask_level_UI;
    public GameObject mask_item_UI;
    public GameObject mask_combobar_UI;
    public GameObject mask_hintBtn_UI;

    LevelUnlockItem lvUnlockItem;
    bool nextSpawnScoreNotf = true;
    List<string> scoreQ = new List<string>();
    SoundManager sound = SoundManager.GetInstance();    

    public void Open()
    {
        Game.GetInstance().scene = SceneState.GAMEPLAY;
        pauseBtn?.onClick.AddListener(() =>
        {
            sound.PlaySFXOneShot(SfxClipName.CLICK02);
            UIManager.GetUI().OpenPauseUI();
        });
        hintBtn?.onClick.AddListener(() =>
        {
            sound.PlaySFXOneShot(SfxClipName.CLICK02);
            UIManager.GetUI().OpenHintUI();
            hintNoft.SetActive(false);            
        });

        PlayDifference currentDiff = Game.GetInstance().diff;
        Debug.Log("UI Play Game Mode:" + currentDiff);
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
        Invoke("DestroyObj", 0.5f);
    }

    public void DestroyObj()
    {
        UIManager.GetUI().CloseUI(this);
        Destroy(gameObject);
    }

    public void CopyFridgeOpenMaskAndOpenTutorial()
    {
        GameObject m = Game.GetInstance().fridgeSpawner.currentFridgeObj.GetComponent<Fridge>().mask_fridge_close;
        mask_frigeOpen_UI = Instantiate(m, m.transform.position,m.transform.rotation,gameObject.transform);
        mask_frigeOpen_UI.transform.localScale = m.transform.localScale*20;
        UIManager.GetUI().OpenTutorialUI();
    }

    public void TimeAlert()
    {
        GetComponent<Animator>().SetTrigger("TimeAlert");
    }

    public void SetHintNoft(bool isOn)
    {
        hintNoft.SetActive(isOn);
    }

    public void SetTimeUI(string timeStr)
    {
        timeText.text = timeStr;
    }

    public void SetScoreUI(int score, string textToShow = null)
    {
        scoreText.text = score.ToString();
        if (!string.IsNullOrEmpty(textToShow))
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
    public void SetActiveAllItem()
    {
        itemsObj[0].GetComponent<ItemBtn>().EnableItem();
        itemsObj[1].GetComponent<ItemBtn>().EnableItem();
        itemsObj[2].GetComponent<ItemBtn>().EnableItem();
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

    public void ShowRedNoft()
    {
        GetComponent<Animator>().SetTrigger("RedNoft");
    }

    private void SpawnScoreNotif()
    {
        if (scoreQ.Count < 1 || !nextSpawnScoreNotf) return;

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

    public void OpenBinSprite(bool isOpen,GameObject binObj)
    {
        Sprite s = default;
        if (isOpen) s = binSprite[1]; //openSprite
        else s = binSprite[0]; //closeSprite

        binObj.GetComponent<SpriteRenderer>().sprite = s;
    }

    public void MoveOutBin()
    {
        if(rubbishBinAnim.GetCurrentAnimatorStateInfo(0).IsName("MoveIn"))rubbishBinAnim.SetTrigger("MoveOut");       
    }

    public void EnebleButton(bool isOn)
    {
        hintBtn.enabled = isOn;
        pauseBtn.enabled = isOn;
    }

    public void SetLayerRubbishBin(string layerNameToSet)
    {
        rubbishBinAnim.gameObject.transform.Find("RubbishBin_Left/RubbishBinL").GetComponent<SpriteRenderer>().sortingLayerName = layerNameToSet;
        rubbishBinAnim.gameObject.transform.Find("RubbishBin_Right/RubbishBinR").GetComponent<SpriteRenderer>().sortingLayerName = layerNameToSet;
    }

}
