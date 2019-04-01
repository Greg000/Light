using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourWayPlatform : Platform { 
    private Vector3 velocity;
	// Use this for initialization
	void Start () {
        velocity = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        if (!obstacleAhead())
        {
            if (inTrigger)
            {
                player.transform.position += velocity * Time.deltaTime;
            }

            transform.position += velocity * Time.fixedDeltaTime;
        }
    }

    public void setVelocity(Vector3 value)
    {
        velocity = value;
    }

    private bool obstacleAhead()
    {
        if (velocity.magnitude != 0)
        {
            RaycastHit hit;
            //boundDistance = (Target.transform.position - Camera.main.transform.position).magnitude;
            if (Physics.Raycast(transform.position, velocity.normalized, out hit, 1f + 0.1f))
            {
                return true;
            }
        }
        return false;
    }
}
