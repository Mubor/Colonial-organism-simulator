using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bacteria : MonoBehaviour
{
    protected GameObject GenerationPlace;
    protected Material deadCellMaterial;
    protected GameObject virusPrefab;


    protected int Speed { get; set; }
    protected float LocalScale { get; set; }
    public float HP { get; set; }

    float hpChanger;
    int daughterCount;
    int infectLevel;
    bool isAlive;

    void Start()
    {
        deadCellMaterial = Resources.LoadAll<Material>("Materials")[6];
        virusPrefab = Resources.LoadAll<GameObject>("Virus")[0];

        Speed = PlayerPrefs.GetInt("SpeedBacteria");
        GenerationPlace = GameObject.Find("GenerationPlace");

        isAlive = true;
        hpChanger = -0.001f;
        infectLevel = 0;

        SetSize(LocalScale);

        //gameObject.AddComponent<Movement>().setParams(Speed);
        gameObject.AddComponent<ColliderEvent>().Params(true);
    }

    private void FixedUpdate()
    {
        if (Camera.main.transform.position.y < 500f && PlayerPrefs.GetInt("NeedDoDetailingBac") == 0)
        {
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
            PlayerPrefs.SetInt("NeedDoSimplificationBac", 0);
        }
        else if (Camera.main.transform.position.y >= 500.0f && PlayerPrefs.GetInt("NeedDoSimplificationBac") == 0)
        {
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
            PlayerPrefs.SetInt("NeedDoDetailingBac", 0);
        }

        if(isAlive)
        {
            HP += hpChanger;
            if (HP >= 100)
            {
                SetSize(LocalScale + 0.002f);
            }
        }
    }

    void Update()
    {
        if (Time.timeScale != 0 && isAlive)
        {
            if (HP <= 0 || daughterCount == 2)
                DeathOfCell();

            if (gameObject.transform.localScale.x >= 8.0f)
                Born();

            if (Random.Range(0, 200) == 0 && infectLevel > 0)
            {
                Infect();
            }

            if (infectLevel > 7)
            {
                for (int i = 0; i < 8; i++)
                {
                    GenerationPlace.GetComponent<CreateNewObj>().CreateSubsidiaryCell(gameObject, 2);
                }
                DeathOfCell(true);
            }
        }
    }

    public void ChangeHP(float hpChanger)
    {
        HP += hpChanger;
    }

    void SetSize(float scale)
    {
        LocalScale = scale;
        gameObject.transform.localScale = new Vector3(LocalScale, LocalScale, LocalScale);
    }

    void DeathOfCell(bool deathByInfection = false)
    {
        Destroy(gameObject.GetComponent<Movement>());
        gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = deadCellMaterial;

        if (deathByInfection)
        {
            for (int i = 2; i < gameObject.transform.childCount; i++)
            {
                Destroy(gameObject.transform.GetChild(i).gameObject);
            }
        }

        isAlive = false;
    }

    void Born()
    {
        SetSize(5f);
        GenerationPlace.GetComponent<CreateNewObj>().CreateSubsidiaryCell(gameObject, 0);
        daughterCount++;
    }

    public void Infect()
    {
        GameObject virus = Instantiate(virusPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;

        virus.transform.SetParent(gameObject.transform, false);
        virus.transform.localPosition = new Vector3(Random.Range(0.700f, 2.850f), Random.Range(-0.200f, 0.200f), Random.Range(-0.250f, 0.250f));

        infectLevel++;
        gameObject.tag = "bacteriaInfected";
    }

    public void SetParams()
    { 
        PlayerPrefs.SetInt("CountBacteria", PlayerPrefs.GetInt("CountBacteria") + 1);
        gameObject.name = "bacteria_" + PlayerPrefs.GetInt("CountBacteria");
        gameObject.transform.Rotate(0, Random.Range(-180, 180), 0);

        LocalScale = 5f;
        HP = Random.Range(-10f, 101f);

       
    }
}