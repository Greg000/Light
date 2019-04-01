using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBox : MonoBehaviour, LightObject {

	public void onLightEnter()
    {
        Light light = GetComponent<Light>();
        light.enabled = true;
        light.intensity = 1;
        
    }

    public void onLightExit()
    {
        StartCoroutine("decreaseIntensity");
    }

    private IEnumerator decreaseIntensity()
    {
        while (GetComponent<Light>().intensity > 0)
        {
            GetComponent<Light>().intensity -= 0.00f * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        GetComponent<Light>().enabled = false;
    }
}
