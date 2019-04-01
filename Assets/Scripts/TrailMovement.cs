using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class TrailMovement : Platform {
    public Transform dest;
    public float maxVelocity;
    public float acceleration;

    private Vector3 origin;
    private bool started;
    private bool forward;
    private Vector3 velocity;
    private Vector3 previousPosition;
    private Vector3 offset;
    private List<GameObject> objectsOnPlatform;
    private bool isPlayerOnPlatform;
    private void Awake()
    {
        origin = transform.position;
        started = false;
        forward = true;
        velocity = (dest.position - origin).normalized;
        objectsOnPlatform = new List<GameObject>();
    }

    public void startAction()
    {
        started = true;
        StartCoroutine("move");

    }

    public void pauseAction()
    {
        started = false;
        //StopCoroutine("move");
    }

    public void continueAction()
    {
        StartCoroutine("move");
    }

    public void restartAction()
    {

    }

    public void FixedUpdate()
    {
       
    }

    IEnumerator move()
    {
        yield return new WaitForSeconds(1);
        print(velocity);    
        while (started)
        {
            if (forward)
            {
                if ((transform.position - dest.position).magnitude < 0.1)
                {
                    forward = false;
                    velocity = -velocity;

                    yield return new WaitForSeconds(1);
                } else
                {
                    transform.Translate(velocity * Time.deltaTime, Space.World);
                    foreach (GameObject obj in objectsOnPlatform)
                        obj.transform.position += velocity * Time.fixedDeltaTime;
                    yield return new WaitForEndOfFrame();
                }
            }
            else {
                if ((transform.position - origin).magnitude < 0.1)
                {
                    forward = true;
                    velocity = -velocity;
                    yield return new WaitForSeconds(1);
                }
                else
                {
                    transform.Translate(velocity * Time.deltaTime, Space.World);
                    foreach (GameObject obj in objectsOnPlatform)
                        obj.transform.position += velocity * Time.fixedDeltaTime;
                    yield return new WaitForEndOfFrame();
                }
            }
            yield return null;
        }
        yield return null;
    }

    public void OnPlatformTriggerEnter(Collider other)
    {
        print(other.gameObject);
        objectsOnPlatform.Add(other.gameObject);
    }

    public void OnPlatformTriggerExit(Collider other)
    {
        objectsOnPlatform.Remove(other.gameObject);
    }

}
