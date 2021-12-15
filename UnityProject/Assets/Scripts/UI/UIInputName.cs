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
        
        print(inputStr);
        outputTx.text = inputStr;

        if (string.IsNullOrEmpty(inputStr)) return;

        if(inputStr.Length > 12)
        {
            outputTx.text = "ชื่อที่คุณตั้งยาวเกินไป\nกรุณาตั้งให้ไม่เกิน12ตัวอักษร";
        }
        else
        {
            outputTx.text = "ตั้งชื่อสำเร็จ";
            //string dateTimeStr = DateTime.UtcNow.ToString("s");
            //outputTx.text = "รับเวลาสำเร็จ";
            int ranID = UnityEngine.Random.Range(0, 1000000);
            outputTx.text = "สุ่มไอดีประจำตัวสำเร็จ";
            string playerTag = inputStr + "#" + ranID; //print(playerTag);
            outputTx.text = "สร้างแท็กประจำตัวสำเร็จ";
            PlayerPrefs.SetString("PlayerName", inputStr);
            outputTx.text = "บันทึกชื่อสำเร็จ";
            PlayerPrefs.SetString("PlayerTag", playerTag);
            outputTx.text = "บันทึกแท็กสำเร็จ";
            Close();
            outputTx.text = "ปิด UI นี้";
            UIManager.GetUI().OpenLobbyUI();
            outputTx.text = "เปิด UI Lobby";
        }
        inputField.text = "";
    }
}
