using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreEukaryote : MonoBehaviour
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
    int numPrefab;
    public bool hasDaughter;
    //сохранение направления движения при смене префаба
    bool newDirection;
    Quaternion direction;
    bool isFullEukaryote;
    int daughterCount;
    // int Numprefab;

    public void ChangeVariablehasDaughter()
    {
        hasDaughter = false;
        daughterCount++;
    }
    void Start()
    {
        //setting variables
        // numPrefab = 0;
        Speed = 10;
        hpChanger = -0.001f;
      
        maxScale = numPrefab == 5 ? 13f : 8f;
        scaleChanger = 0.0045f;
        scaleBuffer = 0;
        DirectionChange = 2000;
        GenerationPlace = GameObject.Find("GenerationPlace");


        cell.transform.localScale = new Vector3(LocalScale, LocalScale, LocalScale);


        cell.AddComponent<Movement>().setParams(Speed);

        cell.AddComponent<ColliderEvent>().Params(cell, false);
    }
    private void FixedUpdate()
    {
    //    if (Camera.main.transform.position.y < 500f && PlayerPrefs.GetInt("NeedDoDetailingEu") == 0)
    //    {
    //        DetailedPrefabs();
    //        PlayerPrefs.SetInt("NeedDoSimplificationEu", 0);
    //    }
    //    else if (Camera.main.transform.position.y >= 500.0f && PlayerPrefs.GetInt("NeedDoSimplificationEu") == 0)
    //    {
    //        SimplePrefabs();
    //        PlayerPrefs.SetInt("NeedDoDetailingEu", 0);
    //    }

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
                if (Camera.main.transform.position.y < 350f)
                {
                    if (numPrefab == 5)
                        GenerationPlace.GetComponent<CreateNewObj>().CreateSubsidiaryCell(cell, "eukaryoteDetailed", 3, true, 0f);
                    else
                        GenerationPlace.GetComponent<CreateNewObj>().CreateSubsidiaryEukaryote(cell, numPrefab + 1, 0f, true);
                }
                else
                {
                    if (numPrefab == 5)
                        GenerationPlace.GetComponent<CreateNewObj>().CreateSubsidiaryCell(cell, "eukaryote", 3, false, 0f);
                    else
                        GenerationPlace.GetComponent<CreateNewObj>().CreateSubsidiaryEukaryote(cell, numPrefab + 1, 0f, false);
                }
                hasDaughter = true;
            }
            
        }
    }


    public void ScaleChanger(float localScale) {
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
            GenerationPlace.GetComponent<CreateNewObj>().RecreateObjDead(thisObject, thisObject.name + "_Dead", "preEukaryoteDead", numPrefab < 5 ? 0 : 3, false); //bacteriaDead
        else
            GenerationPlace.GetComponent<CreateNewObj>().RecreateObjDead(thisObject, thisObject.name + "_Dead", "preEukaryoteDeadDetailed", numPrefab < 5 ? 0 : 3, true);  //bacteriaDetailed   == bacteria + DeadDetailed
    }

    //void DetailedPrefabs()
    //{
    //    switch (this.cell.tag)
    //    {
    //        case "preEukaryote":
    //            PlayerPrefs.SetInt("NeedDoDetailingEu", 0);
    //            break;
    //        default:
    //            //print("U do some mistake!");
    //            PlayerPrefs.SetInt("NeedDoDetailingEu", 1);
    //            break;
    //    }

    //    if (PlayerPrefs.GetInt("NeedDoDetailingEu") == 0)
    //        GenerationPlace.GetComponent<CreateNewObj>().RecreateObjEukaryote(this.cell, this.cell.name, this.cell.tag + "Detailed", numPrefab, true, HP, hasDaughter, cell.GetComponent<Motion>().direction, daughterCount);
    //}
    //void SimplePrefabs()
    //{

    //    switch (this.cell.tag)
    //    {
    //        case "preEukaryoteDetailed":
    //            PlayerPrefs.SetInt("NeedDoSimplificationEu", 0);
    //            break;

    //        default:
    //            //print("U do some mistake!");
    //            PlayerPrefs.SetInt("NeedDoSimplificationEu", 1);
    //            break;
    //    }

    //    if (PlayerPrefs.GetInt("NeedDoSimplificationEu") == 0)
    //        GenerationPlace.GetComponent<CreateNewObj>().RecreateObjEukaryote(this.cell, this.cell.name, this.cell.tag.Remove(this.cell.tag.Length - 8), numPrefab, false, HP, hasDaughter, cell.GetComponent<Motion>().direction, daughterCount);
    //}


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
      //  isFullEukaryote = full;
    }
}
