using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoWayPlatform : Platform, LightSpotBehavior {
    public Transform dest;
    private Vector3 origin;
    float progress;
    // Use this for initialization
    void Start()
    {
        origin = transform.position;
    }

    public void respondToProgress(LSBInput input)
    {
        float oldProgress = progress;
        progress += input.getInput1D();
        progress = Mathf.Max(progress, 0);
        progress = Mathf.Min(progress, 1);
        Vector3 newPosi = Vector3.Lerp(origin, dest.position, progress);
        if (!obstacleAhead(newPosi - transform.position))
        {
            if (inTrigger)
            {
                player.transform.position += newPosi - transform.position;
            }
            transform.parent.position += (newPosi - transform.position);
        } else
        {
            progress = oldProgress;
        }
        
    }

    private bool obstacleAhead(Vector3 direction)
    {
         RaycastHit hit;
        //boundDistance = (Target.transform.position - Camera.main.transform.position).magnitude;
        if (Physics.Raycast(transform.position, direction, out hit, 1f + 0.3f))
            {
            return true;
        }
        return false;
    }
}
