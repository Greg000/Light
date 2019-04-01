using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideDoor : MonoBehaviour, LightSpotBehavior {
    public Transform dest;
    private Vector3 initialPosi;
    private float progress;

    private void Start()
    {
        initialPosi = transform.position;
        
    }

    public void respondToProgress(LSBInput input)
    {
        progress += input.getInput1D();
        progress = Mathf.Max(progress, 0);
        progress = Mathf.Min(progress, 1);
        transform.position = Vector3.Lerp(initialPosi, dest.position, progress);
    }
}
