using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCell : MonoBehaviour
{
    protected GameObject cell;
    protected GameObject canvas;
    protected GameObject GenerationPlace;
    protected float LocalScale { get; set; }
    protected int lenght = 0;
    int numPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //setting variables
        //numPrefab = 0;
        GenerationPlace = GameObject.Find("GenerationPlace");
        canvas = GameObject.Find("Canvas");

        cell.transform.localScale = new Vector3(LocalScale, LocalScale, LocalScale);

        cell.AddComponent<ColliderEvent>().Params(cell, false);



    }
    private void FixedUpdate()
    {
        //if (Camera.main.transform.position.y < 150f && PlayerPrefs.GetInt("NeedDoDetailingDead") == 0)
        //{
        //    DetailedPrefabs();
        //    PlayerPrefs.SetInt("NeedDoSimplificationDead", 0);
        //}
        //else if (Camera.main.transform.position.y >= 150f && PlayerPrefs.GetInt("NeedDoSimplificationDead") == 0)
        //{
        //    SimplePrefabs();

        //    PlayerPrefs.SetInt("NeedDoDetailingDead", 0);
        //}

    }
    // Update is called once per frame
    //void Update()
    //{

    //}
    private float MaxHeight(GameObject obj)
    {
        if (obj.tag == "bacteriaDead" || obj.tag == "bacteriaDeadDetailed")
            return 500f;
        else if ((obj.tag == "virusDead" || obj.tag == "virusDeadDetailed"))
            return 100f;
        else
            return 300f;
    }
    //void DetailedPrefabs()
    //{
    //    switch (this.cell.tag)
    //    {
    //        case "bacteriaDead":
    //            PlayerPrefs.SetInt("NeedDoDetailingDead", 0);
    //            break;
    //        case "virusDead":
    //            PlayerPrefs.SetInt("NeedDoDetailingDead", 0);
    //            break;
    //        case "cianobacteriaDead":
    //            PlayerPrefs.SetInt("NeedDoDetailingDead", 0);
    //            break;
    //        case "eukaryoteDead":
    //            PlayerPrefs.SetInt("NeedDoDetailingDead", 0);
    //            break;
    //        case "preEukaryoteDead":
    //            PlayerPrefs.SetInt("NeedDoDetailingDead", 0);
    //            break;
    //        default:
    //            //print("U do some mistake!");
    //            PlayerPrefs.SetInt("NeedDoDetailingDead", 1);
    //            break;
    //    }

    //    if (PlayerPrefs.GetInt("NeedDoDetailingDead") == 0)
    //    {

    //        GenerationPlace.GetComponent<CreateNewObj>().RecreateObjDead(this.cell, this.cell.name, this.cell.tag + "Detailed", numPrefab, true);

    //    }
    //}
    //void SimplePrefabs()
    //{
    //    switch (this.cell.tag)
    //    {
    //        case "bacteriaDeadDetailed":
    //            PlayerPrefs.SetInt("NeedDoSimplificationDead", 0);
    //            break;
    //        case "virusDeadDetailed":
    //            PlayerPrefs.SetInt("NeedDoSimplificationDead", 0);
    //            break;
    //        case "cianobacteriaDeadDetailed":
    //            PlayerPrefs.SetInt("NeedDoSimplificationDead", 0);
    //            break;
    //        case "eukaryoteDeadDetailed":
    //            PlayerPrefs.SetInt("NeedDoSimplificationDead", 0);
    //            break;
    //        case "preEukaryoteDeadDetailed":
    //            PlayerPrefs.SetInt("NeedDoSimplificationDead", 0);
    //            break;
    //        default:
    //            //print("U do some mistake!");
    //            PlayerPrefs.SetInt("NeedDoSimplificationDead", 1);
    //            break;
    //    }

    //    if (PlayerPrefs.GetInt("NeedDoSimplificationDead") == 0)
    //    {

    //        GenerationPlace.GetComponent<CreateNewObj>().RecreateObjDead(this.cell, this.cell.name, this.cell.tag.Remove(this.cell.tag.Length - 8), numPrefab, false);

    //    }
    //}

    public void SetParams(GameObject obj, string tag, string name, float scale, int numprefab)
    {
        //setting params
        cell = obj;
        LocalScale = scale;
        cell.tag = tag;
        cell.name = name;
        numPrefab = numprefab;
    }
}