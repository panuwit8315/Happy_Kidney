using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fridge : MonoBehaviour //default
{
    enum MoveState {DEFAULT, MOVINGPLAY, MOVINGOUT, MOVEDOUT, PLAYERDRAG}

    Vector3 PlayArea = new Vector3(0,-1,0);
    private delegate void Movement();
    private Movement OnMovement;
    [SerializeField] MoveState moveState = MoveState.MOVINGPLAY;

    [SerializeField] float moveSpeed;

    [SerializeField] bool isComplete = false;
    [SerializeField] bool isSetOriginalPos = false;
    [SerializeField] GameObject fridgeDoor;
    [SerializeField] GameObject spawnPoints;
    [SerializeField] int fridgeLv;
    [SerializeField] GameObject ingredientPrefab;
    [SerializeField] List<GameObject> ingredientObjs = new List<GameObject>();
    List<GameObject> spawnList = new List<GameObject>();
    List<GameObject> spawnObstaclePoint = new List<GameObject>();
    [SerializeField] GameObject obstaclePrefab;
    public bool nextSpawn = true;

    void Start()
    {
        OnMovement += DefaultMove;
        OnMovement += MoveToPlayArea;
    }

    public void Setup(int fridgeLv, PlayDifference playDiff)
    {
        fridgeDoor.SetActive(fridgeLv == 1);
        gameObject.name = "Fridge Lv" + fridgeLv;
        this.fridgeLv = fridgeLv;

        DataManager dataM = Game.GetInstance().dataManager;
        DifferenceLvData diff = dataM.GetDifferenceByLy(fridgeLv, playDiff);

        //Setup FridgeShelfStyle
        FridgeShelf fridgeShelf = dataM.GetFridgeShelf(diff.style);
        GetComponent<SpriteRenderer>().sprite = fridgeShelf.fridgeSprite;
        spawnPoints = Instantiate(fridgeShelf.spawnPoint, transform);

        //Setup Ingrediant
        int canEat = diff.canEatIngrediant;
        int notEat = diff.shouldNotEatIngrediant;

        foreach (Transform spawn in spawnPoints.transform.GetComponentsInChildren<Transform>())
        {   
            if(spawn != spawnPoints.transform)spawnList.Add(spawn.gameObject);           
        }

        List<IngredientData> ingredientData_canEat = new List<IngredientData>();
        foreach(IngredientData data in dataM.GetIngredientData(IngredientType.CAN_EAT))
        {
            ingredientData_canEat.Add(data);
        }

        List<IngredientData> ingredientData_notEat = new List<IngredientData>();
        foreach (IngredientData data in dataM.GetIngredientData(IngredientType.SHOULD_NOT_EAT))
        {
            ingredientData_notEat.Add(data);
        }

        int allSpawn = canEat + notEat;
        if (allSpawn > spawnPoints.transform.childCount) Debug.LogWarning("จำนวนที่ต้อง spawn มากว่าจุด spawn: Lv" + fridgeLv);
        for(int i = 0; i < allSpawn; i++)
        {          
            if (i < canEat)
            {
                int random = Random.Range(0, ingredientData_canEat.Count);
                SpawnIngredient(ingredientData_canEat[random],i);
                ingredientData_canEat.RemoveAt(random);
            }
            else
            {
                int random = Random.Range(0, ingredientData_notEat.Count);
                SpawnIngredient(ingredientData_notEat[random],i);
                ingredientData_notEat.RemoveAt(random);
            }
        }

        //Setup Obstacle
        foreach(GameObject ingre in ingredientObjs)
        {
            spawnObstaclePoint.Add(ingre);
        }
        int obsCount = 0;
        for(int ice = diff.obstacle_Ice; ice > 0; ice--)
        {
            obsCount++;
            SpwanObstacle(ObstacleType.ICE, obsCount);
        }
        for (int spiderWeb = diff.obstacle_SpiderWeb; spiderWeb > 0; spiderWeb--)
        {
            obsCount++;
            SpwanObstacle(ObstacleType.SPIDERWEB, obsCount);
        }
        for (int woodenBoard = diff.obstacle_WoodenBoard; woodenBoard > 0; woodenBoard--)
        {
            obsCount++;
            SpwanObstacle(ObstacleType.WOODENBOARD, obsCount);
        }
    }

    private void Update()
    {
        OnMovement();
        if (transform.position.x < -4 && moveState == MoveState.PLAYERDRAG)
        {
            moveState = MoveState.MOVINGOUT;
            OnMovement += MoveOut;
        }
        else if (transform.position.x >= -4 && moveState == MoveState.PLAYERDRAG)
        {
            moveState = MoveState.MOVINGPLAY;
            OnMovement += MoveToPlayArea;
        }    
    }

    //----------------------Movement----------------------//
    public void OnDrag(float xPos)
    {
        if (xPos >= 0 || moveState == MoveState.MOVINGOUT || !isComplete) return;
        if (moveState == MoveState.MOVINGPLAY)
        {
            OnMovement -= MoveToPlayArea;          
        }
        moveState = MoveState.PLAYERDRAG;
        Vector3 newPos = new Vector3(xPos, transform.position.y, transform.position.z);
        transform.position = newPos;
    }

    private void MoveToPlayArea()
    {
        moveState = MoveState.MOVINGPLAY;
        Vector3 dir = Vector3.zero;
        if(transform.position.x > PlayArea.x)
        {
            dir = new Vector3(-1, 0, 0);
        }
        else if(transform.position.x < PlayArea.x)
        {
            //dir = new Vector3(1, 0, 0);
            transform.position = PlayArea;
        }
        
        transform.Translate(dir * Time.deltaTime * moveSpeed);
        if(Vector3.Distance(transform.position, PlayArea) < 0.5)
        {
            transform.position = PlayArea;
            OnMovement -= MoveToPlayArea;
            moveState = MoveState.DEFAULT;
            if(fridgeLv > 1 && !isSetOriginalPos)
            {
                OpenTheFridge();
            }
        }
    }

    private void MoveOut()
    {
        if (moveState == MoveState.MOVEDOUT) return;
        moveState = MoveState.MOVINGOUT;
        Vector3 dir = new Vector3(-1, 0, 0);
        transform.Translate(dir * Time.deltaTime * moveSpeed);
        if(transform.position.x < -10)
        {
            moveState = MoveState.MOVEDOUT;
            if(nextSpawn) Game.GetInstance().fridgeSpawner.LetSpawner();
            Destroy(gameObject);
        }
    }

    void DefaultMove()
    {
        //Do Nothing
    }

    //---------------------End Movement-------------------//

    public void OpenTheFridge()
    {
        fridgeDoor.SetActive(false);
        SetOriginalPos();
        EnableAllIngredintCol(true);

        //Complete();
    }

    void SetOriginalPos()
    {
        foreach(GameObject g in ingredientObjs) { g.GetComponent<Ingredient>().SetOriginalPos(); }
        isSetOriginalPos = true;
    }

    void Complete()
    {
        //isComplete = true;
        //GetComponent<BoxCollider2D>().enabled = true;
        moveState = MoveState.MOVINGOUT;
        OnMovement += MoveOut;
        EnableAllIngredintCol(false);
        Debug.Log("Fridge Complete");
    }

    void SpawnIngredient(IngredientData data,int count)
    {
        int random = Random.Range(0, spawnList.Count);
        GameObject g = Instantiate(ingredientPrefab, spawnList[random].transform.position, spawnList[random].transform.rotation);
        g.transform.SetParent(spawnList[random].transform);
        g.name = count.ToString("00")+" :" + data.engName +":"+data.type;
        spawnList.RemoveAt(random);
        ingredientObjs.Add(g);
        g.GetComponent<Ingredient>().Setup(data);
    }

    private void SpwanObstacle(ObstacleType obstacleType,int count)
    {
        int random = Random.Range(0, spawnObstaclePoint.Count);
        Transform transform = spawnObstaclePoint[random].transform.parent;
        GameObject g = Instantiate(obstaclePrefab, transform);
        g.name = count.ToString("00") + " :" + obstacleType;
        g.GetComponent<Obstacle>().SetUp(obstacleType, spawnObstaclePoint[random]);       
        print("SpwanObstacle :" + obstacleType);

        spawnObstaclePoint.RemoveAt(random);
    }

    public void EnableAllIngredintCol(bool set)
    {
        foreach (GameObject g in ingredientObjs) 
        {
            if (g != null)
            {
                BoxCollider2D col = g.GetComponent<BoxCollider2D>();
                if (!g.GetComponent<Ingredient>().isBehindObstacle)
                {
                    col.enabled = set;
                }
                else
                {
                    col.enabled = false;
                }
                
            }
        }
    }

    public void DisableOtherIngrediantCol(string name)
    {
        foreach (GameObject g in ingredientObjs) 
        {
            if (g == null) continue;
            if (g.name == name)
            {
                g.GetComponent<BoxCollider2D>().enabled = true;
                g.GetComponent<SpriteRenderer>().sortingOrder = 10;
            }
            else
            {
                g.GetComponent<BoxCollider2D>().enabled = false;
                g.GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
        }
    }

    public void CheckIngrediant()
    {
        foreach(GameObject ingrediant in ingredientObjs)
        {
            if(ingrediant != null)
            {
                Ingredient ingre = ingrediant.GetComponent<Ingredient>();
                if (ingre.GetIngrediantType() == IngredientType.SHOULD_NOT_EAT)
                {
                    if (!ingre.isDroped)return;                
                }
            }
        }

        Complete();
    }

    public void Out()
    {
        moveState = MoveState.MOVINGOUT;
        OnMovement += MoveOut;
    }
}
