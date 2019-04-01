using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        GetComponent<Notification>().FireNotification();
    }

    private void OnTriggerExit(Collider other)
    {
        UIEventSystem.clear();
    }
}
