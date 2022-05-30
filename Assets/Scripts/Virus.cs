using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : MonoBehaviour
{
    protected GameObject cell;
    protected GameObject GenerationPlace;

    protected int Speed { get; set; }
    protected int DirectionChange { get; set; }
    public float HP { get; set; }
    float hpChanger;
    int numPrefab;
    Quaternion direction;
    bool NewDirection;
    // Start is called before the first frame update
    void Start()
    {
        //setting variables
        numPrefab = 2;
        Speed = PlayerPrefs.GetInt("SpeedVirus");
        hpChanger = -0.25f;
        DirectionChange = 1000;
        GenerationPlace = GameObject.Find("GenerationPlace");

        //Add physic and cell transform
        cell.transform.localScale = new Vector3(1f, 1f, 1f);
        cell.AddComponent<Movement>().setParams(Speed);
        cell.AddComponent<ColliderEvent>().Params(cell, false);
    }
    private void FixedUpdate()
    {
        //Prefab changing
        //if (Camera.main.transform.position.y < 150.0f && PlayerPrefs.GetInt("NeedDoDetailingVir") == 0)
        //{
        //    DetailedPrefabs();
        //    PlayerPrefs.SetInt("NeedDoSimplificationVir", 0);
        //}
        //else if (Camera.main.transform.position.y >= 150.0f && PlayerPrefs.GetInt("NeedDoSimplificationVir") == 0)
        //{
        //    SimplePrefabs();
        //    PlayerPrefs.SetInt("NeedDoDetailingVir", 0);
        //}
        //Hp changing
        HP += hpChanger;
    }
    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (HP <= 0)
                DeathOfCell(cell);
        }
    }
    void DeathOfCell(GameObject thisObject)
    {
        if (Camera.main.transform.position.y >= 150.0f)
            GenerationPlace.GetComponent<CreateNewObj>().RecreateObjDead(thisObject, thisObject.name + "_Dead", "virusDead", numPrefab, false); //bacteriaDead
        else
            GenerationPlace.GetComponent<CreateNewObj>().RecreateObjDead(thisObject, thisObject.name + "_Dead", "virusDeadDetailed", numPrefab, true);  //bacteriaDetailed   == bacteria + DeadDetailed
    }

    //void DetailedPrefabs()
    //{
    //    switch (this.cell.tag)
    //    {
    //        case "virus":
    //            PlayerPrefs.SetInt("NeedDoDetailingVir", 0);
    //            break;
    //        default:
    //            //print("U do some mistake!");
    //            PlayerPrefs.SetInt("NeedDoDetailingVir", 1);
    //            break;
    //    }
    //    if (PlayerPrefs.GetInt("NeedDoDetailingVir") == 0)
    //        GenerationPlace.GetComponent<CreateNewObj>().RecreateObj(this.cell, this.cell.name, this.cell.tag + "Detailed", numPrefab, true, HP, false, cell.GetComponent<MotionSphere>().direction, 0);
    //}
    //void SimplePrefabs()
    //{
    //    switch (this.cell.tag)
    //    {
    //        case "virusDetailed":
    //            PlayerPrefs.SetInt("NeedDoSimplificationVir", 0);
    //            break;

    //        default:
    //            //print("U do some mistake!");
    //            PlayerPrefs.SetInt("NeedDoSimplificationVir", 1);
    //            break;
    //    }
    //    if (PlayerPrefs.GetInt("NeedDoSimplificationVir") == 0)
    //        GenerationPlace.GetComponent<CreateNewObj>().RecreateObj(this.cell, this.cell.name, this.cell.tag.Remove(this.cell.tag.Length - 8), numPrefab, false, HP, false, cell.GetComponent<MotionSphere>().direction, 0);
    //}

    public void SetParams(GameObject obj, string tag, string name, float hp, bool newdirection, Quaternion direction)
    {
        //setting params
        cell = obj;
        cell.tag = tag;
        cell.name = name;
        HP = hp;
        NewDirection = newdirection;
        this.direction = direction;
    }
}