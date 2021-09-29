using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public ObstacleType type;
    GameObject behindIngredientObj;

    [SerializeField] Sprite ice_Sprite;
    [SerializeField] Sprite spiderWeb_Sprite;
    [SerializeField] Sprite woodenBoard_Sprite;

    public void SetUp(ObstacleType type, GameObject behindIngredientObj)
    {
        this.type = type;
        this.behindIngredientObj = behindIngredientObj;
        this.behindIngredientObj.GetComponent<Ingredient>().isBehindObstacle = true;
        SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();

        switch (type)
        {
            case ObstacleType.ICE: sprite.sprite = ice_Sprite; break;
            case ObstacleType.SPIDERWEB: sprite.sprite = spiderWeb_Sprite; break;
            case ObstacleType.WOODENBOARD: sprite.sprite = woodenBoard_Sprite; break;
        }
    }

    public void EnableColBehindIngredient()
    {
        behindIngredientObj.GetComponent<Ingredient>().isBehindObstacle = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
