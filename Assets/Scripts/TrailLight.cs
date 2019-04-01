using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailLight : MonoBehaviour, LightSpotBehavior {
    private Material material;
    public Color color;
    private Color oldColor;
    private bool actionStarted;
    private void Awake()
    {
        print("???");
        Transform frame = TransformDeepChildExtension.FindDeepChild(transform, "frame");
        print(frame.gameObject);
        Renderer rend = frame.gameObject.GetComponent<Renderer>();
        material = new Material(Shader.Find("Standard"));
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", Color.black);
        rend.material = material;
        oldColor = new Color(0, 0, 0);
    }

    public void respondToProgress(LSBInput input)
    {
        float deltaProgress = input.getInput1D();
        if (deltaProgress != 0) {
            if (!actionStarted)
            {
                StopAllCoroutines();
                StartCoroutine(ChangeIntensity(color, 2.7f, 0.5f));
                actionStarted = true;
            }
            
        } else
        {
            StopAllCoroutines();
            StartCoroutine(ChangeIntensity(Color.black, 0, 1));
            actionStarted = false;
        }
        
    }

    public void startAction()
    {
        StartCoroutine(ChangeIntensity(color, 2.7f, 0.5f));
    }

    public void pauseAction()
    {
        StartCoroutine(ChangeIntensity(Color.black, 0, 0.5f));
    }

    public void continueAction()
    {

    }

    public void restartAction()
    {

    }


    private IEnumerator ChangeIntensity(Color color, float lightIntensity, float duration)
    {
        float time = 0;
        Color temp = new Color(0,0,0);
        float oldIntensity = GetComponentInChildren<Light>().intensity;
        while (time < duration)
        {
            time += Time.deltaTime;
            temp = Color.Lerp(oldColor, color, time / duration);
            material.SetColor("_EmissionColor", temp);
            GetComponentInChildren<Light>().intensity = oldIntensity + (lightIntensity - oldIntensity) * (time / duration);
            yield return new WaitForEndOfFrame();
        }
        oldColor = temp;
    }
}
