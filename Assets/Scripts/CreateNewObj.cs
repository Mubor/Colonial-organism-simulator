using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNewObj : MonoBehaviour
{
    int rand_obj;

    float[,] newCoord;

    GameObject[] detailCells;


    private GameObject inst_obj;

    private void Awake()
    {
        detailCells = Resources.LoadAll<GameObject>("DetailedCells");
    }

    void Start()
    {
        newCoord = GenerateArray();
        //Создание префабов
        //for (int i = 0; i < PlayerPrefs.GetInt("CountSpawn"); i++)
        for (int i = 0; i < 0; i++)
        {
            rand_obj = Random.Range(0, 2);
            inst_obj = Instantiate(detailCells[rand_obj], new Vector3(newCoord[i, 0], 0, newCoord[i, 1]), Quaternion.identity) as GameObject;

            switch (rand_obj)
            {
                case 0:
                    {
                        inst_obj.AddComponent<Bacteria>().SetParams();
                    }
                    break;
                case 1:
                    {
                        inst_obj.AddComponent<CianoBacteria>().SetParams(inst_obj);
                    }
                    break;
                case 2:
                    {
                        inst_obj.AddComponent<Virus>().SetParams();
                    }
                    break;
                default:
                    print("Unknown thing");
                    break;
            }
        }
    }

    public void CreateSubsidiaryCell(GameObject parent, int numprefab)
    {
        GameObject subsidiary = Instantiate(detailCells[numprefab], parent.transform.position, parent.transform.rotation) as GameObject;

        switch (numprefab)
        {
            case 0:
                {
                    subsidiary.AddComponent<Bacteria>().SetParams();
                    break;
                }
            case 1:
                {
                    subsidiary.AddComponent<CianoBacteria>().SetParams(parent);
                    break;
                }
            case 2:
                {
                    subsidiary.AddComponent<Virus>().SetParams();
                    break;
                }
        }
    }

 
    float[,] GenerateArray()
    {
        float[,] Coordinates = new float[PlayerPrefs.GetInt("CountSpawn"), 2];

        for (int i = 0; i < PlayerPrefs.GetInt("CountSpawn"); i++)
        {
            Coordinates[i, 0] = Random.Range(-1213.0f, 1213.0f);
            Coordinates[i, 1] = Random.Range(-560.0f, 560.0f);

            if (!CheckDistanceForArray(i, Coordinates) && i != 0)
                i--;
        }

        return Coordinates;
    }

    bool CheckDistanceForArray(int currentIndex, float[,] arr)
    {
        for (int i = 0; i < currentIndex; i++)
        {
            if (CheckDistance(arr[i, 0], arr[i, 1], arr[currentIndex, 0], arr[currentIndex, 1]))
                continue;
            else
                return false;
        }

        return true;
    }

    bool CheckDistance(float xa, float za, float xb, float zb)
    {
        if (Mathf.Sqrt(Mathf.Pow(xb - xa, 2) + Mathf.Pow(zb - za, 2)) > 5)
            return true;
        return false;
    }
}