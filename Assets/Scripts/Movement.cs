using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    protected float Speed;

    float rangeX;
    float rangeZ;
    public Quaternion targetRot;

    void Start()
    {
        rangeX = 1215;
        rangeZ = 570;
    }

    void Update()
    {
        if (rangeX - Mathf.Abs(transform.position.x) < 0 || rangeZ - Mathf.Abs(transform.position.z) < 0)
            transform.rotation = Quaternion.Euler(0f, transform.rotation.y * -1, 0f); 

        transform.position += -transform.right * Speed * Time.deltaTime;

        if (Random.Range(0, 1000) == 0)
        {
            targetRot = Quaternion.Euler(0f, Random.Range(-200, 200), 0f);
        }

        ExecuteRotation(targetRot);
        
    }

    public void ExecuteRotation(Quaternion targetRotation)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.3f * Time.deltaTime);
    }

    public void setParams(float speed)
    {
        Speed = speed;
    }
}
