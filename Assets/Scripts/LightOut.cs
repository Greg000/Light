using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOut : MonoBehaviour {
    public Light light;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(lightOut(0.5f));
        }
    }

    IEnumerator lightOut(float duration)
    {
        float time = 0;
        float originalIntensity = light.intensity;
        while (time < duration)
        {
            time += Time.deltaTime;
            light.intensity = originalIntensity * (1 - time / duration);
            yield return new WaitForEndOfFrame();
        }
    }
}
