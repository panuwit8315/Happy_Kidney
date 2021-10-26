using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugCtrl : MonoBehaviour
{
    [SerializeField] GameObject controlPanel;
    [SerializeField] GameObject content;
    [SerializeField] GameObject debugTemp;
    [SerializeField] Button debugBtn;
    [SerializeField] Button clearBtn;
    [SerializeField] Button enterBtn;
    [SerializeField] InputField inputField;

    static DebugCtrl instance;
    List<GameObject> currentDebug = new List<GameObject>();
    //bool canUseInput = false;

    Game game;

    public static DebugCtrl GetDebug() { return instance; }
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        OnBack();
        clearBtn.onClick.AddListener(() =>
        {
            Clear();
        });
        enterBtn.onClick.AddListener(() =>
        {
            OnEnterBtn();
        });
        game = Game.GetInstance();
    }

    void OnBack()
    {
        controlPanel.SetActive(false);
        debugBtn.transform.Find("Text").GetComponent<Text>().text = "Debug";
        debugBtn?.onClick.RemoveAllListeners();
        debugBtn.onClick.AddListener(() =>
        {
            OnDebug();
        });
    } 

    void OnDebug()
    {
        controlPanel.SetActive(true);
        debugBtn.transform.Find("Text").GetComponent<Text>().text = "กลับ";
        debugBtn?.onClick.RemoveAllListeners();
        debugBtn?.onClick.AddListener(() =>
        {
            OnBack();
        });
    }

    public void Log(object message,bool isJeng = false)
    {
        GameObject g = Instantiate(debugTemp, content.transform);
        g.SetActive(true);
        g.GetComponent<Text>().text = message.ToString();
        currentDebug.Add(g);
        if (isJeng)
        {
            Button b = g.AddComponent<Button>();
            g.GetComponent<Text>().color = Color.red;
            g.GetComponent<Text>().fontStyle = FontStyle.Bold;
            b.onClick.AddListener(() =>
            {
                Application.OpenURL("https://www.instagram.com/jkp_jeng/");                
            });
        }
        if (currentDebug.Count > 50) 
        {
            Destroy(currentDebug[0]);
            currentDebug.RemoveAt(0);
        }      
        
        Debug.Log(message);
    }

    void Clear()
    {
        foreach(GameObject g in currentDebug) Destroy(g);
        currentDebug.Clear();
    }

    void OnEnterBtn()
    {
        if (string.IsNullOrEmpty(inputField.text)) return;

        string input = inputField.text;
        inputField.text = "";

        {/*if (!canUseInput)
        {
            if (input == "jkp_jeng" || input == "Jkp_jeng" || input == "JKP_JENG") 
            {  
                canUseInput = true;
                Log("คุณมีสิทธิ์ใช้คำสั่งต่างๆแล้ว");
                Log(">>>>กดตรงข้อความนี้ครับ<<<<",true);
            }
            else
            {
                Log("คุณไม่มีสิทธิ์ในใช้คำสั่งต่างๆ");
            }
            return;
        }*/
        }

        if(input == "time5" || input == "Time5" || input == "time 5" || input == "Time 5")
        {
            if(game.scene != SceneState.GAMEPLAY)
            {
                Log("ต้องสั่งในหน้า GamePlay");
                return;
            }
            game.timer.SetTime(5);
            Log("ลดเวลาเหลือ 5 วินาที");
            return;
        }
        else if (input == "time20" || input == "Time20" || input == "time 20" || input == "Time 20")
        {
            if (game.scene != SceneState.GAMEPLAY)
            {
                Log("ต้องสั่งในหน้า GamePlay");
                return;
            }
            game.timer.SetTime(20);
            Log("ลดเวลาเหลือ 20 วินาที");
            return;
        }
        else if (input == "unlock" || input == "Unlock")
        {
            PlayerPrefs.SetInt("HighestFridgeLv", 40);
            Log("ปลดล็อก โหมดยากแล้ว");
            Log("ลองเข้าหน้าต่างเลือกโหมดดู");
            return;
        }

        string[] inputs = input.Split(' ');
        if ((inputs[0] != "dev") && (inputs[0] != "Dev"))
        {
            Log("เราไม่รู้จักคำสั่งของคุณ");
            return;
        }
        if(inputs.Length < 2)
        {
            Log("เราไม่รู้จักคำสั่งของคุณ");
            return;
        }

        string command = inputs[1];
        if(command == "next" || command == "Next")
        {
            if(!(game.scene == SceneState.GAMEPLAY))
            {
                Log("ต้องสั่งในหน้า GamePlay");
                return;
            }
            if (inputs.Length < 3)
            {
                Log("เราไม่รู้จักคำสั่งของคุณ");
                return;
            }
            int value;
            if(!Int32.TryParse(inputs[2], out value))
            {
                Log("คำสั่ง Next ต้องสั่งแบบนี้ [Dev next 31]" + value);
                return;
            }

            if (value > 0)
            {
                game.fridgeSpawner.SetFridgeCount(value-1);
                UIManager.GetUI().UIGamePlay().SetActiveAllItem();
                Log("ตู้เย็นตู้ต่อไป จะเป็นตู้ที่ " + value+" และปลดล็อกไอเทมทั้งหมด");
                return;
            }
            else
            {
                Log("คำสั่งนี้ต้องสั่งด้วยตัวเลขที่มากกว่า0");
                return;
            }
        }
        else if (command == "time" || command == "Time")
        {
            if (inputs.Length < 3)
            {
                Log("เราไม่รู้จักคำสั่งของคุณ");
                return;
            }
            if(inputs[2] == "5")
            {
                if(game.scene == SceneState.GAMEPLAY)
                {
                    game.timer.SetTime(5);
                    Log("ลดเวลาเหลือ 5 วินาที");
                    return;
                }
                else
                {
                    Log("ต้องสั่งในหน้า GamePlay");
                    return;
                }               
            }
            else
            {
                Log("เราไม่รู้จักคำสั่งของคุณ");
                return;
            }
        }
        else
        {
            Log("เราไม่รู้จักคำสั่งของคุณ");
            return;
        }
    }
}
