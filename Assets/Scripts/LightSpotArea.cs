using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSpotArea : MonoBehaviour {
    public GameObject lightSpot;
    public GameObject light;

    private bool inLightSpotArea;
    private bool merged;
    private bool actionStarted;

    public void FireNotifications()
    {
        foreach (Notification n in GetComponents<Notification>())
        {
            n.FireNotification();
        }
    }

    public void onReceivedProgressUpdate(LSBInput input)
    {
        foreach (LightSpotBehavior lb in GetComponentsInChildren<LightSpotBehavior>())
        {
            lb.respondToProgress(input);
        }
    }

    public void setMerged(bool value)
    {
        merged = value;
    }

    public bool isInLightSpotArea()
    {
        return inLightSpotArea;
    }

    public bool isMerged()
    {
        return merged;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Equals(other.name, "Character"))
        {
            inLightSpotArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        inLightSpotArea = false;
    }

    private void Update()
    {

        /*if (merged && !actionStarted)
        {
            foreach (LightSpotBehavior lb in GetComponentsInChildren<LightSpotBehavior>())
            {
                actionStarted = true;
                lb.startAction();
            }
        } 
        
        if (!merged && actionStarted)
        {
            foreach (LightSpotBehavior lb in GetComponentsInChildren<LightSpotBehavior>())
            {
                actionStarted = false;
                lb.pauseAction();
            }
        }*/
    }


}
