using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetLightTrigger : MonoBehaviour {
    private bool inTrigger;
    private GameObject player;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;
            //boundDistance = (Target.transform.position - Camera.main.transform.position).magnitude;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
            {
                print(hit.collider.name + inTrigger);
                if (hit.collider.name == "Light" && inTrigger)
                {
                    UIEventSystem.createTextHint("You now have a Light.", 10);
                    UIEventSystem.createTextHint("You can hold down left mouse button to increase the range of light.", 10);
                    hit.collider.gameObject.GetComponent<LightBehavior>().enabled = true;
                    hit.collider.gameObject.GetComponent<BoxCollider>().enabled = false;
                    hit.collider.gameObject.GetComponent<Light>().range = 10;
                }
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = true;
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = false;
        }
    }
}
