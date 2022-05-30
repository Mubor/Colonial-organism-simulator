using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTool : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject[] detailedPrefabs;
    GameObject[] simplePrefabs;
    GameObject thisObject;

    int toolMod;

    public Image Current;
    public Image CurrentBacteria;

    public Sprite[] sprites;

    void Start()
    {
        detailedPrefabs = Resources.LoadAll<GameObject>("DetailedCells");
        simplePrefabs = Resources.LoadAll<GameObject>("SimpleCells");

        toolMod = -1;
    }

    // Update is called once per frame
    void CreateNewObject(Vector3 hit, int numprefab, string tag, string name, int spd, int directionChange, float scale, float hp, int maxHeight)
    {
        if (Camera.main.transform.position.y >= maxHeight)
            thisObject = Instantiate(simplePrefabs[numprefab], hit, Quaternion.identity) as GameObject;
        else
        {
            thisObject = Instantiate(detailedPrefabs[numprefab], hit, Quaternion.identity) as GameObject;
            tag += "Detailed";
        }

        switch (numprefab)
        {
            case 0:
                thisObject.AddComponent<Bacteria>().SetParams(thisObject, tag, name, hp, scale, false, true, Quaternion.identity, 0);
                break;
            case 1:
                thisObject.AddComponent<CianoBacteria>().SetParams(thisObject, tag, name, hp, scale, false, true, Quaternion.identity, 0);
                break;
            case 2:
                thisObject.AddComponent<Virus>().SetParams(thisObject, tag, name, hp, true, Quaternion.identity);
                break;
        }
    }

    void Update()
    {
        if (Time.timeScale != 0)
        {
            // Установка модификатора действия
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                toolMod = 1;
                CurrentBacteria.enabled = true;
                CurrentBacteria.sprite = sprites[0];
                Current.enabled = false;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                toolMod = 2;
                Current.enabled = true;
                Current.sprite = sprites[1];
                CurrentBacteria.enabled = false;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                toolMod = 3;
                Current.enabled = true;
                Current.sprite = sprites[2];
                CurrentBacteria.enabled = false;
            }
            else if (Input.GetKeyDown(KeyCode.Delete))
            {
                toolMod = 0;
                Current.enabled = true;
                Current.sprite = sprites[3];
                CurrentBacteria.enabled = false;
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                toolMod = -1;
                Current.enabled = true;
                Current.sprite = sprites[4];
                CurrentBacteria.enabled = false;
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                {
                    if (toolMod == 0 && hit.transform.name != "GenerationPlace")
                    {
                        thisObject = GameObject.Find(hit.transform.name);
                        Destroy(thisObject);
                    }
                    if (hit.transform.name == "GenerationPlace")
                    {
                        //Заменить HP для бактерий - 100, для цианобактерий - 80

                        if (toolMod == 1)
                        {
                            CreateNewObject(hit.point, toolMod - 1, "bacteria", "bacteria_" + PlayerPrefs.GetInt("CountBacteria"), 10, 2000, 5.0f, 120, 500);
                            PlayerPrefs.SetInt("CountBacteria", PlayerPrefs.GetInt("CountBacteria") + 1);
                        }
                        else if (toolMod == 2)
                        {
                            CreateNewObject(hit.point, toolMod - 1, "cianobacteria", "cianobacteria_" + PlayerPrefs.GetInt("CountCianobacteria"), 10, 1000, 3.0f, 120, 350);
                            PlayerPrefs.SetInt("CountCianobacteria", PlayerPrefs.GetInt("CountCianobacteria") + 1);
                        }
                        else if (toolMod == 3)
                        {
                            CreateNewObject(hit.point, toolMod - 1, "virus", "virus_" + PlayerPrefs.GetInt("CountVirus"), 30, 1000, 1.0f, 100, 150);
                            PlayerPrefs.SetInt("CountVirus", PlayerPrefs.GetInt("CountVirus") + 1);
                        }
                    }
                }
            }
        }

    }

}