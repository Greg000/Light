using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Dots3 : Shape {
    public Vector3 end1;
    public Vector3 end2;
    public Vector2 directionOnScreen;
    public float secondsPerRound;
    public GameObject pair;

    //progress ranges from 0 - 1
    private float progress;
    private bool check;
    /*
    public Line(Vector3 end1, Vector3 end2)
    {
        this.end1 = end1;
        this.end2 = end2;
    }*/
    override public Vector3 gameInputOnShape()
    {
        if (pair != null && GetComponentInParent<LightSpotArea>().isMerged() == true)
        {
            pair.GetComponent<Shape>().gameInputOnShape();
        }
        check = false;
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
            }
            else
            {
                deltaProgress = Mathf.Max(deltaProgress, -1 - progress);
                magnitude = (-1 - progress);
            }
        }
        progress += deltaProgress;
        if (progress > 0.5) {
            return transform.position + (end1 - end2) / 2;
        } else if (progress < -0.5)
        {
            return transform.position - (end1 - end2) / 2;
        } else
        {
            return transform.position;
        }
    }

    override public Vector3 getRestorePosition()
    {
    
        if (pair != null && GetComponentInParent<LightSpotArea>().isMerged() == true)
        {
            pair.GetComponent<Shape>().getRestorePosition();
        }
        if (!check)
        {
            check = true;
            progress = Mathf.Round(progress);
        }
        float newProgress = 0;
        if (progress > 0)
        {
            newProgress = Mathf.Max(progress - Time.deltaTime / secondsPerRound, 0);
        }
        else if (progress < 0)
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
