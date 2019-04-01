using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour, LightSpotBehavior
{
    public Transform dest;
    private Vector3 origin;
    private float progress;

    private void Start()
    {
        origin = transform.position;
    }
    public void respondToProgress(LSBInput input)
    {
        Vector3 v = (dest.position - origin) * input.getInput1D();
        progress += input.getInput1D();
        progress = Mathf.Max(progress, 0);
        progress = Mathf.Min(progress, 1);
        Vector3 newPosi = Vector3.Lerp(origin, dest.position, progress);
        transform.parent.position += (newPosi-transform.position);
    }
}
