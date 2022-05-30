using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CianoBacteria : MonoBehaviour
{
    protected GameObject cell;
    protected GameObject canvas;
    protected GameObject GenerationPlace;

    protected int Speed { get; set; }
    protected int DirectionChange { get; set; }
    protected float LocalScale { get; set; }
    public float HP { get; set; }
    public float hpChanger;
    float maxScale;
    float scaleChanger;
    float scaleBuffer;
    int numPrefab;
    bool timechanged;
    //cell born
    public bool hasDaughter;
    bool newDirection;
    Quaternion direction;
    int daughterCount;

    public void ChangeVariablehasDaughter()
    {
        hasDaughter = false;
        daughterCount++;
    }
    // Start is called before the first frame update
    void Start()
    {
        //setting variables
        numPrefab = 1;
        Speed = PlayerPrefs.GetInt("SpeedCiano");
        hpChanger = 0.01f;
        maxScale = 4.5f;
        scaleChanger = 0.001f;
        scaleBuffer = 0;
        DirectionChange = 1000;
        GenerationPlace = GameObject.Find("GenerationPlace");
        canvas = GameObject.Find("Canvas");

        //Add physic and cell transform
        cell.transform.localScale = new Vector3(LocalScale, LocalScale, LocalScale);
        cell.AddComponent<Movement>().setParams(Speed);
        cell.AddComponent<ColliderEvent>().Params(cell, true);
    }
    private void FixedUpdate()
    {

        //Prefab changing
        //if (Camera.main.transform.position.y < 350.0f && PlayerPrefs.GetInt("NeedDoDetailingCiano") == 0)
        //{
        //    DetailedPrefabs();
        //    PlayerPrefs.SetInt("NeedDoSimplificationCiano", 0);
        //}
        //else if (Camera.main.transform.position.y >= 350.0f && PlayerPrefs.GetInt("NeedDoSimplificationCiano") == 0)
        //{
        //    SimplePrefabs();
        //    PlayerPrefs.SetInt("NeedDoDetailingCiano", 0);
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
            canvas.GetComponent<World>().OxygenUp(0.5f);
            if (cell.transform.localScale.x >= maxScale && !hasDaughter)
            {
                if (Camera.main.transform.position.y >= 350.0f)
                    GenerationPlace.GetComponent<CreateNewObj>().CreateSubsidiaryCell(cell, cell.tag, 1, false, 0f);
                else
                    GenerationPlace.GetComponent<CreateNewObj>().CreateSubsidiaryCell(cell, cell.tag, 1, true, 0f);
                hasDaughter = true;
            }
            //timechanged = canvas.GetComponent<World>().tick == 0 ? true : false;

            //if (timechanged)
            //{
            //    print("SUKA VSE PRAVILNO");
            //    if (canvas.GetComponent<World>().GetTime())
            //        hpChanger = 0.01f;
            //    else
            //        hpChanger = -0.01f;
            //    timechanged = false;
            //}
            
        }
    }
    //для доступа изменения размера из скрипта дочерней клетки
    public void ScaleChanger(float localScale)
    {
        LocalScale += localScale;
        cell.transform.localScale = new Vector3(LocalScale, LocalScale, LocalScale);
    }
    public void HPChanger(bool time)
    {
        hpChanger = time ? 0.01f : -0.01f;
    }

    void DeathOfCell(GameObject thisObject)
    {
        if (Camera.main.transform.position.y >= 150.0f)  //Камера выше 350
            GenerationPlace.GetComponent<CreateNewObj>().RecreateObjDead(thisObject, thisObject.name + "_Dead", "cianobacteriaDead", numPrefab, false); //bacteriaDead
        else
            GenerationPlace.GetComponent<CreateNewObj>().RecreateObjDead(thisObject, thisObject.name + "_Dead", "cianobacteriaDeadDetailed", numPrefab, true);  //bacteriaDetailed   == bacteria + DeadDetailed
    }

    //void DetailedPrefabs()
    //{
    //    switch (this.cell.tag)
    //    {
    //        case "cianobacteria":
    //            PlayerPrefs.SetInt("NeedDoDetailingCiano", 0);
    //            break;
    //        default:
    //            //print("U do some mistake!");
    //            PlayerPrefs.SetInt("NeedDoDetailingCiano", 1);
    //            break;
    //    }
    //    if (PlayerPrefs.GetInt("NeedDoDetailingCiano") == 0)
    //        GenerationPlace.GetComponent<CreateNewObj>().RecreateObj(this.cell, this.cell.name, this.cell.tag + "Detailed", numPrefab, true, HP, hasDaughter, cell.GetComponent<MotionSphere>().direction, daughterCount);
    //}
    //void SimplePrefabs()
    //{
    //    switch (this.cell.tag)
    //    {
    //        case "cianobacteriaDetailed":
    //            PlayerPrefs.SetInt("NeedDoSimplificationCiano", 0);
    //            break;

    //        default:
    //            //print("U do some mistake!");
    //            PlayerPrefs.SetInt("NeedDoSimplificationCiano", 1);
    //            break;
    //    }
    //    if (PlayerPrefs.GetInt("NeedDoSimplificationCiano") == 0)
    //        GenerationPlace.GetComponent<CreateNewObj>().RecreateObj(this.cell, this.cell.name, this.cell.tag.Remove(this.cell.tag.Length - 8), numPrefab, false, HP, hasDaughter, cell.GetComponent<MotionSphere>().direction, daughterCount);
    //}

    public void SetParams(GameObject obj, string tag, string name, float hp, float scale, bool hasdaughter, bool newdirection, Quaternion direction, int daughtercnt)
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
        daughterCount = daughtercnt;
    }
}