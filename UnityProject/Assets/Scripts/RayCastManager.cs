using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastManager : MonoBehaviour
{
    float xOfset; //of raycast to fridge and ingredente
    float yOfset; //of raycast to ingredente

    [SerializeField] GameObject currentHitObj;

    readonly string fridgeTag = "Fridge";
    readonly string ingredenteTag = "Ingredente";
    readonly string openFridgeTag = "OpenFridge";
    readonly string itemTag = "Item";

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                xOfset = hit.collider.transform.position.x - hit.point.x;
                yOfset = hit.collider.transform.position.y - hit.point.y;
                currentHitObj = hit.collider.gameObject;

                if (currentHitObj.CompareTag(ingredenteTag))
                {
                    Game.GetInstance().fridgeSpawner.GetFridgeObj().DisableOtherIngrediantCol(currentHitObj.name);
                    UIManager.GetUI().UIGamePlay().EnableColAllItem(false);
                    UIManager.GetUI().UIGamePlay().MoveInRubbishBin();
                    
                    UITutorial tutorial = UIManager.GetUI().UITutorial();
                    if(tutorial != null)
                    {
                        tutorial.OnClickIngreObj();
                        currentHitObj.GetComponent<SpriteRenderer>().sortingLayerName = "Popup";
                    }
                    else
                    {
                        currentHitObj.GetComponent<SpriteRenderer>().sortingLayerName = "Obstacle";
                    }
                }
                else if (currentHitObj.CompareTag(itemTag))
                {
                    if (currentHitObj.GetComponent<ItemBtn>().enableItem)
                    {
                        UIManager.GetUI().UIGamePlay().DisableColOtherItem(currentHitObj.name);
                        Game.GetInstance().fridgeSpawner.GetFridgeObj().EnableAllIngredintCol(false);
                        currentHitObj.GetComponent<ItemBtn>().SpawnItemObj();
                    }                   
                }
            }
        }

        else if (Input.GetMouseButton(0))
        {
            if(currentHitObj != null)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                GameObject obj = currentHitObj;
                if (obj.CompareTag(fridgeTag))
                {
                    float newX = mousePos.x + xOfset;
                    obj.GetComponent<Fridge>().OnDrag(newX);
                }
                else if (obj.CompareTag(ingredenteTag))
                {
                    float newX = mousePos.x + xOfset;
                    float newY = mousePos.y + yOfset;
                    obj.GetComponent<Ingredient>().OnDrag(newX, newY);
                }
                else if (obj.CompareTag(itemTag))
                {
                    ItemBtn btn = obj.GetComponent<ItemBtn>();
                    if (btn.enableItem)
                    {
                        float newX = mousePos.x + xOfset;
                        float newY = mousePos.y + yOfset;
                        btn.currentItemObj.GetComponent<ItemObj>().OnDrag(newX, newY);
                    }                  
                }
            }          
        }

        else if(Input.GetMouseButtonUp(0) && currentHitObj != null)
        {
            if (currentHitObj.CompareTag(openFridgeTag))
            {
                currentHitObj.GetComponentInParent<Fridge>().OpenTheFridge();
            }
            else if (currentHitObj.CompareTag(ingredenteTag))
            {
                
                UITutorial tutorial = UIManager.GetUI().UITutorial();
                if (tutorial != null)
                {
                    currentHitObj.GetComponent<SpriteRenderer>().sortingLayerName = "Popup";
                    Game.GetInstance().fridgeSpawner.GetFridgeObj().EnableAllIngredintCol(true,true);
                }
                else
                {
                    currentHitObj.GetComponent<SpriteRenderer>().sortingLayerName = "Ingredente";
                    Game.GetInstance().fridgeSpawner.GetFridgeObj().EnableAllIngredintCol(true);
                }
                
                currentHitObj.GetComponent<Ingredient>().BackToSpawnPoint();
                UIManager.GetUI().UIGamePlay().MoveOutRubbishBin();
                UIManager.GetUI().UIGamePlay().EnableColAllItem(true);                
            }
            else if (currentHitObj.CompareTag(itemTag))
            {
                if (currentHitObj.GetComponent<ItemBtn>().enableItem) 
                {
                    UIManager.GetUI().UIGamePlay().EnableColAllItem(true);
                    currentHitObj.GetComponent<ItemBtn>().currentItemObj.GetComponent<ItemObj>().ActiveItem();
                    currentHitObj.GetComponent<ItemBtn>().DestroyItemObj();
                    if(UIManager.GetUI().UITutorial() == null) Game.GetInstance().fridgeSpawner.GetFridgeObj().EnableAllIngredintCol(true);
                }              
            }
            currentHitObj = null;
        }
        
    }

    public void Clear()
    {
        if(currentHitObj != null)
        {
            Destroy(currentHitObj);
            currentHitObj = null;
        }
    }
}
