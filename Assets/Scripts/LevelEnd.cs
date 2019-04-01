using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour {
    public GameObject blackBG;

    private void Start()
    {
        blackBG.GetComponent<Image>().color = new Color(1, 1, 1, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(blackFadeIn(1));
            
        }
    }

    private IEnumerator blackFadeIn(float duration)
    {
        float time = 0;
        Color c = blackBG.GetComponent<Image>().color;
        while (duration > time)
        {
            time += Time.deltaTime;
            blackBG.GetComponent<Image>().color = Color.Lerp(c, Color.black, time / duration);
            yield return new WaitForEndOfFrame(); 
        }
        SceneManager.LoadScene("Level2");
    }
}
