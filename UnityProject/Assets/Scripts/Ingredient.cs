using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Ingredient : MonoBehaviour
{
    public IngredientData data;

    Vector3 originalPos;
    bool inRubbishBin = false;
    public bool isDroped = false;
    public bool isBehindObstacle = false;

    string binName;

    //public delegate void OnBackToSpawnPoint(bool isbin);
    //public OnBackToSpawnPoint onBackToSpawnPoint;
    //void NotDoing(bool isbin) { }

    public void Setup(IngredientData data)
    {
        this.data = data;
        GetComponent<SpriteRenderer>().sprite = data.sprite;
        //onBackToSpawnPoint+=NotDoing;
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

            if(binName == "RubbishBinL")
            {
                Analytics.CustomEvent("RubbishL");
                print("RubbishBinL Success");
            }
            else if (binName == "RubbishBinR")
            {
                Analytics.CustomEvent("RubbishR");
                print("RubbishBinR Success");
            }

            if (data.type == IngredientType.SHOULD_NOT_EAT)
            {
                spawner.AddIngredientCount(1);
                spawner.AddDataIngredientThrowAway(data);
                spawner.GetFridgeObj().CheckIngrediant();
            }

            UITutorial tutorial = UIManager.GetUI().UITutorial();
            if (tutorial != null)
            {
                tutorial.IngreBackToSpawnPoint();
            }
        }
        //onBackToSpawnPoint(inRubbishBin);
        
    }

    public void SetOriginalPos()
    {
        originalPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("RubbishBin"))
        {
            binName = collision.gameObject.name;
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
