using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IngredientData
{
    public string engName;
    public string thName;
    public Sprite sprite;
    public string dec;
    public IngredientType type;
}

[System.Serializable]
public class DifferenceLvData
{
    public FridgeShelfStyle style;
    public int shouldNotEatIngrediant;
    public int canEatIngrediant;
    public int obstacle_Ice;
    public int obstacle_SpiderWeb;
    public int obstacle_WoodenBoard;
}

[System.Serializable]
public class ComboDifference
{
    public int maxCombo;
    public int addTime;
    public int addScore;
    public BonusType bonusType;
}

[System.Serializable]
public class FridgeShelf
{
    public Sprite fridgeSprite;
    public GameObject spawnPoint;
    public FridgeShelfStyle style;
}

[System.Serializable]
public class LevelUnlockItem
{
    public int unlockFire;
    public int unlockBroom;
    public int unlockCrowbar;

    public LevelUnlockItem() { unlockFire = 0; unlockBroom = 0; unlockCrowbar = 0; }
    public LevelUnlockItem(int unlockFire, int unlockBroom, int unlockCrowbar) 
    { 
        this.unlockFire = unlockFire; 
        this.unlockBroom = unlockBroom; 
        this.unlockCrowbar = unlockCrowbar; 
    }
}

public enum IngredientType {SHOULD_NOT_EAT, CAN_EAT}
public enum PlayDifference { NORMAL, HARD}
public enum ItemType { FIRE, BROOM, CROWBAR }
public enum ObstacleType { ICE, SPIDERWEB, WOODENBOARD }
public enum BonusType { X1 = 1, X2 = 2, X3 = 3, X4 = 4 }
public enum FridgeShelfStyle { EZ1_5, EZ6_10, EZ11_15, EZ16_20, EZ21_25, EZ26_30, EZ31_35, EZ36_40, EZ41_45, EZ46_50,
                               H1_5, H6_10, H11_15, H16_20, H21_25, H26_30, H31_35, H36_40, H41_45, H46_50 }

public class DataManager : MonoBehaviour
{
    [SerializeField] List<IngredientData> ingredientData_SHOULD_NOT_EAT;
    [SerializeField] List<IngredientData> ingredientData_CAN_EAT;

    [SerializeField] List<DifferenceLvData> normalDifLv;
    [SerializeField] List<DifferenceLvData> hardDifLv;

    [SerializeField] List<ComboDifference> comboDiff;

    [SerializeField] List<FridgeShelf> fridgeShelfs;

    public List<IngredientData> GetIngredientData(IngredientType type)
    {
        if (type == IngredientType.CAN_EAT) return ingredientData_CAN_EAT;
        else if (type == IngredientType.SHOULD_NOT_EAT) return ingredientData_SHOULD_NOT_EAT;
        return null;

    }

    public DifferenceLvData GetDifferenceByLy(int fridgeLv, PlayDifference diff)
    {
        if (diff == PlayDifference.NORMAL)
        {
            if (fridgeLv > normalDifLv.Count - 1)
            {
                Debug.LogError("ตู้เย็นเวเวล" + fridgeLv+" ระดับความยาก"+ diff + "-->ยังไม่ได้เซ็ตจำนวนวัตถุดิบ");
                return normalDifLv[normalDifLv.Count - 1];
            }
            return normalDifLv[fridgeLv];

        }
        else if (diff == PlayDifference.HARD)
        {
            if (fridgeLv > hardDifLv.Count - 1)
            {
                Debug.LogError("ตู้เย็นเวเวล" + fridgeLv + " ระดับความยาก" + diff + "-->ยังไม่ได้เซ็ตจำนวนวัตถุดิบ");
                return hardDifLv[hardDifLv.Count - 1];
            }
            return hardDifLv[fridgeLv];
        }
        return null;
    }

    public ComboDifference GetComboDiff(int stepComplete)
    {
        if(stepComplete < comboDiff.Count)
        {
            return comboDiff[stepComplete];
        }
        else
        {
            return comboDiff[comboDiff.Count - 1];
        }
    }

    public FridgeShelf GetFridgeShelf(FridgeShelfStyle style)
    {
        foreach(FridgeShelf shelf in fridgeShelfs)
        {
            if (style == shelf.style) return shelf;
        }
        Debug.LogError("ไม่มีชั้นวางแบบนี้ [" + style +"] ในฐานข้อมูล");
        return fridgeShelfs[0];
    }

    public LevelUnlockItem GetLevelUnlockItem()
    {
        LevelUnlockItem unlockItem = new LevelUnlockItem();
        for(int i = 0; i < normalDifLv.Count; i++)
        {
            if (normalDifLv[i].obstacle_Ice > 0 && unlockItem.unlockFire == 0) unlockItem.unlockFire = i;
            if (normalDifLv[i].obstacle_SpiderWeb > 0 && unlockItem.unlockBroom == 0) unlockItem.unlockBroom = i;
            if (normalDifLv[i].obstacle_WoodenBoard > 0 && unlockItem.unlockCrowbar == 0) unlockItem.unlockCrowbar = i;
            if (unlockItem.unlockFire > 0 && unlockItem.unlockBroom > 0 && unlockItem.unlockCrowbar > 0) return unlockItem;
        }

        Debug.LogError("GetLevelUnlockItem In DataManager Item:");
        return unlockItem;
    }
}
