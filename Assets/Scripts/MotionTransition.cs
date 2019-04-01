using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionTransition : MonoBehaviour {
    public Transform dest;

    private Vector3 origin;
    private float progress;

    private void Start()
    {
        origin = transform.position;
    }

    public void onTargetMoved(float amount)
    {
        amount = Mathf.Abs(amount);
        progress += amount;
        float actual_progress;
        if (progress % 1 > 0.5)
        {
            actual_progress = 0.5f - (progress % 1 - 0.5f);
        } else
        {
            actual_progress = progress % 1;
        }
        GetComponentInChildren<TrailLight>().respondToProgress(new LSBInput(amount));
        transform.position = Vector3.Lerp(origin, dest.position, actual_progress * 2);
    }
}
