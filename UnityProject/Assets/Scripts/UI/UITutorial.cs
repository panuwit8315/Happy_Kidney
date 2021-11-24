using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITutorial : MonoBehaviour, IUI
{
    //public enum TuturialTitle { PickIngre,HoldIngre,AddScore,Combo,Bonus,Hint,Fridge1,Fridge2,Item,TimeUpCoin,TimeUpFridge,TimeUpIngre}
    [System.Serializable]
    private class Tutorial
    {
        public string name;
        public GameObject content;
        public GameObject mask;
        public bool continueOnClick;
        public bool continueNextTutorial;
    }

    [SerializeField] GameObject bgBack;
    [SerializeField] GameObject defaultMask;
    [SerializeField] bool continueOnClick;
    [SerializeField] bool continueNextTutorial;
    [SerializeField] int tutorialStep = 0;
    [SerializeField] Tutorial[] tutorialContents; //StoryBoradDesign URL https://cdn.discordapp.com/attachments/591652855807475724/909375372779462666/Tutorial1.png

    UIManager ui;
    UIGamePlay uIGamePlay;
    UIEndGame uIEndGame;

    float timer = 1;
    float clickCoolDown = 1;

    public void Open()
    {
        tutorialStep = PlayerPrefs.GetInt("TutorialStep", 0);
        if (tutorialStep < 7) tutorialStep = 0;

        ui = UIManager.GetUI();
        uIGamePlay = ui.UIGamePlay();
        uIEndGame = ui.UIEndGame();
        if (uIEndGame == null)
        {
            print("uIEndGame is mull");
            if (uIGamePlay != null)
            {
                uIGamePlay.EnebleButton(false);
                tutorialContents[0].mask = uIGamePlay.mask_frigeOpen_UI;
                tutorialContents[4].mask = uIGamePlay.mask_combobar_UI;
                tutorialContents[5].mask = uIGamePlay.mask_time_score_UI;
                tutorialContents[6].mask = uIGamePlay.mask_hintBtn_UI;
                tutorialContents[8].mask = uIGamePlay.mask_level_UI;
                tutorialContents[9].mask = uIGamePlay.mask_item_UI;
            }
        }
        else
        {
            print("uIEndGame is open");
            tutorialContents[10].mask = uIEndGame.mask_coin_UI;
            tutorialContents[11].mask = uIEndGame.mask_fridge_UI;
            tutorialContents[12].mask = uIEndGame.mask_ingre_UI;
        }
        
        OpenTutorial(tutorialStep);
    }

    public void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if(timer<0) if (tutorialContents[tutorialStep].continueOnClick) tutorialContents[tutorialStep].content.transform.Find("TabText").gameObject.SetActive(true);
            return;
        }
        if (Input.GetMouseButtonUp(0) && continueOnClick)
        {
            print("UITutorial Click");
            timer = clickCoolDown;
            if (tutorialContents[tutorialStep].continueOnClick)SoundManager.GetInstance().PlaySFXOneShot(SfxClipName.CLICK02);
            tutorialStep++;
            continueOnClick = false;
            
            if (continueNextTutorial)
            {
                CloseCurrentContent();
                OpenTutorial(tutorialStep);
            }
            else
            {
                CloseCurrentContent();
                Close();
            }
            
        }
    }

    public void Close()
    {
        if (uIGamePlay != null) uIGamePlay.EnebleButton(true);
        if (uIEndGame != null) 
        {
            PlayerPrefs.SetString("TutorialComplete", "Yes");
            uIEndGame.close.enabled = true;
        } 
        
        Game.GetInstance().timer.isRunTime = true;

        PlayerPrefs.SetInt("TutorialStep", tutorialStep);

        GetComponent<Animator>().SetTrigger("MoveOut");
        Invoke("DestroyObj", 0.5f);
    }

    public void DestroyObj()
    {
        UIManager.GetUI().CloseUI(this);
        if (tutorialStep < 10)
        {
            uIGamePlay.SetLayerRubbishBin("RubbishBin");
            Game.GetInstance().fridgeSpawner.GetFridgeObj().EnableAllIngredintCol(true);
        }
        Destroy(gameObject);
    }

    void OpenTutorial(int tutorialIndex)
    {
        if(tutorialIndex == 1)
        {
            List<GameObject> ingreObjs = Game.GetInstance().fridgeSpawner.currentFridgeObj.GetComponent<Fridge>().ingredientObjs;
            foreach(GameObject g in ingreObjs)
            {
                Ingredient i = g.GetComponent<Ingredient>();
                if(i.data.type == IngredientType.SHOULD_NOT_EAT)
                {
                    g.GetComponent<SpriteRenderer>().sortingLayerName = "Popup";
                }
                else
                {
                    g.GetComponent<BoxCollider2D>().enabled = false;
                }
            }
        }
        else if (tutorialIndex == 2)
        {
            uIGamePlay.SetLayerRubbishBin("UI");
        }
        else if (tutorialIndex == 4)
        {
            Game.GetInstance().fridgeSpawner.GetFridgeObj().EnableAllIngredintCol(false);
            List<GameObject> ingreObjs = Game.GetInstance().fridgeSpawner.currentFridgeObj.GetComponent<Fridge>().ingredientObjs;
            foreach (GameObject g in ingreObjs)
            {
                
                g.GetComponent<SpriteRenderer>().sortingLayerName = "Ingredente";

            }
        }
        else if (tutorialIndex == 6)
        {
            uIGamePlay.hintBtn.enabled = true;
        }
        else if (tutorialIndex == 9)
        {
            //Game.GetInstance().fridgeSpawner.GetFridgeObj().EnableAllIngredintCol(false);
            print("tutorialIndex == 9");
        }

        if (tutorialContents[tutorialIndex].mask != null)
        {
            GameObject newMask = Instantiate(tutorialContents[tutorialIndex].mask, gameObject.transform);
            newMask.name = "Mask";
            newMask.transform.SetAsFirstSibling();
            bgBack.transform.SetParent(newMask.transform);
        }
        else
        {
            defaultMask.transform.SetAsFirstSibling();
            bgBack.transform.SetParent(defaultMask.transform);
        }
        tutorialContents[tutorialIndex].content.SetActive(true);
        this.continueOnClick = tutorialContents[tutorialIndex].continueOnClick;
        this.continueNextTutorial = tutorialContents[tutorialIndex].continueNextTutorial;
    }

    public void OpenTheFridge()
    {
        if(tutorialStep == 0)
        {
            CloseCurrentContent();
            tutorialStep++;
            OpenTutorial(tutorialStep);
        }
    }

    public void OnClickIngreObj()
    {
        if (tutorialStep == 1)
        {
            CloseCurrentContent();
            tutorialStep++;
            OpenTutorial(tutorialStep);
        }
    }

    public void IngreBackToSpawnPoint()
    {
        if (tutorialStep == 2)
        {
            CloseCurrentContent();
            tutorialStep++;
            OpenTutorial(tutorialStep);
        }
    }

    public void OnUseItem()
    {
        if (tutorialStep == 9)
        {
            Game.GetInstance().fridgeSpawner.GetFridgeObj().EnableAllIngredintCol(true);
            CloseCurrentContent();
            tutorialStep++;
            Close();
        }
    }

    void CloseCurrentContent()
    {
        foreach(Tutorial t in tutorialContents)
        {
            if (t.content.activeInHierarchy)
            {
                t.content.SetActive(false);
            }
        }
    }
}
