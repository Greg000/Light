using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourWayDirection : MonoBehaviour {
    public GameObject platform;
    public Vector3 direction;

    public void startAction() {
        platform.GetComponent<FourWayPlatform>().setVelocity(direction);
    }
    public void pauseAction() {
        platform.GetComponent<FourWayPlatform>().setVelocity(new Vector3(0,0,0));
    }
    public void continueAction() { }
    public void restartAction() { }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
