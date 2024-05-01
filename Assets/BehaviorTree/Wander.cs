using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//chose to just have the wander move towards objects that are tagged 
public class Wander : SteeringBehavior
{
    public Kinematic character;
    float wanderRadius = 5f;
    float wanderRate = 50f; //the more i increase the more random
    float timeToChangeTarget = 5f;
    float timer;
    void Start()
    {
        SetNewWanderTarget();
    }

    // Update is called once per frame
    public override SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();

        timer += Time.deltaTime;
        if (timer >= timeToChangeTarget)
        {
            SetNewWanderTarget();
            timer = 0f;
        }
        result.linear = (character.myTarget.transform.position - character.transform.position).normalized;

        return result;
    }

    //setting a new wander target
    void SetNewWanderTarget()
    {
        GameObject[] wanderTargets = GameObject.FindGameObjectsWithTag("WanderTarget");

        if (wanderTargets.Length > 0)
        {
            character.myTarget = wanderTargets[Random.Range(0, wanderTargets.Length)];
        }
        else
        {
            character.myTarget = null;
        }
    }
}
