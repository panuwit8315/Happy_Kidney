using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    //[SerializeField] string engName;
    //[SerializeField] string thName;
    //[SerializeField] string dec;
    //[SerializeField] IngredientType type;
    [SerializeField] IngredientData data;

    Vector3 originalPos;
    bool inRubbishBin = false;
    public bool isDroped = false;
    public bool isBehindObstacle = false;
    public void Setup(IngredientData data)
    {
        //engName = data.engName;
        //thName = data.thName;
        //dec = data.dec;
        //type = data.type;
        this.data = data;
        GetComponent<SpriteRenderer>().sprite = data.sprite;      
    }

    public void OnDrag(float xPos, float yPos)
    {
        Vector3 newPos = new Vector3(xPos, yPos, transform.position.z);
        transform.position = newPos;
    }

    public void BackToSpawnPoint()
    {
        transform.position = originalPos;
        GetComponent<SpriteRenderer>().sortingOrder = 0;
        if (inRubbishBin)
        {
            FridgeSpawner spawner = Game.GetInstance().fridgeSpawner;
            spawner.AddScoreAfterDropIngredient(data.type);
            gameObject.SetActive(data.type == IngredientType.CAN_EAT);
            isDroped = data.type == IngredientType.SHOULD_NOT_EAT;
            //SoundManager sound = SoundManager.GetInstance();
            if (data.type == IngredientType.SHOULD_NOT_EAT)
            {
                spawner.AddIngredientCount(1);
                spawner.AddDataIngredientThrowAway(data);
                spawner.GetFridgeObj().CheckIngrediant();
            }
            /*{
                if (Game.GetInstance().fridgeSpawner.GetFridgeObj().CheckIngrediant())
                {
                    sound.PlaySFXOneShot(SfxClipName.FRIDGECOMPLETE);
                }
                else
                {
                    sound.PlaySFXOneShot(SfxClipName.PICKOUTCORRECT02);
                }
            }
            else
            {
                sound.PlaySFXOneShot(SfxClipName.PICKOUTFAIL01);
            }*/
        }
    }

    public void SetOriginalPos()
    {
        originalPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("RubbishBin"))
        {
            inRubbishBin = true;
            UIManager.GetUI().UIGamePlay().OpenBinSprite(true, collision.gameObject);
            //print(gameObject.name+ " Hit RubbishBin");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("RubbishBin"))
        {
            inRubbishBin = false;
            UIManager.GetUI().UIGamePlay().OpenBinSprite(false,collision.gameObject);
            //print(gameObject.name + " Out RubbishBin");
        }
    }

    public IngredientType GetIngrediantType()
    {
        return data.type;
    }
}
