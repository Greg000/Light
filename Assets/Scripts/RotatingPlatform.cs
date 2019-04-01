using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : Platform, LightSpotBehavior {
    public float angularSpeed;
    public Transform pivot;

    private float radius;

    public void respondToProgress(LSBInput input)
    {
        float deltaAngle = input.getInput1D() * 360;
        transform.Rotate(new Vector3(0, deltaAngle, 0));
        print(transform.rotation.eulerAngles);
    }

    private void Awake()
    {
        //radius = (pivot.position - transform.position).magnitude;
    }




}
