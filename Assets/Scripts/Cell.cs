//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
/// <summary>
/// ///////ПЕРЕДАВАТЬ В РЕКРЕЕЙТОБЖЕКТ ХП
/// </summary>
//public class Cell : MonoBehaviour
//{
//    protected float distanceMin = 1000.0f;
//    protected float distanceMax = 5000.0f;
//    protected GameObject cell { get; set; }
//    protected GameObject canvas { get; set; }
//    protected GameObject GenerationPlace;
//    protected int Speed { get; set; }
//    protected int infectLvl;
//    protected int DirectionChange { get; set; }
//    protected float LocalScale { get; set; }
//    protected bool isLive = true;
//    protected bool isNew { get; set; }
//    protected int lenght = 0;
//    protected float HP { get; set; }
//    float hpChanger;
//    float maxScale;
//    float scaleChanger;
//    float scaleBuffer;
//    bool timechanged;

//    private void Awake()
//    {
//        canvas = GameObject.Find("Canvas");
//    }
//    void Start()
//    {
//        scaleBuffer = 0;
//        GenerationPlace = GameObject.Find("GenerationPlace");
//        canvas = GameObject.Find("Canvas");
//        cell.transform.localScale = new Vector3(LocalScale, LocalScale, LocalScale);
//        switch (cell.tag)
//        {
//            case "bacteria":
//                {
//                    AddPhysic(this.cell, true, true, true, -1);
//                    hpChanger = -0.001f;
//                    maxScale = 8.0f;
//                    scaleChanger = 0.002f;
//                }
//                break;
//            case "bacteriaDetailed":
//                {
//                    AddPhysic(this.cell, true, true, true, -1);
//                    hpChanger = -0.001f;
//                    maxScale = 8.0f;
//                    scaleChanger = 0.002f;
//                }
//                break;
//            case "cianobacteria":
//                {
//                    AddPhysic(this.cell, false, true, true, -1);
//                    hpChanger = 0.01f;
//                    maxScale = 4.5f;
//                    scaleChanger = 0.001f;

//                }
//                break;
//            case "cianobacteriaDetailed":
//                {
//                    AddPhysic(this.cell, false, true, true, -1);
//                    hpChanger = 0.01f;
//                    maxScale = 4.5f;
//                    scaleChanger = 0.001f;
//                }
//                break;
//            case "virus":
//                {
//                    AddPhysic(this.cell, false, false, true, -1);
//                    hpChanger = -0.5f;
//                }
//                break;
//            case "virusDetailed":
//                {
//                    AddPhysic(this.cell, false, false, true, -1);
//                    hpChanger = -0.5f;
//                }
//                break;
//            case "bacteriaDead":
//                hpChanger = 0;

//                break;
//            case "cianobacteriaDead":
//                hpChanger = 0;
//                break;
//            case "bacteriaDeadDetailed":
//                hpChanger = 0;
//                break;
//            case "cianobacteriaDeadDetailed":
//                hpChanger = 0;
//                break;
//            case "virusDead":
//                hpChanger = 0;
//                break;
//            case "virusDeadDetailed":
//                hpChanger = 0;
//                break;
//            default:
//                print("U'do some mistake!");
//                break;
//        }
//    }


//    void FixedUpdate()
//    {
//        if (Camera.main.transform.position.y < 350.0f && PlayerPrefs.GetInt("NeedDoDetailing") == 0)
//        {
//            DetailedPrefabs();
//            PlayerPrefs.SetInt("NeedDoSimplification", 0);
//        }
//        else if (Camera.main.transform.position.y >= 350.0f && PlayerPrefs.GetInt("NeedDoSimplification") == 0)
//        {
//            SimplePrefabs();
//            PlayerPrefs.SetInt("NeedDoDetailing", 0);
//        }

//        ChangeHP(hpChanger);


//        if (HP >= 100 && (cell.tag != "virus" || cell.tag != "virusDetailed"))
//            scaleBuffer += scaleChanger;
//        if (scaleBuffer >= 0.2f)
//        {
//            CellGrowing(scaleBuffer);
//            scaleBuffer = 0;
//        }

//        //if (HP >= 100 && (cell.tag != "virus" || cell.tag != "virusDetailed"))
//        //    CellGrowing(scaleChanger);
//    }

