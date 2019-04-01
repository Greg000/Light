using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularProgressBar : MonoBehaviour {
    public void SetProgress(float percent) {
        Image whiteCircle = GetComponent<Image>();
        if (percent < 0) {
            whiteCircle.fillClockwise = false;
            whiteCircle.color = Color.red;
            percent = -percent;
        } else {
            whiteCircle.fillClockwise = true;
            whiteCircle.color = Color.white;
        }
        whiteCircle.fillAmount = percent;
    }
}
