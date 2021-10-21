using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHint : MonoBehaviour, IUI
{
    public Button closeBtn;
    [SerializeField] Transform panel;
    [SerializeField] GameObject temp;

    SoundManager sound;

    public void Open()
    {
        Game game = Game.GetInstance();
        sound = SoundManager.GetInstance();
        game.rayCast.enabled = false;
        closeBtn?.onClick.AddListener(() =>
        {
            sound.PlaySFXOneShot(SfxClipName.CLICK02);
            Close();
            game.rayCast.enabled = true; 
        });

        foreach(IngredientData n in game.fridgeSpawner.allCurrentIngredientNotEat)
        {
            SpawnIngredintList(n);
        }
        
    }

    void SpawnIngredintList(IngredientData data)
    {
        GameObject g = Instantiate(temp, panel);
        g.GetComponentInChildren<Text>().text = data.thName;
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
