using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shape : MonoBehaviour {
    public Transform viewPoint;

    abstract public Vector3 gameInputOnShape();
    abstract public Vector3 getRestorePosition();
    abstract public bool isRestored();

    virtual public Vector3 initialLightPosition()
    {
        return transform.position;
    }
}
