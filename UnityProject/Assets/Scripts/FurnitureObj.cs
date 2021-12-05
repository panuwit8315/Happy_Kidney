using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureObj : MonoBehaviour
{
    [SerializeField] FurnitureData.Furniture data;
    [SerializeField] Button outlineBtn;
    [SerializeField] bool canBuyNext;

    public void Setup(FurnitureData.Furniture data)
    {
        this.data = data;

        Image image = GetComponent<Image>();
        image.enabled = true;
        if (data.sprite != null) image.sprite = data.sprite;
        else image.enabled = false;

        canBuyNext = false;

        if ((int)data.style < (int)FurnitureStyle.PIRATES)
        {
            int next = (int)data.style;
            next++;
            FurnitureData.Furniture nextFurniture = Game.GetInstance().dataManager.GetFurnitureData((FurnitureStyle)next,data.type);
            SetOutlineButton(nextFurniture);
            canBuyNext = true;
        }
    }

    void SetOutlineButton(FurnitureData.Furniture nextFurnitureData)
    {
        if(outlineBtn != null)
        {
            outlineBtn.onClick.RemoveAllListeners();
            outlineBtn.onClick.AddListener(() =>
            {
                SoundManager.GetInstance().PlaySFXOneShot(SfxClipName.CLICK02);
                UIManager.GetUI().UILobby().OpenBuyPanel(nextFurnitureData);
            });
        }
    }

    public void ShowOutline(bool isSet)
    {
        if (isSet) outlineBtn.gameObject.SetActive(canBuyNext);
        else outlineBtn.gameObject.SetActive(false);
    }
}
