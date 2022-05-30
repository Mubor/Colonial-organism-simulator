using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{
    bool canFollow;
    GameObject objectToFollow;
    float speed = 0.1f;
    float xR = 1215.0f;
    float zR = 570.0f;
    string Name; // переменная для сохранения имени обьекта слежения 
    float prevHeight; //предыдущая позиция камеры

    public Text currentHP;

    void Start()
    {
        PlayerPrefs.SetInt("NeedDoDetailingDead", 0);
        PlayerPrefs.SetInt("NeedDoSimplificationDead", 0);
        PlayerPrefs.SetInt("NeedDoDetailingBac", 0);
        PlayerPrefs.SetInt("NeedDoSimplificationBac", 0);
        PlayerPrefs.SetInt("NeedDoDetailingCiano", 0);
        PlayerPrefs.SetInt("NeedDoSimplificationCiano", 0);
        PlayerPrefs.SetInt("NeedDoDetailingVir", 0);
        PlayerPrefs.SetInt("NeedDoSimplificationVir", 0);
        PlayerPrefs.SetInt("NeedDoDetailingInf", 0);
        PlayerPrefs.SetInt("NeedDoSimplificationInf", 0);
        PlayerPrefs.SetInt("NeedDoDetailingEu", 0);
        PlayerPrefs.SetInt("NeedDoSimplificationEu", 0);
        PlayerPrefs.SetInt("CountBacteria", 0);
        PlayerPrefs.SetInt("CountCianobacteria", 0);
        PlayerPrefs.SetInt("CountVirus", 0);
        PlayerPrefs.SetInt("CountEukaryote", 0);

        //PlayerPrefs.SetInt("CountSpawn", 100);
        //PlayerPrefs.SetInt("SpeedBacteria", 10);
        //PlayerPrefs.SetInt("SpeedCiano", 10);
        //PlayerPrefs.SetInt("SpeedVirus", 10);
        //PlayerPrefs.SetInt("SpeedEu", 10);

        canFollow = false;
        prevHeight = transform.position.y;
    }

    void FixedUpdate()
    {
        float cameraHeight = transform.position.y;
        float speedMoveCoef;
        Vector3 moving = new Vector3(0, 0, 0);

        //Сопутсвующие переменные
        speedMoveCoef = 1 + (cameraHeight / 100);
        var frustumHeight = 2.0f * transform.position.y * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        var frustumWidth = 2.0f * transform.position.y * Mathf.Tan(Camera.main.fieldOfView * 0.25f * Mathf.Deg2Rad);


        if (!canFollow)
        {
            if (Input.GetKey(KeyCode.A) && transform.position.x > -xR + frustumHeight)
                transform.position = new Vector3(transform.position.x - speedMoveCoef, transform.position.y, transform.position.z);
            if (Input.GetKey(KeyCode.D) && transform.position.x < xR - frustumHeight)
                transform.position = new Vector3(transform.position.x + speedMoveCoef, transform.position.y, transform.position.z);
            if (Input.GetKey(KeyCode.W) && transform.position.z < zR - frustumWidth)
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speedMoveCoef / (xR / zR));
            if (Input.GetKey(KeyCode.S) && transform.position.z > -zR + frustumWidth)
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - speedMoveCoef / (xR / zR));
        }


        //Стрелка вверх   Стрелка вниз - управление камеры вертикально
        if (Input.GetKey(KeyCode.UpArrow) && cameraHeight > 5.0f)
        {
            prevHeight = transform.position.y;
            transform.position = new Vector3(transform.position.x, transform.position.y - 5.0f, transform.position.z);

        }

        if (Input.GetKey(KeyCode.DownArrow) && cameraHeight < 1000.0f)
        {
            prevHeight = transform.position.y;

            //движение камеры в с орону центра при отдалении
            if (((transform.position.x > 10 || transform.position.x < -10) || (transform.position.z > 10 || transform.position.z < -10)) && !canFollow)
            {
                if ((transform.position.x > 10 || transform.position.x < -10) && (transform.position.z < 10 || transform.position.z > -10))
                    moving = new Vector3(((transform.position.x < 0 ? 1 : -1) * frustumHeight), 0, 0);
                else if ((transform.position.x < 10 || transform.position.x > -10) && (transform.position.z > 10 || transform.position.z < -10))
                    moving = new Vector3(0, 0, ((transform.position.z < 0 ? 1 : -1) * frustumWidth));
                else
                    moving = new Vector3(((transform.position.x < 0 ? 1 : -1) * frustumHeight), 0, ((transform.position.z < 0 ? 1 : -1) * frustumWidth));

                transform.position += moving.normalized * 400.0f * Time.deltaTime;
            }

            //движение камеры вверх
            transform.position = new Vector3(transform.position.x, transform.position.y + 5.0f, transform.position.z);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            canFollow = false;
            Name = null;
            objectToFollow = null;
        }
        // Слежение камеры за обьектом
        if (canFollow && objectToFollow != null)
        {
            //print("I am following " + objectToFollow.name);
            Vector3 pos = new Vector3(objectToFollow.transform.position.x, transform.position.y, objectToFollow.transform.position.z);
            transform.position = Vector3.Lerp(transform.position, pos, speed);

            switch (objectToFollow.tag)
            {
                case "bacteria":
                    currentHP.text = "Current Energy: " + objectToFollow.GetComponent<Bacteria>().HP.ToString("F1");
                    break;

                case "bacteriaDetailed":
                    currentHP.text = "Current Energy: " + objectToFollow.GetComponent<Bacteria>().HP.ToString("F1");
                    break;

                case "cianobacteria":
                    currentHP.text = "Current Energy: " + objectToFollow.GetComponent<CianoBacteria>().HP.ToString("F1");
                    break;

                case "cianobacteriaDetailed":
                    currentHP.text = "Current Energy: " + objectToFollow.GetComponent<CianoBacteria>().HP.ToString("F1");
                    break;

                case "virus":
                    currentHP.text = "Current Energy: " + objectToFollow.GetComponent<Virus>().HP.ToString("F1");
                    break;

                case "virusDetailed":
                    currentHP.text = "Current Energy: " + objectToFollow.GetComponent<Virus>().HP.ToString("F1");
                    break;

                case "preEukaryote":
                    currentHP.text = "Current Energy: " + objectToFollow.GetComponent<PreEukaryote>().HP.ToString("F1");
                    break;

                case "preEukaryoteDetailed":
                    currentHP.text = "Current Energy: " + objectToFollow.GetComponent<PreEukaryote>().HP.ToString("F1");
                    break;

                case "eukaryote":
                    currentHP.text = "Current Energy: " + objectToFollow.GetComponent<Eukaryote>().HP.ToString("F1");
                    break;

                case "eukaryoteDetailed":
                    currentHP.text = "Current Energy: " + objectToFollow.GetComponent<Eukaryote>().HP.ToString("F1");
                    break;

                default:
                    currentHP.text = "Current Energy: -";
                    break;
            }


        }
        else
            currentHP.text = "Current Energy: -";
        if (Input.GetKeyDown(KeyCode.E))
        {
            canFollow = false;
            Name = null;
            objectToFollow = null;
        }

    }

    void Update()
    {
        //if (Name != null)
        //    objectToFollow = GameObject.Find(Name);

        // обработка нажатия клавиши для слежения
        if (Time.timeScale != 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                {
                    if (hit.transform.name != "GenerationPlace")
                    {
                        Name = hit.transform.name;
                        objectToFollow = GameObject.Find(Name);
                        canFollow = true;
                    }
                }
            }
        }
    }
}