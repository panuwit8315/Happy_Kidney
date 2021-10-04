using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPause : MonoBehaviour, IUI
{
    [SerializeField] Button closeBtn;
    [SerializeField] Button Btn;

    public void Open()
    {
        Game game = Game.GetInstance();
        game.timer.SetRunTime(false);
        game.GetComponent<RayCastManager>().enabled = false;
        closeBtn?.onClick.AddListener(() =>
        {
            Close();
            game.timer.SetRunTime(true);
            game.GetComponent<RayCastManager>().enabled = true;
        });
        Btn?.onClick.AddListener(() =>
        {
            Application.OpenURL("https://www.instagram.com/jkp_jeng/");
            //Close();
        });
    }

    public void Close()
    {
        GetComponent<Animator>().SetTrigger("MoveOut");
        Invoke("DestroyObj", 1);
    }

    public void DestroyObj()
    {
        UIManager.GetUI().CloseUI(this);
        Destroy(gameObject);
    }   
}
