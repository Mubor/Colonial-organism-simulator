using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ColliderEvent : MonoBehaviour
{
    protected bool trigger;
    protected bool iscapsule;
    GameObject thisObject;
    GameObject[] simpleCells;
    GameObject[] infectedCells;
    GameObject[] eukaryotesEvo;
    GameObject generationPlace;

    private void Awake()
    {
        simpleCells = Resources.LoadAll<GameObject>("SimpleCells");
        infectedCells = Resources.LoadAll<GameObject>("InfectedCells");
        eukaryotesEvo = Resources.LoadAll<GameObject>("EukariothEvolution");
        generationPlace = GameObject.Find("GenerationPlace");
    }

    void Start()
    {
        switch (thisObject.tag)
        {
            case "bacteria":
                iscapsule = true;
                break;
            case "bacteriaInfected":
                iscapsule = true;
                break;
            case "bacteriaDead":
                iscapsule = true;
                break;
            case "bacteriaDetailed":
                iscapsule = true;
                break;
            case "bacteriaInfectedDetailed":
                iscapsule = true;
                break;
            case "bacteriaDeadDetailed":
                iscapsule = true;
                break;
            case "eukaryote":
                iscapsule = true;
                break;
            case "eukaryoteInfected":
                iscapsule = true;
                break;
            case "eukaryoteDead":
                iscapsule = true;
                break;
            case "eukaryoteDetailed":
                iscapsule = true;
                break;
            case "eukaryoteInfectedDetailed":
                iscapsule = true;
                break;
            case "eukaryoteDeadDetailed":
                iscapsule = true;
                break;
            default:
                iscapsule = false;
                break;
        }
    }

    void Update()
    {

    }

    public void Params(GameObject obj, bool IsTrigger)
    {
        thisObject = obj;
        trigger = IsTrigger;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (Time.timeScale != 0)
        {
            if (trigger && (collision.tag == "virusDetailed" || collision.tag == "virus"))// условие столкновение другой обьект - вирус
            {
                if ((thisObject.tag == "bacteria" || thisObject.tag == "bacteriaDetailed") && Random.Range(0, 2) == 0) // возможность мутации вируса внутри в ядро если обьект - бактерия
                {
                    if (Camera.main.transform.position.y < 500f)
                        DoMutation(thisObject, collision, true);
                    else
                        DoMutation(thisObject, collision, false);
                }
                else // заражение вирусом обьектов
                {
                    if (Camera.main.transform.position.y < MaxHeight(thisObject))
                        DoInfection(thisObject, collision, true, thisObject.tag);
                    else
                        DoInfection(thisObject, collision, false, thisObject.tag);
                }
            }
            if (trigger && (thisObject.tag == "bacteria" || thisObject.tag == "bacteriaDetailed") && (collision.tag != "virusDetailed" || collision.tag != "virus"))// условие столкновения бактерии с другим мертвым обьектом
            {
                if (collision.tag == "virusDead" || collision.tag == "virusDeadDetailed")
                    DoEating(thisObject, collision, 10);
                else if (collision.tag == "cianobacteriaDead" || collision.tag == "cianobacteriaDeadDetailed")
                    DoEating(thisObject, collision, 50);
                else if (collision.tag == "bacteriaDead" || collision.tag == "bacteriaDeadDetailed")
                    DoEating(thisObject, collision, 100);
            }
            else if (trigger && (thisObject.tag == "eukaryote" || thisObject.tag == "eukaryoteDetailed") && (collision.tag != "virusDetailed" || collision.tag != "virus") && (collision.tag != "eukaryote" || collision.tag != "eukaryoteDetailed"))// столкновение еукариоты с другим обьектом
            {
                if ((collision.tag == "bacteria" || collision.tag == "bacteriaDetailed") && thisObject.GetComponent<Eukaryote>().numPrefab < 5) // если столкновение было с бактерией, то добавляется митохондрия в еукариоте (если ее нету)
                    thisObject.GetComponent<Eukaryote>().addMitohondrias(collision.gameObject);
                else // иначе поедание еукариотой даного обьекта
                    DoHunt(thisObject, collision, 20);
            }
        }
    }
    private float MaxHeight(GameObject obj)
    {
        
        if (obj.tag == "cianobacteria" || obj.tag == "cianobacteriaDetailed")
            return 350f;
        else
            return 500f;
    }

    private void DoEating(GameObject thisObject, Collider collision, float hpChanger)
    {
        Destroy(collision.gameObject);

        thisObject.GetComponent<Bacteria>().ChangeHP(hpChanger);
    }
    private void DoHunt(GameObject thisObject, Collider collision, float hpChanger)
    {
        Destroy(collision.gameObject);

        thisObject.GetComponent<Eukaryote>().ChangeHP(hpChanger);
    }
    private void DoInfection(GameObject thisObject, Collider collision, bool zoom, string tag)
    {
        //Сохранение параметров объекта
        var transform = thisObject.transform;
        var name = thisObject.name;
        int prefabInd = (tag == "bacteria" || tag == "bacteriaDetailed") ? 0 : (tag == "cianobacteria" || tag == "cianobacteriaDetailed" )? 7 : 12;
        float scale = thisObject.transform.localScale.x;
        int simpleInd = prefabInd == 0 ? 0 : prefabInd == 7 ? 1 : 3;
        Quaternion direction = thisObject.GetComponent<Movement>().targetRot;

        if (prefabInd == 12 && (thisObject.tag == "eukaryote" || thisObject.tag == "eukaryoteDetailed"))
        {
            if (thisObject.GetComponent<Eukaryote>().numPrefab != 5)
                return;
        }

        //Уничтожение старого префаба и вируса который с ним столкнулся
        Destroy(thisObject.gameObject);
        Destroy(collision.gameObject);

        //Замена префаба на инфицированный
        thisObject = Instantiate(zoom == true ? infectedCells[prefabInd] : simpleCells[simpleInd], transform.position, transform.rotation);
        thisObject.tag = zoom ? tag.Remove(tag.Length -8) + "InfectedDetailed" : tag + "Infected";
        thisObject.name = name;
        //Добавление условий движения
        thisObject.AddComponent<infectedCell>().setParams(thisObject, thisObject.tag, thisObject.name, scale, 10, 200, prefabInd, direction);

        //thisObject.AddComponent<infectedCell>().Params(thisObject, prefabInd);
    }
    private void DoMutation(GameObject thisObject, Collider collision, bool zoom)
    {
        var transform = thisObject.transform;
        //var name = thisObject.name;
        float scale = thisObject.transform.localScale.x;
        Quaternion direction = thisObject.GetComponent<Movement>().targetRot;

        //Уничтожение старого префаба и вируса который с ним столкнулся
        Destroy(thisObject.gameObject);
        Destroy(collision.gameObject);

        //Замена префаба на инфицированный
        thisObject = Instantiate(zoom ? eukaryotesEvo[0] : simpleCells[0], transform.position, transform.rotation);
        thisObject.tag = zoom ? "preEukaryoteDetailed" : "preEukaryote";
        thisObject.name = "eukaryote_" + PlayerPrefs.GetInt("CountEukaryote");
        PlayerPrefs.SetInt("CountEukaryote", PlayerPrefs.GetInt("CountEukaryote") + 1);
        //Добавление условий движения
        thisObject.AddComponent<PreEukaryote>().SetParams(thisObject, thisObject.tag, thisObject.name, 120, scale, false, false, direction, 0, 0);
    }
}