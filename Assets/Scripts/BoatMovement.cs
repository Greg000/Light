using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour {
    public float maxVelocity;
    public float acceleration;

    public Transform dest; 

    private bool inTrigger;
    private Vector3 direction;
    private float velocity;
    private GameObject player;
	// Use this for initialization
	void Start () {
        velocity = 0;
        direction = (dest.position - transform.position).normalized;
	}
	
	// Update is called once per frame
	void Update () {
        velocity += Mathf.Min(acceleration * Time.deltaTime, maxVelocity);
        transform.position += velocity * direction * Time.deltaTime;
        if (inTrigger)
        {
            player.transform.position += velocity * direction * Time.deltaTime;
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
        if (other.CompareTag("Player")) {
            inTrigger = false;
        }
        
    }
}
