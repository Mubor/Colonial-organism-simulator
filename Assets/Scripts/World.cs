using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public int tick;
    public int tickBuff;
    protected bool timesOfDay { get; set; }//true-day, false - night
    protected float otherGazes { get; set; } //Other Gazes: --
    protected float oxygen; //Oxygen: --

    protected GameObject ciano;

    // Start is called before the first frame update
    void Awake()
    {
        tick = 0;
        timesOfDay = true;
        otherGazes = 100000f;
        oxygen = 1000f;
        tickBuff = 0;
    }

    public bool GetTime()
    {
        return timesOfDay;
    }

    public float GetOxygen()
    {
        return oxygen;
    }

    public void OtherGazesUp(float upInd)
    {
        otherGazes += upInd;
    }

    public float GetGazePercent(float Gaze)
    {
        return (Gaze * 100) / (otherGazes + oxygen);
    }

    public void OxygenUp(float upInd)
    {
        if(timesOfDay)
            oxygen += upInd;
    }

    public void ChangeCianoHpChanger()
    {
        int n = PlayerPrefs.GetInt("CountCianobacteria");
        for (int i = 0; i < n; i++)
        {

            ciano = GameObject.Find("cianobacteria_" + i);
            if (ciano != null && ciano.name == "cianobacteria_" + i && (ciano.tag == "cianobacteria" || ciano.tag == "cianobacteriaDetailed"))
                ciano.GetComponent<CianoBacteria>().HPChanger(timesOfDay);


        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (tick >= 7500)
        {
            timesOfDay = !timesOfDay;
            tick = 0;
            ChangeCianoHpChanger();
        }

        tick++;
    }
}