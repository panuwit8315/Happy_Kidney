using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;
using SimpleJSON;
using UnityEngine.Events;
using System.Linq;

public class UILeaderboard : MonoBehaviour, IUI
{
    Game game;
    SoundManager sound;
    [SerializeField] Transform panel;
    [SerializeField] GameObject temp;
    [SerializeField] Button leftBtn, rightBtn;
    [SerializeField] int currentTab, maxTab, maxSpawnPerTab;
    [SerializeField] List<User.UserData> dataToShow;

    [SerializeField] GameObject myScoreObj;
    [SerializeField] Sprite[] rankSprites;
    
    public Button closeBtn;

    public string url;
    public string secret;

    int scoreGame = 0;
    string playerName;
    string playerTag;

    public UnityEvent OnGetDone;

    public void Open()
    {
        game = Game.GetInstance();
        sound = SoundManager.GetInstance();
        playerName = PlayerPrefs.GetString("PlayerName");
        playerTag = PlayerPrefs.GetString("PlayerTag");
        url = game.dataManager.url;
        secret = game.dataManager.secret;
        if (string.IsNullOrEmpty(playerTag))
        {
            playerTag = playerName + "#" + System.DateTime.Now.ToString("s");
            PlayerPrefs.SetString("PlayerTag",playerTag);
        }       
        
        scoreGame = game.fridgeSpawner.GetScore();
       
        closeBtn.onClick.AddListener(() =>
        {
            if(game.scene == SceneState.GAMEPLAY) UIManager.GetUI().OpenLobbyUI();
            sound.PlaySFXOneShot(SfxClipName.CLICK02);
            //closeBtn.gameObject.SetActive(false);
            Close();
        });

        GetData();        
    }

    public void AddData()
    {
        //User.UserData playerData = new User.UserData(PlayerPrefs.GetString("PlayerName"), scoreGame); //Debug.Log("user.userData" + user.userData.Count);
        dataToShow = user.userData; //Debug.Log("dataToShow" + user.userData.Count);
        SortLeaderboard();
        CalMaxTab(); //Debug.Log("CalMaxTab" + user.userData.Count);
        OpenTab(1); //Debug.Log("OpenTab" + user.userData.Count);
        SetMyScore();
        //print("AddData()");
    }

    public void SortLeaderboard()
    {
        dataToShow = dataToShow.OrderByDescending(score => score.score).ToList();
        user.userData = user.userData.OrderByDescending(score => score.score).ToList();
    }

    void OpenTab(int tabIndex)
    {
        currentTab = tabIndex;
        foreach (Transform t in panel.GetComponentsInChildren<Transform>())
        {
            if (t.gameObject == temp) continue;
            if (t == panel) continue;

            Destroy(t.gameObject);
        }

        //SetUp LeftButton
        if (currentTab > 1)
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

        int firstDataIndex = (currentTab - 1) * maxSpawnPerTab;
        int finalDataIndex = (maxSpawnPerTab * currentTab) - 1; //-1 เพราะ index ของlist เริ่มจาก 0        
        //Debug.Log("finalDataIndex= " + finalDataIndex + ", firstDataIndex= " + firstDataIndex);

        for (int i = firstDataIndex; i <= finalDataIndex; i++)
        {
            if (i >= dataToShow.Count) break;
            SpawnPlayerData(dataToShow[i],i+1);   
        }
    }

    void CalMaxTab()
    {
        int dataCount = dataToShow.Count;
        int remainder = dataCount % maxSpawnPerTab;
        maxTab = dataCount / maxSpawnPerTab;
        if (remainder > 0) maxTab++;
    }

    void SpawnPlayerData(User.UserData data,int rank)
    {
        GameObject g = Instantiate(temp, panel);
        SetPlayerScore(data, rank, g);
        g.SetActive(true);
    }

    void SetMyScore()
    {
        for (int i = 0; i < user.userData.Count; i++)
        {
            if (user.userData[i].tag == playerTag)
            {
                if (scoreGame > user.userData[i].score) user.userData[i].score = scoreGame;
                SetPlayerScore(user.userData[i], i + 1,myScoreObj);
                PlayerPrefs.GetInt("PlayerHighScore", scoreGame);
                break;
            }
        }
    }
    void SetPlayerScore(User.UserData data,int rank,GameObject obj)
    {
        Image rankImage;
        obj.transform.Find("Name").GetComponent<Text>().text = data.name;
        obj.transform.Find("Score").GetComponent<Text>().text = data.score.ToString();
        if(rank <= 3)
        {
            rankImage = obj.transform.Find("TopRankSprite").GetComponent<Image>();
            rankImage.sprite = rankSprites[rank-1];
            rankImage.gameObject.SetActive(true);
        }
        else
        {
            rankImage = obj.transform.Find("RankSprite").GetComponent<Image>();
            rankImage.gameObject.SetActive(true);
            Text rankTx = obj.transform.Find("RankText").GetComponent<Text>();
            rankTx.text = rank.ToString();
            rankTx.gameObject.SetActive(true);
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

    public User user;

    public void GetData()
    {
        string urlData = $"{url}/User/userData.json?auth={secret}";

        user.userData = new List<User.UserData>();

        RestClient.Get(urlData).Then(response =>
        {
            Debug.Log(response.Text);
            JSONNode jsonNode = JSONNode.Parse(response.Text);

            for (int i = 0; i < jsonNode.Count; i++)
            {
                user.userData.Add(new User.UserData(jsonNode[i]["name"], jsonNode[i]["score"], jsonNode[i]["tag"]));               
            }

            if (game.scene == SceneState.GAMEPLAY)
            {
                bool isDuplicateTag = false;
                for (int i = 0; i < user.userData.Count; i++)
                {
                    if (user.userData[i].tag == playerTag)
                    {
                        if (scoreGame > user.userData[i].score) user.userData[i].score = scoreGame;
                        isDuplicateTag = true;
                    }
                }

                if (!isDuplicateTag)
                {
                    user.userData.Add(new User.UserData(playerName, scoreGame, playerTag));
                }
            }

            Debug.Log("GetData" + user.userData.Count);
            OnGetDone.Invoke();
            SetData();

        }).Catch(error =>
        {
            Debug.Log("Not Found Data");
            if (game.scene == SceneState.GAMEPLAY)
            {
                user.userData = new List<User.UserData>();
                user.userData.Add(new User.UserData(playerName, scoreGame, playerTag));
                SetData();
            }             
        });
    }

    public void SetData()
    {
        string urlData = $"{url}/User/.json?auth={secret}";

        RestClient.Put<User>(urlData, user).Then(response =>
        {
            Debug.Log("Upload Data Complete");

        }).Catch(error =>
        {
            Debug.Log("error on set to server");
            Debug.Log(error);
        });
            
        Debug.Log("SetData" + user.userData.Count);  
    }
}
