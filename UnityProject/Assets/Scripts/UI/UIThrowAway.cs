using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIThrowAway : MonoBehaviour, IUI
{
    public Button closeBtn;
    [SerializeField] Transform panel;
    [SerializeField] GameObject temp;
    [SerializeField] Button leftBtn, rightBtn;
    [SerializeField] int currentTab, maxTab, maxSpawnPerTab;
    [SerializeField] List<IngredientData> dataToShow;
    
    Game game;
    SoundManager sound;

    public void Open()
    {
        game = Game.GetInstance();
        sound = SoundManager.GetInstance();
        closeBtn.onClick.AddListener(() =>
        {
            UIManager.GetUI().OpenLobbyUI();
            sound.PlaySFXOneShot(SfxClipName.CLICK02);
            closeBtn.gameObject.SetActive(false);
            Close();
        });

        dataToShow = game.fridgeSpawner.allCurrentIngredientThrowAway;
        CalMaxTab();
        OpenTab(1);
    }

    void OpenTab(int tabIndex)
    {
        currentTab = tabIndex;
        foreach(Transform t in panel.GetComponentsInChildren<Transform>())
        {
            if (t.gameObject == temp) continue; 
            if (t == panel) continue;

            Destroy(t.gameObject);
        }

        //SetUp LeftButton
        if(currentTab > 1)
        {
            leftBtn.interactable = true;
            leftBtn.onClick.RemoveAllListeners();
            leftBtn.onClick.AddListener(() =>
            {
                sound.PlaySFXOneShot(SfxClipName.CLICK02);
                OpenTab(currentTab - 1);
            });
        }
        else
        {
            leftBtn.interactable = false;
        }

        //SetUp RightButton
        if (currentTab < maxTab)
        {
            rightBtn.interactable = true;
            rightBtn.onClick.RemoveAllListeners();
            rightBtn.onClick.AddListener(() =>
            {
                sound.PlaySFXOneShot(SfxClipName.CLICK02);
                OpenTab(currentTab + 1);
            });
        }
        else
        {
            rightBtn.interactable = false;
        }

        int firstDataIndex = (currentTab-1) * maxSpawnPerTab;
        int finalDataIndex = (maxSpawnPerTab*currentTab) - 1; //-1 เพราะ index ของlist เริ่มจาก 0        
        Debug.Log("finalDataIndex= "+finalDataIndex+", firstDataIndex= "+firstDataIndex);

        for(int i = firstDataIndex; i <= finalDataIndex; i++)
        {
            if (i >= dataToShow.Count) break;
            SpawnIngredintList(dataToShow[i]);
        }
    }

    void CalMaxTab()
    {
        int dataCount = dataToShow.Count;
        int remainder = dataCount % maxSpawnPerTab;
        maxTab = dataCount / maxSpawnPerTab;       
        if (remainder > 0) maxTab++;
    }

    void SpawnIngredintList(IngredientData data)
    {
        GameObject g = Instantiate(temp, panel);
        g.GetComponentInChildren<Text>().text = data.thName;
        /*g.transform.Find("Score").GetComponent<Text>().text = "45,645,566";
        g.transform.Find("Name").GetComponent<Text>().text = "MpTong";
        g.transform.Find("RankSprite").GetComponent<Image>().sprite = data.sprite;*/
        g.GetComponentInChildren<Image>().sprite = data.sprite;
        g.SetActive(true);
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
}
