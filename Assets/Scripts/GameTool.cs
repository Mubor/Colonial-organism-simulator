using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTool : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject[] detailedPrefabs;
    GameObject thisObject;

    int toolMod;

    public Image Current;
    public Image CurrentBacteria;

    public Sprite[] sprites;

    void Start()
    {
        detailedPrefabs = Resources.LoadAll<GameObject>("DetailedCells");

        toolMod = -1;
    }

    // Update is called once per frame
    void CreateNewObject(Vector3 hit, int numprefab)
    {
       
        thisObject = Instantiate(detailedPrefabs[numprefab], hit, Quaternion.identity) as GameObject;
    
        switch (numprefab)
        {
            case 0:
                thisObject.AddComponent<Bacteria>().SetParams();
                break;
            case 1:
                thisObject.AddComponent<CianoBacteria>().SetParams(thisObject);
                break;
            case 2:
                thisObject.AddComponent<Virus>().SetParams();
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
                            CreateNewObject(hit.point, toolMod - 1);
                            PlayerPrefs.SetInt("CountBacteria", PlayerPrefs.GetInt("CountBacteria") + 1);
                        }
                        else if (toolMod == 2)
                        {
                            CreateNewObject(hit.point, toolMod - 1);
                            PlayerPrefs.SetInt("CountCianobacteria", PlayerPrefs.GetInt("CountCianobacteria") + 1);
                        }
                        else if (toolMod == 3)
                        {
                            CreateNewObject(hit.point, toolMod - 1);
                            PlayerPrefs.SetInt("CountVirus", PlayerPrefs.GetInt("CountVirus") + 1);
                        }
                    }
                }
            }
        }

    }

}