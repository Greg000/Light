using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification : MonoBehaviour{
    public string content;
    public float duration;
    public void FireNotification()
    {
        UIEventSystem.createTextHint(content, duration);
    }
}
