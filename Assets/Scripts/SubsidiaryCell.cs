using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubsidiaryCell : MonoBehaviour
{
    GameObject parentCell;
    GameObject subsidiaryCell;
    float positionChangerX;
    float positionChangerZ;
    int numPrefab;
    string parentName;
    float scalebuffer;
    bool isfull;

    Collider subsidiaryCollider;
    Collider parentCollider;

    // Start is called before the first frame update
    void Start()
    {
        parentCell = GameObject.Find(parentName);
        subsidiaryCell.transform.position = parentCell.transform.position;
        subsidiaryCell.transform.localScale = parentCell.transform.localScale;
        positionChangerX = 0.01f;
        positionChangerZ = 0.01f;

        if(parentCell.tag == "bacteria" || parentCell.tag == "bacteriaDetailed" || parentCell.tag == "preEukaryote" || parentCell.tag == "preEukaryoteDetailed" || parentCell.tag == "eukaryote" || parentCell.tag == "eukaryoteDetailed")
        {
            parentCollider = parentCell.GetComponent<CapsuleCollider>();
            subsidiaryCollider = subsidiaryCell.GetComponent<CapsuleCollider>();
        }
            
        else if(parentCell.tag == "cianobacteria" || parentCell.tag == "cianobacteriaDetailed")
        {
            parentCollider = parentCell.GetComponent<SphereCollider>();
            subsidiaryCollider = subsidiaryCell.GetComponent<SphereCollider>();
        }
            
        subsidiaryCollider.enabled = false;
        parentCollider.enabled = false;

        positionChangerX = Random.Range(0, 2) == 0 ? positionChangerX : -positionChangerX;
        positionChangerZ = Random.Range(0, 2) == 0 ? positionChangerZ : -positionChangerZ;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        parentCell = GameObject.Find(parentName);

        if (parentCell.name == parentName)
        {
            subsidiaryCell.transform.position = new Vector3(parentCell.transform.position.x + positionChangerX, 0, parentCell.transform.position.z + positionChangerZ);
            subsidiaryCell.transform.rotation = parentCell.transform.rotation;
            positionChangerX = positionChangerX < 0 ? positionChangerX -= 0.01f : positionChangerX += 0.01f;
            positionChangerZ = positionChangerZ < 0 ? positionChangerZ -= 0.01f : positionChangerZ += 0.01f;

        }
        else
            print("You do some mistake");


        scalebuffer += 0.01f;
        if (scalebuffer >= 1 && scalebuffer < 1.01)
        {
            if (parentCell.tag == "preEukaryote" || parentCell.tag == "preEukaryoteDetailed")
                parentCell.GetComponent<PreEukaryote>().ScaleChanger(-0.6f);
            else if (numPrefab == 0)
                parentCell.GetComponent<Bacteria>().ScaleChanger(-0.6f);
            else if (numPrefab == 1)
                parentCell.GetComponent<CianoBacteria>().ScaleChanger(-0.3f);
            else
                parentCell.GetComponent<Eukaryote>().ScaleChanger(-0.6f);

            subsidiaryCell.transform.localScale = parentCell.transform.localScale;


            scalebuffer = 0;
        }
        if (Mathf.Abs(positionChangerX) >= 5 || Mathf.Abs(positionChangerZ) >= 5)
        {
            Born();
        }
    }

    void Born()
    {
        if (subsidiaryCell.tag != "preEukaryote" && subsidiaryCell.tag != "preEukaryoteDetailed")
        {
            if (numPrefab == 0)
            {
                subsidiaryCell.AddComponent<Bacteria>().SetParams(subsidiaryCell, subsidiaryCell.tag, "bacteria_" + PlayerPrefs.GetInt("CountBacteria"), 100, 5f, false, true, Quaternion.identity, 0);
                PlayerPrefs.SetInt("CountBacteria", PlayerPrefs.GetInt("CountBacteria") + 1);
                subsidiaryCollider = subsidiaryCell.GetComponent<CapsuleCollider>();
                parentCollider = parentCell.GetComponent<CapsuleCollider>();
                subsidiaryCollider.enabled = true;
                parentCollider.enabled = true;
                parentCell.transform.localScale = new Vector3(5f, 5f, 5f);
                parentCell.GetComponent<Bacteria>().ChangeVariablehasDaughter();
                enabled = false;
            }
            else if (numPrefab == 1)
            {
                subsidiaryCell.AddComponent<CianoBacteria>().SetParams(subsidiaryCell, subsidiaryCell.tag, "cianobacteria_" + PlayerPrefs.GetInt("CountCianobacteria"), 80, 3f, false, true, Quaternion.identity, 0);
                PlayerPrefs.SetInt("CountCianobacteria", PlayerPrefs.GetInt("CountCianobacteria") + 1);
                subsidiaryCollider = subsidiaryCell.GetComponent<SphereCollider>();
                parentCollider = parentCell.GetComponent<SphereCollider>();
                subsidiaryCollider.enabled = true;
                parentCollider.enabled = true;
                parentCell.transform.localScale = new Vector3(3f, 3f, 3f);
                parentCell.GetComponent<CianoBacteria>().ChangeVariablehasDaughter();
                enabled = false;
            }
            else
            {
                subsidiaryCell.AddComponent<Eukaryote>().SetParams(subsidiaryCell, subsidiaryCell.tag, "eukaryote_" + PlayerPrefs.GetInt("CountEukaryote"), 120, 10f, false, true, Quaternion.identity, numPrefab, 0);
                PlayerPrefs.SetInt("CountEukaryote", PlayerPrefs.GetInt("CountEukaryote") + 1);
                subsidiaryCollider = subsidiaryCell.GetComponent<CapsuleCollider>();
                parentCollider = parentCell.GetComponent<CapsuleCollider>();
                subsidiaryCollider.enabled = true;
                parentCollider.enabled = true;
                if (parentCell.tag == "eukaryote" || parentCell.tag == "eukaryoteDetailed")
                {
                    parentCell.transform.localScale = new Vector3(10f, 10f, 10f);
                    parentCell.GetComponent<Eukaryote>().ChangeVariablehasDaughter();
                    enabled = false;
                }
                else
                {
                    parentCell.transform.localScale = new Vector3(10f, 10f, 10f);
                    parentCell.GetComponent<PreEukaryote>().ChangeVariablehasDaughter();
                    enabled = false;
                }
            }
        }
        else
        {
            subsidiaryCell.AddComponent<PreEukaryote>().SetParams(subsidiaryCell, subsidiaryCell.tag, "eukaryote_" + PlayerPrefs.GetInt("CountEukaryote"), 120, 5f, false, true, Quaternion.identity, numPrefab, 0);
            PlayerPrefs.SetInt("CountEukaryote", PlayerPrefs.GetInt("CountEukaryote") + 1);
            subsidiaryCollider = subsidiaryCell.GetComponent<CapsuleCollider>();
            parentCollider = parentCell.GetComponent<CapsuleCollider>();
            subsidiaryCollider.enabled = true;
            parentCollider.enabled = true;
            parentCell.transform.localScale = new Vector3(5f, 5f, 5f);
            parentCell.GetComponent<PreEukaryote>().ChangeVariablehasDaughter();
            enabled = false;

        }
        
    }

    public void SetParams(string parentName, GameObject subsidiary, int numprefab, float scalebuffer, bool full)
    {
        this.parentName = parentName;
        subsidiaryCell = subsidiary;
        numPrefab = numprefab;
        this.scalebuffer = scalebuffer;
        isfull = full;
    }
    public void SetParams(string parentName, GameObject subsidiary, int numprefab, float scalebuffer)
    {
        this.parentName = parentName;
        subsidiaryCell = subsidiary;
        numPrefab = numprefab;
        this.scalebuffer = scalebuffer;
    }
}