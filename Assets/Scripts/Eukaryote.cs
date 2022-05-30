using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eukaryote : MonoBehaviour
{
    protected GameObject cell;
    protected GameObject GenerationPlace;

    protected int Speed { get; set; }
    protected int DirectionChange { get; set; }
    protected float LocalScale { get; set; }
    public float HP { get; set; }
    float hpChanger;
    float maxScale;
    float scaleChanger;
    float scaleBuffer;
    public int numPrefab;
    public bool hasDaughter;
    //сохранение направления движения при смене префаба
    bool newDirection;
    Quaternion direction;
    int daughterCount;
    // int Numprefab;

    public void ChangeVariablehasDaughter()
    {
        hasDaughter = false;
        daughterCount++;
    }
    void Start()
    {
        Speed = PlayerPrefs.GetInt("SpeedEu");
        hpChanger = -0.001f;
        maxScale = 13;
        scaleChanger = 0.0015f;
        scaleBuffer = 0;
        DirectionChange = 2000;
        GenerationPlace = GameObject.Find("GenerationPlace");


        cell.transform.localScale = new Vector3(LocalScale, LocalScale, LocalScale);


        cell.AddComponent<Movement>().setParams(Speed);

        cell.AddComponent<ColliderEvent>().Params(cell, true);


    }
    private void FixedUpdate()
    {
        //if (Camera.main.transform.position.y < 500f && PlayerPrefs.GetInt("NeedDoDetailingEu") == 0)
        //{
        //    DetailedPrefabs();
        //    PlayerPrefs.SetInt("NeedDoSimplificationEu", 0);
        //}
        //else if (Camera.main.transform.position.y >= 500.0f && PlayerPrefs.GetInt("NeedDoSimplificationEu") == 0)
        //{
        //    SimplePrefabs();
        //    PlayerPrefs.SetInt("NeedDoDetailingEu", 0);
        //}

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
            if (cell.transform.localScale.x >= maxScale && !hasDaughter)
            {

                if (Camera.main.transform.position.y < 500f)
                {
                    GenerationPlace.GetComponent<CreateNewObj>().CreateSubsidiaryCell(cell, cell.tag, numPrefab, true, 0f);
                }
                else
                    GenerationPlace.GetComponent<CreateNewObj>().CreateSubsidiaryCell(cell, cell.tag, numPrefab, false, 0f);


                hasDaughter = true;
            }
        }
    }

    //void DetailedPrefabs()
    //{
    //    switch (this.cell.tag)
    //    {
    //        case "eukaryote":
    //            PlayerPrefs.SetInt("NeedDoDetailingEu", 0);
    //            break;
    //        default:
    //            PlayerPrefs.SetInt("NeedDoDetailingEu", 1);
    //            break;
    //    }

    //    if (PlayerPrefs.GetInt("NeedDoDetailingEu") == 0)
    //        GenerationPlace.GetComponent<CreateNewObj>().RecreateObj(this.cell, this.cell.name, this.cell.tag + "Detailed", numPrefab, true, HP, hasDaughter, cell.GetComponent<Motion>().direction, daughterCount);
    //}
    //void SimplePrefabs()
    //{

    //    switch (this.cell.tag)
    //    {
    //        case "eukaryoteDetailed":
    //            PlayerPrefs.SetInt("NeedDoSimplificationEu", 0);
    //            break;

    //        default:
    //            PlayerPrefs.SetInt("NeedDoSimplificationEu", 1);
    //            break;
    //    }

    //    if (PlayerPrefs.GetInt("NeedDoSimplificationEu") == 0)
    //        GenerationPlace.GetComponent<CreateNewObj>().RecreateObj(this.cell, this.cell.name, this.cell.tag.Remove(this.cell.tag.Length - 8), numPrefab, false, HP, hasDaughter, cell.GetComponent<Motion>().direction, daughterCount);
    //}
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
            GenerationPlace.GetComponent<CreateNewObj>().RecreateObjDead(thisObject, thisObject.name + "_Dead", "eukaryoteDead", 3, false); //bacteriaDead
        else
            GenerationPlace.GetComponent<CreateNewObj>().RecreateObjDead(thisObject, thisObject.name + "_Dead", "eukaryoteDeadDetailed", 3, true);  //bacteriaDetailed   == bacteria + DeadDetailed

    }

    public void addMitohondrias(GameObject oldObj)
    {
        Destroy(oldObj.gameObject);

        if (Camera.main.transform.position.x >= 500.0f)
            GenerationPlace.GetComponent<CreateNewObj>().RecreateObj(cell, cell.name, cell.tag, numPrefab + 1, false, HP, hasDaughter, cell.GetComponent<Movement>().targetRot, daughterCount);
        else
            GenerationPlace.GetComponent<CreateNewObj>().RecreateObj(cell, cell.name, cell.tag, numPrefab + 1, true, HP, hasDaughter, cell.GetComponent<Movement>().targetRot, daughterCount);

    }

    public void SetParams(GameObject obj, string tag, string name, float hp, float scale, bool hasdaughter, bool newdirection, Quaternion direction, int numprefab, int daughtercnt)
    {
        //setting params
        cell = obj;
        LocalScale = scale;
        cell.tag = tag;
        cell.name = name;
        HP = hp;
        hasDaughter = hasdaughter;
        newDirection = newdirection;
        this.direction = direction;
        numPrefab = numprefab;
        daughterCount = daughtercnt;
    }
}