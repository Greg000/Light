using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour {
    private void OnTriggerEnter(Collider other)
    {
        transform.parent.GetComponent<Platform>().OnPlatformTriggerEnter(other);   
    }

    private void OnTriggerExit(Collider other)
    {
        transform.parent.GetComponent<Platform>().OnPlatformTriggerExit(other);
    }
}
