using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobby : MonoBehaviour, IUI
{
    [SerializeField] Button startBtn;
    [SerializeField] Button settingBtn;
    [SerializeField] Button leaderbordBtn;
    [SerializeField] Button shopBtn;
    [SerializeField] Button infoBtn;
    [SerializeField] Button closeShopBtn;
    [SerializeField] Text nameTx;
    [SerializeField] Text coinTx;
    [SerializeField] Text coinInShopTx;
    [SerializeField] Text highScoreTx;
    [SerializeField] Text bonusTx;
    [SerializeField] GameObject[] furObj;
    [SerializeField] int[] myFur;
    [SerializeField] int curentBonus;

    [Header("Buy Panel")]
    [SerializeField] Text priceTx;
    [SerializeField] Image showSprite;
    [SerializeField] Button closeBuyBtn, buyBtn;

    Game game;
    SoundManager sound;
    public void Open()
    {
        GetComponent<Animator>().SetTrigger("FadeInFur");
        game = Game.GetInstance();
        sound = SoundManager.GetInstance();
        game.scene = SceneState.LOBBY;
        startBtn?.onClick.AddListener(() =>
        {
            sound.PlaySFXOneShot(SfxClipName.CLICK02);
            UIManager.GetUI().OpenSelectModeUI();
        });
        settingBtn?.onClick.AddListener(() =>
        {
            sound.PlaySFXOneShot(SfxClipName.CLICK02);
            UIManager.GetUI().OpenPauseUI();            
        });
        leaderbordBtn?.onClick.AddListener(() =>
        {
            sound.PlaySFXOneShot(SfxClipName.CLICK02);
            UIManager.GetUI().OpenLeaderboardUI();
        });
        shopBtn?.onClick.AddListener(() =>
        {
            sound.PlaySFXOneShot(SfxClipName.CLICK02);
            OpenShop();
        });
        infoBtn?.onClick.AddListener(() =>
        {
            sound.PlaySFXOneShot(SfxClipName.CLICK02);
            UIManager.GetUI().OpenInfoUI();
        });
        closeShopBtn?.onClick.AddListener(() =>
        {
            sound.PlaySFXOneShot(SfxClipName.CLICK02);
            CloseShop();
        });
        closeBuyBtn?.onClick.AddListener(() =>
        {
            sound.PlaySFXOneShot(SfxClipName.CLICK02);
            GetComponent<Animator>().SetTrigger("BuyMoveOut");
        });

        nameTx.text = PlayerPrefs.GetString("PlayerName");
        coinTx.text = PlayerPrefs.GetInt("PlayerCoin",0).ToString();
        coinInShopTx.text = PlayerPrefs.GetInt("PlayerCoin",0).ToString();
        highScoreTx.text = PlayerPrefs.GetInt("PlayerHighScore",0).ToString();

        SetFurniture();
    }

    public void RefreshHighScore()
    {
        highScoreTx.text = PlayerPrefs.GetInt("PlayerHighScore", 0).ToString();
    }

    public void Close()
    {
        GetComponent<Animator>().SetTrigger("MoveOut");
        GetComponent<Animator>().SetTrigger("FadeOutFur");
        Invoke("DestroyObj",0.5f);
    }

    public void DestroyObj()
    {
        UIManager.GetUI().CloseUI(this);
        Destroy(gameObject);
    }

    void OpenShop()
    {
        GetComponent<Animator>().SetTrigger("MoveOut");
        GetComponent<Animator>().SetTrigger("ShopMoveIn");

        ShowOutline(true);
    }

    void CloseShop()
    {
        GetComponent<Animator>().SetTrigger("MoveIn");
        GetComponent<Animator>().SetTrigger("ShopMoveOut");
        shopBtn.gameObject.SetActive(true);
        ShowOutline(false);
    }

    void ShowOutline(bool set)
    {
        foreach(GameObject g in furObj)
        {
            g.GetComponent<FurnitureObj>().ShowOutline(set);
        }
    }

    void SetFurniture(bool skipLoad = false) //BG, Carpet, Desk, Frame, Shelf, Sofa, Standy, Window
    {
        int minFurniture = 100;
        for(int i = 0; i < furObj.Length; i++)
        {
            if (!skipLoad)
            {
                string keySave = "Furniture_" + ((FurnitureType)i).ToString(); //print("LoadFurniture:" + keySave);
                myFur[i] = PlayerPrefs.GetInt(keySave, 0);
                if (i == 0 && myFur[0] == 0) myFur[0] = 1; //มีแค่ BG ที่จะได้อันแรกมาฟรี
            }           

            FurnitureData.Furniture furniture = game.dataManager.GetFurnitureData((FurnitureStyle)myFur[i], (FurnitureType)i);
            furObj[i].GetComponent<FurnitureObj>().Setup(furniture);

            if (myFur[i] < minFurniture) minFurniture = myFur[i];
        }
        int furnitureBonus = minFurniture * 5;
        print("furnitureBonus:" + furnitureBonus);
        bonusTx.text = "คะแนน +" + furnitureBonus + "%";
        PlayerPrefs.SetInt("FurnitureBonus", furnitureBonus);

        //if (!skipLoad)
        //{
            if(curentBonus < minFurniture)
            {
                GetComponent<Animator>().SetTrigger("BonusFadeIn");
                curentBonus = minFurniture;
                Debug.LogWarning("BusnusFadeIn");
            }
        //}
    }

    public void OpenBuyPanel(FurnitureData.Furniture nextFurniture)
    {
        GetComponent<Animator>().SetTrigger("BuyMoveIn");
        int myCoin = PlayerPrefs.GetInt("PlayerCoin", 0);
        if (myCoin >= nextFurniture.price) buyBtn.interactable = true;
        else buyBtn.interactable = false;
        showSprite.sprite = nextFurniture.spriteToShow;
        priceTx.text = nextFurniture.price.ToString();
        buyBtn?.onClick.RemoveAllListeners();
        buyBtn?.onClick.AddListener(() =>
        {
            sound.PlaySFXOneShot(SfxClipName.CLICK02);
            GetComponent<Animator>().SetTrigger("BuyMoveOut");

            //Buy Function
            myCoin -= nextFurniture.price;
            coinTx.text = myCoin.ToString();
            coinInShopTx.text = myCoin.ToString();          
            myFur[(int)nextFurniture.type] = (int)nextFurniture.style;
            string keySave = "Furniture_" + nextFurniture.type.ToString();

            PlayerPrefs.SetInt("PlayerCoin", myCoin);
            PlayerPrefs.SetInt(keySave, (int)nextFurniture.style);

            print("ซื้อ "+nextFurniture.type+nextFurniture.style+" สำเร็จ");
            SetFurniture(true);
            ShowOutline(true);
        });
    }
}