//    private void Update()
//    {
//        if (cell.name == "bacteria_6")
//        {
//            Debug.Log("My hp " + HP);
//            Debug.Log("My hpChanger " + hpChanger);
//        }

//        timechanged = canvas.GetComponent<World>().tick == 0 ? true : false;

//        if (cell.tag == "cianobacteria" || cell.tag == "cianobacteriaDetailed")
//            if (timechanged)
//            {
//                print("SUKA VSE PRAVILNO");
//                if (canvas.GetComponent<World>().GetTime())
//                    hpChanger = 0.01f;
//                else
//                    hpChanger = -0.01f;
//                timechanged = false;
//            }

//        if (HP <= 0)
//            DeathOfCell(cell);
//    }

//    public void CellGrowing(float scalechanger)
//    {
//        LocalScale += scalechanger;
//        cell.transform.localScale = new Vector3(LocalScale, LocalScale, LocalScale);
//    }
//    public void ChangeHP(float changer)
//    {
//        print("HP was " + HP);
//        HP += changer;
//        print("HP now " + HP);
//    }
//    добавление скриптов движения и коллайдеров на игровой обьект
//    void AddPhysic(GameObject obj, bool isCapsule, bool IsTrigger, bool isLive, int infLVL)
//    {
//        if (isCapsule)
//            obj.AddComponent<Motion>().Params(Speed, DirectionChange);
//        else
//            obj.AddComponent<MotionSphere>().Params(Speed, DirectionChange);

//        if (obj.tag == "bacteria" || obj.tag == "bacteriaDetailed" || obj.tag == "cianobacteria" || obj.tag == "cianobacteriaDetailed")
//        {
//            obj.AddComponent<ColliderEvent>().Params(obj, IsTrigger);
//            infectLvl = infLVL;
//        }
//        else
//            infectLvl = infLVL;
//    }
//    обработка смерти клетки
//    void DeathOfCell(GameObject thisObject)
//    {
//        //    print(thisObject.tag);
//        //  var transform = thisObject.transform;
//        int numprefab = 0;

//        if (thisObject.tag == "bacteria" || thisObject.tag == "bacteriaDetailed")
//            numprefab = 0;
//        else if (thisObject.tag == "cianobacteria" || thisObject.tag == "cianobacteriaDetailed")
//            numprefab = 1;
//        else if (thisObject.tag == "virus" || thisObject.tag == "virusDetailed")
//            numprefab = 2;

//        if (Camera.main.transform.position.y >= 350.0f)
//            GenerationPlace.GetComponent<CreateNewObj>().RecreateObj(thisObject, thisObject.transform, thisObject.name + "_Dead", thisObject.tag + "Dead", numprefab, false);
//        else
//            GenerationPlace.GetComponent<CreateNewObj>().RecreateObj(thisObject, thisObject.transform, thisObject.name + "_Dead", thisObject.tag.Remove(thisObject.tag.Length - 8) + "DeadDetailed",
//                numprefab, true);

//    }

//    детализация префабов при приближении
//    void DetailedPrefabs()
//    {
//        int numPrefab = 0;

//        switch (this.cell.tag)
//        {
//            case "bacteria":
//                numPrefab = 0;
//                PlayerPrefs.SetInt("NeedDoDetailing", 0);
//                break;
//            case "cianobacteria":
//                numPrefab = 1;
//                PlayerPrefs.SetInt("NeedDoDetailing", 0);
//                break;
//            case "virus":
//                numPrefab = 2;
//                PlayerPrefs.SetInt("NeedDoDetailing", 0);
//                break;
//            case "bacteriaDead":
//                numPrefab = 0;
//                isLive = false;
//                PlayerPrefs.SetInt("NeedDoDetailing", 0);
//                break;
//            case "cianobacteriaDead":
//                numPrefab = 1;
//                isLive = false;
//                PlayerPrefs.SetInt("NeedDoDetailing", 0);
//                break;
//            case "virusDead":
//                numPrefab = 2;
//                PlayerPrefs.SetInt("NeedDoDetailing", 0);
//                break;
//            default:
//                //print("U do some mistake!");
//                PlayerPrefs.SetInt("NeedDoDetailing", 1);
//                break;
//        }

