using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidance : SteeringBehavior
{
    public Kinematic character;
    public float maxAcceleration = 10f;
    public float avoidRadius = 2f;

    public override SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();

        Collider[] colliders = Physics.OverlapSphere(character.transform.position, avoidRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != character.gameObject)
            {
                Vector3 avoidanceDirection = character.transform.position - collider.transform.position;
                float distance = avoidanceDirection.magnitude;

                float strength = Mathf.Min(avoidRadius / distance, maxAcceleration);
                avoidanceDirection.Normalize();
                result.linear += strength * avoidanceDirection;
            }
        }

        return result;
    }
}
