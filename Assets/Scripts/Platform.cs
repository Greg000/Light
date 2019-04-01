using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// This is the class of all "platforms". A platform is a object where the player can stand, and
// the platform's movement will be added to the player as an offset, i.e. the player wil moving along
// the platform.

public class Platform : MonoBehaviour {
    protected bool inTrigger;
    protected GameObject player;

    public void OnPlatformTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            inTrigger = true;
        }
    }
    public void OnPlatformTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = false;
        }
    }
}
