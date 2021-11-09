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

    [SerializeField] Text nameTx;
    [SerializeField] Text scoreTx;
    
    public Button closeBtn;

    public string url = "https://happy-kidney-default-rtdb.asia-southeast1.firebasedatabase.app/";
    public string secret = "U6YbKtoQ5tcUBci5ErEm1wL8bOYybsR2BcvQRrnu";

    int scoreGame = 0;
    public UnityEvent OnGetDone;

    public void Open()
    {
        game = Game.GetInstance();
        string playerName = PlayerPrefs.GetString("PlayerName");
        int score = game.fridgeSpawner.GetScore();
        nameTx.text = playerName;
        scoreTx.text = score.ToString();
        scoreGame = score;
        sound = SoundManager.GetInstance();
        
        closeBtn.onClick.AddListener(() =>
        {
            UIManager.GetUI().OpenLobbyUI();
            sound.PlaySFXOneShot(SfxClipName.CLICK02);
            closeBtn.gameObject.SetActive(false);
            Close();
        });

        GetData();        
    }

    public void AddData()
    {
        User.UserData playerData = new User.UserData(PlayerPrefs.GetString("PlayerName"), scoreGame);
        Debug.Log("user.userData" + user.userData.Count);
        dataToShow = user.userData; 
        Debug.Log("dataToShow" + user.userData.Count);
        SortLeaderboard();
        CalMaxTab(); 
        Debug.Log("CalMaxTab" + user.userData.Count);
        OpenTab(1);
        Debug.Log("OpenTab" + user.userData.Count);
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
        Debug.Log("finalDataIndex= " + finalDataIndex + ", firstDataIndex= " + firstDataIndex);

        for (int i = firstDataIndex; i <= finalDataIndex; i++)
        {
            if (i >= dataToShow.Count) break;
            SpawnPlayerData(dataToShow[i]);   
        }
    }

    void CalMaxTab()
    {
        int dataCount = dataToShow.Count;
        int remainder = dataCount % maxSpawnPerTab;
        maxTab = dataCount / maxSpawnPerTab;
        if (remainder > 0) maxTab++;
    }

    void SpawnPlayerData(User.UserData data)
    {
        GameObject g = Instantiate(temp, panel);
        //g.GetComponentInChildren<Text>().text = data.thName;
        g.transform.Find("Score").GetComponent<Text>().text = data.score.ToString();
        g.transform.Find("Score").GetComponent<Text>().enabled = true;
        g.transform.Find("Name").GetComponent<Text>().text = data.name;
        g.transform.Find("Name").GetComponent<Text>().enabled = true;
        //g.transform.Find("RankSprite").GetComponent<Image>().sprite = data.sprite;
        //g.GetComponentInChildren<Image>().sprite = data.sprite;
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

    [System.Serializable]
    public class User
    {
        [System.Serializable]
        public class UserData
        {
            public string name;
            public int score;

            public UserData(string name,int score)
            {
                this.name = name;
                this.score = score;              
            }
        }

        public List<UserData> userData;
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
                user.userData.Add(new User.UserData(jsonNode[i]["name"], jsonNode[i]["score"]));               
            }

            bool isDuplicateName = false;
            for (int i = 0; i < user.userData.Count; i++)
            {
                if (user.userData[i].name == PlayerPrefs.GetString("PlayerName"))
                {
                    user.userData[i].score = scoreGame;
                    isDuplicateName = true;
                }
            }

            if(!isDuplicateName)
            {
                user.userData.Add(new User.UserData(PlayerPrefs.GetString("PlayerName"), scoreGame));
            }

            Debug.Log("GetData" + user.userData.Count);
            OnGetDone.Invoke();
            SetData();

        }).Catch(error =>
        {
            Debug.Log("Not Found Data");
            user.userData = new List<User.UserData>();
            user.userData.Add(new User.UserData(PlayerPrefs.GetString("PlayerName"), scoreGame));
            SetData();     
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
