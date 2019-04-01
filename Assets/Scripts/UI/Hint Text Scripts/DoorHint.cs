using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHint : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIEventSystem.createTextHint("Stand close to the green light and click left mouse button",-1);
        }
    }
}
