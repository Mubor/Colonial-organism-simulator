using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infectedCell : MonoBehaviour
{
    protected bool isLive = true;

    protected int Speed { get; set; }
    [SerializeField]
    protected int InfectLvl;
    protected int DirectionChange { get; set; }
    protected float LocalScale { get; set; }
    protected GameObject infectCell { get; set; }
    protected Vector3 CurrentPosition { get; set; }

    GameObject[] simpleCells;
    GameObject[] detailCells;
    GameObject[] infectedCells;
    GameObject GenerationPlace;

    Quaternion direction;


    private void Awake()
    {
        simpleCells = Resources.LoadAll<GameObject>("SimpleCells");
        detailCells = Resources.LoadAll<GameObject>("DetailedCells");
        infectedCells = Resources.LoadAll<GameObject>("InfectedCells");
    }

    void Start()
    {
        GenerationPlace = GameObject.Find("GenerationPlace");
        this.infectCell.transform.localScale = new Vector3(LocalScale, LocalScale, LocalScale);

        switch (infectCell.tag)
        {
            case "bacteriaInfected":
                AddPhysic(this.infectCell, true);
                break;
            case "bacteriaInfectedDetailed":
                AddPhysic(this.infectCell, true);
                break;
            case "cianobacteriaInfected":
                AddPhysic(this.infectCell, false);
                break;
            case "cianobacteriaInfectedDetailed":
                AddPhysic(this.infectCell, false);
                break;
            case "eukaryoteInfected":
                AddPhysic(this.infectCell, true);
                break;
            case "eukaryoteInfectedDetailed":
                AddPhysic(this.infectCell, true);
                break;
            default:
                //print("U'do some mistake!");
                break;
        }
    }

    private void Update()
    {
        if (Time.timeScale != 0)
        {
            if (InfectLvl == 6 && (this.infectCell.tag == "bacteriaInfected" || this.infectCell.tag == "bacteriaInfectedDetailed"))
            {
                if (Camera.main.transform.position.y >= 350)
                    DeathOfCell(this.infectCell, InfectLvl, false);
                else
                    DeathOfCell(this.infectCell, InfectLvl, true);

                isLive = false;
            }

            else if (InfectLvl == 11 && (this.infectCell.tag == "cianobacteriaInfected" || this.infectCell.tag == "cianobacteriaInfectedDetailed"))
            {
                if (Camera.main.transform.position.y >= 350)
                    DeathOfCell(this.infectCell, InfectLvl, false);
                else
                    DeathOfCell(this.infectCell, InfectLvl, true);

                isLive = false;
            }
            else if (InfectLvl == 16)
            {
                if (Camera.main.transform.position.y >= 350)
                    DeathOfCell(this.infectCell, InfectLvl, false);
                else
                    DeathOfCell(this.infectCell, InfectLvl, true);

                isLive = false;
            }

            if (Random.Range(0, 500) == 0 && isLive)
            {
                if (Camera.main.transform.position.y >= 350.0f)
                    InfectionChange(this.infectCell, false, this.infectCell.tag, InfectLvl);
                else
                    InfectionChange(this.infectCell, true, this.infectCell.tag, InfectLvl);
            }
        }
    }

    void FixedUpdate()
    {
        CurrentPosition = transform.position;

        //if (Camera.main.transform.position.y < 350 && PlayerPrefs.GetInt("NeedDoDetailingInf") == 0)
        //{
        //    DetailedPrefabs();
        //    PlayerPrefs.SetInt("NeedDoSimplificationInf", 0);
        //}

        //if (Camera.main.transform.position.y >= 350 && PlayerPrefs.GetInt("NeedDoSimplificationInf") == 0)
        //{
        //    SimplePrefabs();
        //    PlayerPrefs.SetInt("NeedDoDetailingInf", 0);
        //}
    }

    private void InfectionChange(GameObject thisObject, bool zoom, string tag, int infLvl)
    {
        //Сохранение параметров объекта
        var transform = thisObject.transform;
        var name = thisObject.name;
        float scale = LocalScale;
        int simpleInd = infLvl <= 6 ? 0 : infLvl <= 11 ? 1 : 3;

        //Уничтожение старого префаба
        Destroy(thisObject.gameObject);

        //Замена префаба на инфицированный
        thisObject = Instantiate(zoom ? infectedCells[infLvl] : simpleCells[simpleInd], transform.position, transform.rotation) as GameObject;
        thisObject.tag = tag;
        thisObject.name = name;

        //Добавление условий движения
        thisObject.AddComponent<infectedCell>().setParams(thisObject, thisObject.tag, thisObject.name, scale, 10, 1000, infLvl += 1, direction);
    }

    void DeathOfCell(GameObject thisObject, int infLvl, bool zoom)
    {
        var transform = thisObject.transform;

        if (infLvl == 6)
            GenerationPlace.GetComponent<CreateNewObj>().RecreateObjDead(thisObject, thisObject.name + "_Dead", zoom ? "bacteriaDeadDetailed" : "bacteriaDead", 0, zoom);
        else if (infLvl == 11)
            GenerationPlace.GetComponent<CreateNewObj>().RecreateObjDead(thisObject, thisObject.name + "_Dead", zoom ? "cianobacteriaDeadDetailed" : "cianobacteriaDead", 1, zoom);
        else
            GenerationPlace.GetComponent<CreateNewObj>().RecreateObjDead(thisObject, thisObject.name + "_Dead", zoom ? "eukaryoteDeadDetailed" : "eukaryoteDead", 3, zoom);


        for (int i = 0; i < Random.Range(5, 9); i++)
        {
            var newObject = Instantiate(zoom ? detailCells[2] : simpleCells[2], new Vector3(transform.position.x + 10.0f, 0, transform.position.z), Quaternion.identity) as GameObject;
            newObject.name = "virus_" + PlayerPrefs.GetInt("CountVirus");
            PlayerPrefs.SetInt("CountVirus", PlayerPrefs.GetInt("CountVirus") + 1);
            newObject.tag = zoom ? "virusDetailed" : "virus";

            newObject.AddComponent<Virus>().SetParams(newObject, newObject.tag, newObject.name, Random.Range(80, 101), true, Quaternion.identity);
        }
    }

    void AddPhysic(GameObject obj, bool isCapsule)
    {
       obj.AddComponent<Movement>().setParams(Speed);
    }

    //void DetailedPrefabs()
    //{
    //    switch (this.infectCell.tag)
    //    {
    //        case "bacteriaInfected":
    //            PlayerPrefs.SetInt("NeedDoDetailingInf", 0);
    //            break;
    //        case "cianobacteriaInfected":
    //            PlayerPrefs.SetInt("NeedDoDetailingInf", 0);
    //            break;
    //        case "eukaryoteInfected":
    //            PlayerPrefs.SetInt("NeedDoDetailingInf", 0);
    //            break;
    //        default:
    //            //print("U do some mistake!");
    //            PlayerPrefs.SetInt("NeedDoDetailingInf", 1);
    //            break;
    //    }

    //    if (PlayerPrefs.GetInt("NeedDoDetailingInf") == 0)
    //    {
    //        direction = infectCell.GetComponent<Movement>().targetRot;
    //        GenerationPlace.GetComponent<CreateNewObj>().RecreateObjInfected(this.infectCell, this.infectCell.transform, this.infectCell.name, this.infectCell.tag + "Detailed", Speed, DirectionChange, true, InfectLvl, direction);
    //    }

    //}

    //void SimplePrefabs()
    //{
    //    switch (this.infectCell.tag)
    //    {
    //        case "bacteriaInfectedDetailed":
    //            PlayerPrefs.SetInt("NeedDoSimplificationInf", 0);
    //            break;
    //        case "cianobacteriaInfectedDetailed":
    //            PlayerPrefs.SetInt("NeedDoSimplificationInf", 0);
    //            break;
    //        case "eukaryoteInfectedDetailed":
    //            PlayerPrefs.SetInt("NeedDoSimplificationInf", 0);
    //            break;
    //        default:
    //            //print("U do some mistake!");
    //            PlayerPrefs.SetInt("NeedDoSimplificationInf", 1);
    //            break;
    //    }

    //    if (PlayerPrefs.GetInt("NeedDoSimplificationInf") == 0)
    //    {
    //        if (InfectLvl <= 6 || InfectLvl >= 12)
    //            direction = infectCell.GetComponent<Motion>().direction;
    //        else
    //            direction = infectCell.GetComponent<MotionSphere>().direction;
    //        GenerationPlace.GetComponent<CreateNewObj>().RecreateObjInfected(this.infectCell, this.infectCell.transform, this.infectCell.name, this.infectCell.tag.Remove(this.infectCell.tag.Length - 8), Speed, DirectionChange, false, InfectLvl, direction);
    //    }
    //}
    public void setParams(GameObject gameObject, string tag, string name, float localScale, int speed, int dir, int infLvl, Quaternion direction)
    {
        Speed = speed;
        LocalScale = localScale;
        DirectionChange = dir;
        this.infectCell = gameObject;
        InfectLvl = infLvl;
        this.infectCell.tag = tag;
        this.infectCell.name = name;
        this.direction = direction;
    }
}