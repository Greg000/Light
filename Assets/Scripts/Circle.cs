using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Circle : Shape {
    public float radius;
    public float secondsPerRound;

    private Vector2 cursorPosition;
    private float progress;
    private float angle;

    private void Start()
    {
        cursorPosition = new Vector2(1, 0);
        angle = 0;
        progress = 0;
    }

    override public Vector3 initialLightPosition()
    {
        return transform.position + new Vector3(1, 0, 0) * radius;
    }
    override public Vector3 gameInputOnShape()
    {
        float xRot = CrossPlatformInputManager.GetAxis("Mouse X");
        float yRot = CrossPlatformInputManager.GetAxis("Mouse Y");
        cursorPosition += new Vector2(xRot, yRot);
        float tan = cursorPosition.y / cursorPosition.x;
        float newAngle = Mathf.Atan(tan);
        if (cursorPosition.x < 0)
        {
            newAngle = Mathf.PI + newAngle;
        }
        if (newAngle < 0)
        {
            newAngle = Mathf.PI * 2 + newAngle;
        }
        float minDiff = (newAngle - angle) % (Mathf.PI * 2);
        if (Mathf.Abs(minDiff) > Mathf.PI)
        {
            float mag = Mathf.PI * 2 - Mathf.Abs(minDiff);
            if (minDiff > 0)
            {
                minDiff = - mag;
            } else
            {
                minDiff = mag;
            }
        }
        newAngle = angle + minDiff;
        progress += (newAngle - angle) / (2 * Mathf.PI) / 5;
        angle = newAngle;
        print("angle" + angle);
        float x_coord = Mathf.Cos(progress * 2 * Mathf.PI);
        float y_coord = Mathf.Sin(progress * 2 * Mathf.PI);
        return transform.position +  new Vector3(x_coord, 0, y_coord).normalized * radius;
    }

    override public Vector3 getRestorePosition()
    {
        float newProgress = 0;
        if (progress > 0)
        {
            newProgress = Mathf.Max(progress - Time.deltaTime / secondsPerRound, 0);
        }
        else if (progress < 0)
        {
            newProgress = Mathf.Min(progress + Time.deltaTime / secondsPerRound, 0);
        }
        float delta = progress - newProgress;
        transform.parent.GetComponent<LightSpotArea>().onReceivedProgressUpdate(new LSBInput(delta));
        progress = newProgress;
        float x = Mathf.Cos(progress * 2 * Mathf.PI) * radius;
        float y = Mathf.Sin(progress * 2 * Mathf.PI) * radius;
        return transform.position + new Vector3(x, 0, y);
    }
    override public bool isRestored()
    {
        return progress == 0;
    }

}
