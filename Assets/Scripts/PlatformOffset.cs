using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformOffset : MonoBehaviour {
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject player = other.gameObject;
            Quaternion previousLocalRotation = player.transform.localRotation;
            print(previousLocalRotation.eulerAngles);
            player.transform.parent = transform;
            //player.transform.localRotation = previousLocalRotation;
            print(player.transform.localRotation.eulerAngles);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject player = other.gameObject;
            player.transform.parent = null;
        }
    }
}
