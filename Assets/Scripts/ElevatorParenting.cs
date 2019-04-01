using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorParenting : MonoBehaviour {
    public GameObject character;
    private void OnTriggerEnter(Collider other)
    {
        character.transform.SetParent(transform.parent);
    }

    private void OnTriggerExit(Collider other)
    {
        character.transform.SetParent(null);
    }
}
