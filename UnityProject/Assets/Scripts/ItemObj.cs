using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObj : MonoBehaviour
{
    public ItemType itemType;
    GameObject currentObstacle;

    public void OnDrag(float xPos, float yPos)
    {
        Vector3 newPos = new Vector3(xPos, yPos, transform.position.z);
        transform.position = newPos;
    }

    public void SetType(ItemType type)
    {
        Animator anim = GetComponent<Animator>();
        itemType = type;
        switch (type)
        {
            case ItemType.FIRE: anim.SetTrigger("Fire"); break;
            case ItemType.BROOM: anim.SetTrigger("Broom"); break;
            case ItemType.CROWBAR: anim.SetTrigger("Crowbar"); break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            currentObstacle = collision.gameObject;
        }      
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            if(currentObstacle != null)
            {
                if(collision.gameObject.name == currentObstacle.name)
                {
                    currentObstacle = null;
                }
            }           
        }
    }

    public void ActiveItem()
    {
        Game game = Game.GetInstance();
        FridgeSpawner spawner = game.fridgeSpawner;
        if (currentObstacle == null) return;
        if (!game.timer.isRunTime) return;
        if (spawner.currentFridgeObj == null) return;

        //ActiveItem
        Obstacle obstacle = currentObstacle.GetComponent<Obstacle>();
        if((int)itemType == (int)obstacle.type) //type ตรงกัน ใช้ item สำเร็จ
        {
            obstacle.EnableColBehindIngredient();
            // + Score
            spawner.AddScoreAfterUseItem(true);
        }
        else //ใช้ item ไม่สำเร็จ
        {
            // - Score
            spawner.AddScoreAfterUseItem(false);
        }
        
    }
}
