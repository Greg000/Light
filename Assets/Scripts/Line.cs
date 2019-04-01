using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class Line : Shape {
    public Vector3 end1;
    public Vector3 end2;
    public Vector2 directionOnScreen;
    public float secondsPerRound;
    //progress ranges from 0 - 1
    private float progress;
    /*
    public Line(Vector3 end1, Vector3 end2)
    {
        this.end1 = end1;
        this.end2 = end2;
    }*/
    override public Vector3 gameInputOnShape()
    {
        float x = CrossPlatformInputManager.GetAxis("Mouse X");
        float y = CrossPlatformInputManager.GetAxis("Mouse Y");
        float proj = Vector2.Dot(new Vector2(x, y), directionOnScreen); 
        float magnitude = proj / 40;
        float deltaProgress = 0;
        deltaProgress = magnitude;
        if (Mathf.Abs(deltaProgress + progress) > 1)
        {
            if (magnitude > 0)
            {
                deltaProgress = Mathf.Min(deltaProgress, 1 - progress);
                magnitude = (1 - progress);
            } else
            {
                deltaProgress = Mathf.Max(deltaProgress, -1 - progress);
                magnitude = (-1 - progress);
            }
        }
        progress += deltaProgress;
        print(transform.position + progress * (end1 - end2) / 2);
        return transform.position + progress * (end1 - end2) / 2;

    }

    override public Vector3 getRestorePosition()
    {
        float newProgress = 0;
        if (progress > 0)
        {
            newProgress = Mathf.Max(progress - Time.deltaTime / secondsPerRound, 0);
        } else if (progress < 0)
        {
            newProgress = Mathf.Min(progress + Time.deltaTime / secondsPerRound, 0);
        }
        float delta = progress - newProgress;
        GetComponentInParent<LightSpotArea>().onReceivedProgressUpdate(new LSBInput(delta));
        progress = newProgress;
        print(progress);
        return transform.position + progress * (end1 - end2) / 2;

    }

    override public bool isRestored()
    {
        return progress == 0;
    }
}
