using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorBehavior : MonoBehaviour{
    public Vector3 velocity;
    public GameObject player;
    private bool rising;
    private bool playerStanding;
    private Vector3 origin;
	// Use this for initialization
	void Start () {
        origin = transform.position;
	}
	public void startAction() 
    {
        rising = true;
        StartCoroutine(rise(5f));
    }
    public void pauseAction()
    {
        rising = false;
        StartCoroutine(fall());
        //GetComponent<Rigidbody>().isKinematic = false;
        //GetComponent<Rigidbody>().useGravity = true;
    }
    public void continueAction() { }
    public void restartAction() { }

    private IEnumerator rise(float duration)
    {
        float time = 0;
        while (time < duration && rising)
        {
            time += Time.deltaTime;
            GetComponent<Rigidbody>().MovePosition(transform.position + velocity * Time.deltaTime);
            //transform.Translate(velocity * Time.deltaTime, Space.World);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator fall()
    {
        Vector3 downVelocity = new Vector3(0, 0, 0);
        Vector3 gravity = Physics.gravity;
        while ((transform.position - origin).magnitude > 0.05)
        {
            downVelocity +=  gravity * Time.deltaTime;
            transform.Translate(downVelocity * Time.deltaTime, Space.World);
            yield return new WaitForEndOfFrame();
        }
    }
}
