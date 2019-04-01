using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        print("enter");
        if (other.CompareTag("Player"))
        {
            Respawn.onPlayerReachCheckPoint(this);
        }
    }
}