//        if (PlayerPrefs.GetInt("NeedDoDetailing") == 0)
//        {
//            if (isLive)
//                GenerationPlace.GetComponent<CreateNewObj>().RecreateObj(this.cell, this.cell.transform, this.cell.name, this.cell.tag + "Detailed", Speed, this.DirectionChange, numPrefab, true);
//            else
//                GenerationPlace.GetComponent<CreateNewObj>().RecreateObj(this.cell, this.cell.transform, this.cell.name, this.cell.tag + "Detailed", numPrefab, true);
//        }
//    }
//    упрощение префабов при отдалении
//    void SimplePrefabs()
//    {
//        int numPrefab = 0;

//        switch (this.cell.tag)
//        {
//            case "bacteriaDetailed":
//                numPrefab = 0;
//                PlayerPrefs.SetInt("NeedDoSimplification", 0);
//                break;
//            case "cianobacteriaDetailed":
//                numPrefab = 1;
//                PlayerPrefs.SetInt("NeedDoSimplification", 0);
//                break;
//            case "virusDetailed":
//                numPrefab = 2;
//                PlayerPrefs.SetInt("NeedDoSimplification", 0);
//                break;
//            case "bacteriaDeadDetailed":
//                numPrefab = 0;
//                isLive = false;
//                PlayerPrefs.SetInt("NeedDoSimplification", 0);
//                break;
//            case "cianobacteriaDeadDetailed":
//                numPrefab = 1;
//                isLive = false;
//                PlayerPrefs.SetInt("NeedDoSimplification", 0);
//                break;
//            case "virusDeadDetailed":
//                numPrefab = 2;
//                PlayerPrefs.SetInt("NeedDoSimplification", 0);
//                break;
//            default:
//                print("U do some mistake!");
//                PlayerPrefs.SetInt("NeedDoSimplification", 1);
//                break;
//        }

//        if (PlayerPrefs.GetInt("NeedDoSimplification") == 0)
//        {
//            if (isLive)
//                GenerationPlace.GetComponent<CreateNewObj>().RecreateObj(this.cell, this.cell.transform, this.cell.name, this.cell.tag.Remove(this.cell.tag.Length - 8), Speed, this.DirectionChange, numPrefab, false);
//            else
//                GenerationPlace.GetComponent<CreateNewObj>().RecreateObj(this.cell, this.cell.transform, this.cell.name, this.cell.tag.Remove(this.cell.tag.Length - 8), numPrefab, false);
//        }
//    }
//    public void FindCellType(GameObject obj, string tag, string name, bool isLive, float scale, float hp)
//    {
//        switch (tag)
//        {
//            case "bacteria":
//                {
//                    obj.AddComponent<Bacteria>().SetParams(obj, tag, name, hp, scale);
//                }
//                break;
//            case "bacteriaDetailed":
//                {
//                    obj.AddComponent<Bacteria>().SetParams(obj, tag, name, hp, scale);
//                }
//                break;
//            case "cianobacteria":
//                {

//                }
//                break;
//            case "cianobacteriaDetailed":
//                {

//                }
//                break;
//            case "virus":
//                {

//                }
//                break;
//            case "virusDetailed":
//                {

//                }
//                break;
//            case "bacteriaDead":


//                break;
//            case "cianobacteriaDead":

//                break;
//            case "bacteriaDeadDetailed":

//                break;
//            case "cianobacteriaDeadDetailed":

//                break;
//            case "virusDead":
//                break;
//            case "virusDeadDetailed":
//                break;
//            default:
//                print("U'do some mistake!");
//                break;
//        }

//        enabled = false;
//    }
//    установка параметров для навешивания скрипта
//    public void setParams(GameObject gameObject, string tag, string name, bool IsLive, float localScale, int speed, int dir, float hp)
//    {
//        isLive = IsLive;
//        Speed = speed;
//        LocalScale = localScale;
//        DirectionChange = dir;
//        cell = gameObject;
//        HP = hp;
//        this.cell.tag = tag;
//        this.cell.name = name;
//    }


//    public void setParams(GameObject gameObject, string tag, string name, bool IsLive, float localScale, int speed, int dir, bool isNew, float hp)
//    {
//        isLive = IsLive;
//        Speed = speed;
//        LocalScale = localScale;
//        DirectionChange = dir;
//        cell = gameObject;
//        HP = hp;
//        this.cell.tag = tag;
//        this.cell.name = name;
//    }
//}