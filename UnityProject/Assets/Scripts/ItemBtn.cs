using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBtn : MonoBehaviour
{
    GameObject itemObjPrefab;
    public GameObject currentItemObj;
    [SerializeField] ItemType itemType;

    private void Start()
    {
        itemObjPrefab = Resources.Load("Prefabs/ItemObj") as GameObject;
    }

    public void SpawnItemObj()
    {
        currentItemObj = Instantiate(itemObjPrefab, transform);
        currentItemObj.GetComponent<ItemObj>().SetType(itemType);
    }

    /*public void OnDrag(float xPos, float yPos)
    {
        currentItemObj.GetComponent<ItemObj>().OnDrag(xPos, yPos);
    }*/

    public void DestroyItemObj()
    {
        if (currentItemObj == null) return;

        Destroy(currentItemObj);
        currentItemObj = null;
    }
}