using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObjects : MonoBehaviour {
    public GameObject lightObject;
    private bool ready;

    private void FixedUpdate()
    {
        if (lightObject != null && ready == true)
        {
          
            Vector3 origin = transform.position;
            Vector3 target = origin + Camera.main.transform.forward * 3;
            lightObject.GetComponent<Rigidbody>().MovePosition(target);
            lightObject.GetComponent<Rigidbody>().MoveRotation(transform.rotation);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (lightObject == null)
            {
                lightObject = pickUp();
            } else
            {
                lightObject.GetComponent<Rigidbody>().useGravity = true;
                lightObject.GetComponent<LightObject>().onLightExit();
                lightObject = null;
                ready = false;

            }
        }
    }

    public GameObject pickUp()
    {
        RaycastHit hit;
        Vector3 origin = Camera.main.transform.position;
        Vector3 castDir = Camera.main.transform.forward;

        if (Physics.Raycast(origin, castDir, out hit, 3)) {
            GameObject target = hit.collider.gameObject;
            if (target.CompareTag("LightObject"))
            {
                target.GetComponent<Rigidbody>().useGravity = false;
                StartCoroutine(pullObject(0.2f, target));
                Physics.IgnoreCollision(GetComponent<Collider>(), target.GetComponent<Collider>());
                target.GetComponent<LightObject>().onLightEnter();
                return target;
            }
        }
        return null;
    }

    IEnumerator pullObject(float duration, GameObject targetObject)
    {
        Vector3 origin = targetObject.transform.position;
        Vector3 dest = transform.position + Camera.main.transform.forward * 3;
        Quaternion start = targetObject.transform.rotation;
        Quaternion end = transform.rotation;
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            print(targetObject);
            targetObject.transform.position = Vector3.Lerp(origin, dest, time / duration);
            targetObject.transform.rotation = Quaternion.Lerp(start, end, time / duration);
            yield return new WaitForEndOfFrame();
        }
        ready = true;
    }
}
