using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : MonoBehaviour
{
    protected Material deadCellMaterial;

    protected int Speed { get; set; }
    public float HP { get; set; }

    bool isAlive;

    void Start()
    {
        deadCellMaterial = Resources.LoadAll<Material>("Materials")[6];
        Speed = PlayerPrefs.GetInt("SpeedVirus");
        isAlive = true;

        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        gameObject.AddComponent<Movement>().setParams(Speed);
        gameObject.AddComponent<ColliderEvent>().Params(false);
    }
    private void FixedUpdate()
    {
        //Prefab changing
        if (Camera.main.transform.position.y < 150.0f && PlayerPrefs.GetInt("NeedDoDetailingVir") == 0)
        {
            gameObject.transform.GetChild(2).gameObject.SetActive(true);
            PlayerPrefs.SetInt("NeedDoSimplificationVir", 0);
        }
        else if (Camera.main.transform.position.y >= 150.0f && PlayerPrefs.GetInt("NeedDoSimplificationVir") == 0)
        {
            gameObject.transform.GetChild(2).gameObject.SetActive(false);
            PlayerPrefs.SetInt("NeedDoDetailingVir", 0);
        }

        if(isAlive)
            HP += -0.25f;
    }
    void Update()
    {
        if (Time.timeScale != 0 && isAlive)
        {
            if (HP <= 0)
                DeathOfCell();
        }
    }
    void DeathOfCell()
    {
        Destroy(gameObject.GetComponent<Movement>());
        gameObject.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material = deadCellMaterial;
        isAlive = false;
    }

    public void SetParams()
    {
        PlayerPrefs.SetInt("CountVirus", PlayerPrefs.GetInt("CountVirus") + 1);

        gameObject.name = "virus_" + PlayerPrefs.GetInt("CountVirus");
        gameObject.transform.Rotate(0, Random.Range(-180, 180), 0);

        HP = Random.Range(50f, 100f);
    }
}