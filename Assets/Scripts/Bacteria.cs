using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bacteria : MonoBehaviour
{
    protected GameObject cell;
    protected GameObject GenerationPlace;
    protected GameObject canvas;
    protected int Speed { get; set; }
    protected int DirectionChange { get; set; } // Верхняя граница рандомизации расстояния
    protected float LocalScale { get; set; }
    public float HP { get; set; }
    float hpChanger;
    float maxScale;
    float scaleChanger;
    float scaleBuffer;
    int numPrefab;
    public bool hasDaughter;


    //сохранение направления движения при смене префаба
    bool newDirection;
    Quaternion direction;
    int daughterCount;

    public void ChangeVariablehasDaughter()
    {
        hasDaughter = false;
        daughterCount++;
    }

    private void Awake()
    {
        
    }

    void Start()
    {
        //setting variables
        numPrefab = 0;
        Speed = PlayerPrefs.GetInt("SpeedBacteria");
        hpChanger = -0.001f;
        maxScale = 8.0f;
        scaleChanger = 0.002f;
        scaleBuffer = 0;
        DirectionChange = 2000;
        GenerationPlace = GameObject.Find("GenerationPlace");
        canvas = GameObject.Find("Canvas");

        cell.transform.localScale = new Vector3(LocalScale, LocalScale, LocalScale);

        cell.AddComponent<Movement>().setParams(Speed);
        cell.AddComponent<ColliderEvent>().Params(cell, true);
    }

    private void FixedUpdate()
    {
        if (Camera.main.transform.position.y < 500f && PlayerPrefs.GetInt("NeedDoDetailingBac") == 0)
        {
            DetailedPrefabs();
            PlayerPrefs.SetInt("NeedDoSimplificationBac", 0);
        }
        else if (Camera.main.transform.position.y >= 500.0f && PlayerPrefs.GetInt("NeedDoSimplificationBac") == 0)
        {
            SimplePrefabs();
            PlayerPrefs.SetInt("NeedDoDetailingBac", 0);
        }

        HP += hpChanger;


        if (HP >= 100 && !hasDaughter)
            scaleBuffer += scaleChanger;
        if (scaleBuffer >= 0.2f)
        {
            LocalScale += scaleBuffer;
            cell.transform.localScale = new Vector3(LocalScale, LocalScale, LocalScale);
            scaleBuffer = 0;
        }

    }

    void Update()
    {
        
        if (Time.timeScale != 0)
        {
            if (HP <= 0 || daughterCount == 2)
                DeathOfCell(cell);
            canvas.GetComponent<World>().OtherGazesUp(0.5f);
            if (cell.transform.localScale.x >= maxScale && !hasDaughter)
            {
                if (Camera.main.transform.position.y >= 350.0f)
                    GenerationPlace.GetComponent<CreateNewObj>().CreateSubsidiaryCell(cell, cell.tag, 0, false, 0f);
                else
                    GenerationPlace.GetComponent<CreateNewObj>().CreateSubsidiaryCell(cell, cell.tag, 0, true, 0f);
                hasDaughter = true;

            }
        }
    }

    public void ScaleChanger(float localScale)
    {
        LocalScale += localScale;
        cell.transform.localScale = new Vector3(LocalScale, LocalScale, LocalScale);
    }
    public void ChangeHP(float hpChanger)
    {
        HP += hpChanger;
    }
    void DeathOfCell(GameObject thisObject)
    {
        if (Camera.main.transform.position.y >= 150.0f)  //Камера выше 350
            GenerationPlace.GetComponent<CreateNewObj>().RecreateObjDead(thisObject, thisObject.name + "_Dead", "bacteriaDead", numPrefab, false); //bacteriaDead
        else
            GenerationPlace.GetComponent<CreateNewObj>().RecreateObjDead(thisObject, thisObject.name + "_Dead", "bacteriaDeadDetailed", numPrefab, true);  //bacteriaDetailed   == bacteria + DeadDetailed

    }
    void DetailedPrefabs()
    {
        switch (this.cell.tag)
        {
            case "bacteria":
                PlayerPrefs.SetInt("NeedDoDetailingBac", 0);
                break;
            default:
                //print("U do some mistake!");
                PlayerPrefs.SetInt("NeedDoDetailingBac", 1);
                break;
        }

        if (PlayerPrefs.GetInt("NeedDoDetailingBac") == 0)
            GenerationPlace.GetComponent<CreateNewObj>().RecreateObj(this.cell, this.cell.name, this.cell.tag + "Detailed", numPrefab, true, HP, hasDaughter, cell.GetComponent<Movement>().targetRot, daughterCount);
    }
    void SimplePrefabs()
    {

        switch (this.cell.tag)
        {
            case "bacteriaDetailed":
                PlayerPrefs.SetInt("NeedDoSimplificationBac", 0);
                break;

            default:
                //print("U do some mistake!");
                PlayerPrefs.SetInt("NeedDoSimplificationBac", 1);
                break;
        }

        if (PlayerPrefs.GetInt("NeedDoSimplificationBac") == 0)
            GenerationPlace.GetComponent<CreateNewObj>().RecreateObj(this.cell, this.cell.name, this.cell.tag.Remove(this.cell.tag.Length - 8), numPrefab, false, HP, hasDaughter, cell.GetComponent<Movement>().targetRot, daughterCount);
    }

    public void SetParams(GameObject obj, string tag, string name, float hp, float scale, bool hasdaughter, bool newdirection, Quaternion direction, int daughtercnt)
    {
        if (hp <= 0)
            obj.transform.Rotate(0, Random.Range(0, 361f), 0);

        //setting params
        cell = obj;
        LocalScale = scale;
        cell.tag = tag;
        cell.name = name;
        HP = hp;
        hasDaughter = hasdaughter;
        newDirection = newdirection;
        daughterCount = daughtercnt;
        this.direction = direction;
            
    }
}