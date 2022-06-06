using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ColliderEvent : MonoBehaviour
{

    protected bool trigger;
    
    public void Params(bool IsTrigger)
    {
        trigger = IsTrigger;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (Time.timeScale != 0)
        {
            if (trigger && (gameObject.tag == "bacteria" || gameObject.tag == "cianobacteria") && collision.tag == "virus")// условие столкновение другой обьект - вирус
            {
                DoInfection(collision);   
            }
            if (trigger && gameObject.tag == "bacteria" && collision.tag != "virus")// условие столкновения бактерии с другим мертвым обьектом
            {
                if (collision.tag == "virusDead")
                    DoEating(collision, 10);
                else if (collision.tag == "cianobacteriaDead")
                    DoEating(collision, 50);
                else if (collision.tag == "bacteriaDead")
                    DoEating(collision, 100);
                else if (collision.tag == "cianobacteria")
                    DoEating(collision, 120);
            }
        }
    }
    private void DoEating(Collider collision, float hpChanger)
    {
        Destroy(collision.gameObject);

        gameObject.GetComponent<Bacteria>().ChangeHP(hpChanger);
    }

    private void DoInfection(Collider collision)
    {
       if(gameObject.tag == "cianobacteria")
           gameObject.GetComponent<CianoBacteria>().Infect();
       else if(gameObject.tag == "bacteria")
           gameObject.GetComponent<Bacteria>().Infect();
       
        Destroy(collision.gameObject);
    }
}