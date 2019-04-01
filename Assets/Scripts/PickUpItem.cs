using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour {
    public Camera mainCamera;
    public GameObject swordInHand;
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.E))
        {
            
            int layerMask = 1 << 8;
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 10)) {
                print("?");
                if (Equals(hit.collider.name,"DarkSword")) {
                    swordInHand.SetActive(true);
                    hit.collider.gameObject.SetActive(false);
                }
            }
        }
            
	}
}
