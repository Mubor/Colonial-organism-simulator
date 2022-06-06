using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CianoBacteria : MonoBehaviour
{
    protected GameObject GenerationPlace;
    protected GameObject virusPrefab;
    protected Material deadCellMaterial;

    public GameObject Parent;

    protected int Speed { get; set; }
    protected float LocalScale { get; set; }
    public float HP { get; set; }
    public float hpChanger;

    int daughterCount;
    int infectLevel;
    bool isAlive;


    // Start is called before the first frame update
    void Start()
    {
        deadCellMaterial = Resources.LoadAll<Material>("Materials")[6];
        virusPrefab = Resources.LoadAll<GameObject>("Virus")[0];

        Speed = PlayerPrefs.GetInt("SpeedCiano");
        GenerationPlace = GameObject.Find("GenerationPlace");

        infectLevel = 0;
        isAlive = true;
        hpChanger = 0.01f;

        SetSize(LocalScale);

        gameObject.AddComponent<Movement>().setParams(Speed);
        gameObject.AddComponent<ColliderEvent>().Params(true);
    }

    private void FixedUpdate()
    {
        if (Camera.main.transform.position.y < 350.0f && PlayerPrefs.GetInt("NeedDoDetailingCiano") == 0)
        {
            gameObject.transform.GetChild(2).gameObject.SetActive(true);
            PlayerPrefs.SetInt("NeedDoSimplificationCiano", 0);
        }
        else if (Camera.main.transform.position.y >= 350.0f && PlayerPrefs.GetInt("NeedDoSimplificationCiano") == 0)
        {
            gameObject.transform.GetChild(2).gameObject.SetActive(false);
            PlayerPrefs.SetInt("NeedDoDetailingCiano", 0);
        }

        if (isAlive && infectLevel == 0)
        {
            HP += hpChanger;

            if (HP >= 100)
            {
                SetSize(LocalScale + 0.001f);
            }
        }
    }

    void Update()
    {
        if (Time.timeScale != 0 && isAlive)
        {
            if (HP <= 0 || daughterCount == 2)
            {
                DeathOfCell();
            }

            if (gameObject.transform.localScale.x >= 4.5f && infectLevel == 0)
            {
                Born();
            }

            //Infect level rise (more viruses inside)
            if( Random.Range(0, 200) == 0 && infectLevel > 0)
            {
                Infect();
            }

            if(infectLevel > 7)
            {
                for (int i = 0; i < 8; i++)
                {
                    GenerationPlace.GetComponent<CreateNewObj>().CreateSubsidiaryCell(gameObject, 2);
                }
                DeathOfCell(true);
            }
        }
    }
   
    public void HPChanger(bool time)
    {
        hpChanger = time ? 0.01f : -0.01f;
    }

    public void Infect()
    {
        GameObject virus = Instantiate(virusPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;

        virus.transform.SetParent(gameObject.transform, false);
        virus.transform.localPosition = new Vector3(Random.Range(-0.400f, 0.400f), Random.Range(-0.400f, 0.400f), Random.Range(-0.400f, 0.400f));

        infectLevel++;
        gameObject.tag = "cianobacteriaInfected";
    }

    void SetSize(float scale)
    {
        LocalScale = scale;
        gameObject.transform.localScale = new Vector3(LocalScale, LocalScale, LocalScale);
    }

    void DeathOfCell(bool deathByInfection = false)
    {
        Destroy(gameObject.GetComponent<Movement>());
        gameObject.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material = deadCellMaterial;

        if(deathByInfection)
        {
            for (int i = 3; i < gameObject.transform.childCount; i++)
            {
                Destroy(gameObject.transform.GetChild(i).gameObject);
            }
        }

        isAlive = false;
    }

    void Born()
    {
        SetSize(3f);
        GenerationPlace.GetComponent<CreateNewObj>().CreateSubsidiaryCell(gameObject, 1);
        daughterCount++;
    }

    public void SetParams(GameObject parent)
    {
        PlayerPrefs.SetInt("CountCianobacteria", PlayerPrefs.GetInt("CountCianobacteria") + 1);
        gameObject.name = "cianobacteria_" + PlayerPrefs.GetInt("CountCianobacteria");
        gameObject.transform.Rotate(0, Random.Range(-180, 180), 0);

        Parent = parent;
        LocalScale = 3f;
        HP = Random.Range(-10f, 81f);
    }
}