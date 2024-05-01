using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : Align
{
    // TODO: override Align's getTargetAngle to face the target instead of matching its orientation
    public override float getTargetAngle()
    {
        //its facing the target by calculating the angle between the character position and the target position
        Vector3 direction = target.transform.position - character.transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        return targetAngle;
    }
}


