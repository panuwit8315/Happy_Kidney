using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInputName : MonoBehaviour, IUI
{
    [SerializeField] Button closeBtn;
    [SerializeField] InputField inputField;
    [SerializeField] Text outputTx;

    public void Open()
    {       
        closeBtn.onClick.AddListener(() =>
        {
            OnClickStartBtn();
        });
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

    void OnClickStartBtn()
    {
        string inputStr = inputField.text;
        inputField.text = "";
        print(inputStr);
        if (string.IsNullOrEmpty(inputStr)) return;

        if(inputStr.Length > 12)
        {
            outputTx.text = "ชื่อที่คุณตั้งยาวเกินไป\nกรุณาตั้งให้ไม่เกิน12ตัวอักษร";
        }
        else
        {
            string dateTimeStr = DateTime.Now.ToString("s");
            string playerTag = inputStr + "#" + dateTimeStr; print(playerTag);
            PlayerPrefs.SetString("PlayerName", inputStr);
            PlayerPrefs.SetString("PlayerTag", playerTag);
            Close();
            UIManager.GetUI().OpenLobbyUI();          
        }
    }
}
