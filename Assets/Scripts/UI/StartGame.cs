using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class StartGame : MonoBehaviour {
    public GameObject player;
    public GameObject cursor;

    private GameObject title;
    private GameObject button;
    private Material blurMaterial;
    
    public void Start()
    {
        title = transform.parent.Find("Title").gameObject;
        button = transform.parent.Find("Start").gameObject;
        blurMaterial = new Material(Shader.Find("Unlit/FrostedGlass"));
        blurMaterial.SetFloat("_Radius", 30f);
        transform.parent.Find("Blur").GetComponent<RawImage>().material = blurMaterial;
    }
    public void onButtonClick()
    {
        print("9");
        GameState.SetGameState(GameState.Game_State.IN_PROGRESS);
        player.GetComponent<FirstPersonController>().setMouseLock(true);
        StartCoroutine(fadeOut(0.5f));
        UIEventSystem.createTextHint("Use WASD and mouse to move around", 7);
    }

    IEnumerator fadeOut(float duration) 
    {
        float time = 0;
        while (duration > time)
        {
            Color c_title = title.GetComponent<Text>().color;
            Color c_button = button.GetComponent<Text>().color;
            c_title.a = 1 - (time / duration);
            c_button.a = 1 - (time / duration);
            title.GetComponent<Text>().color = c_title;
            button.GetComponent<Text>().color = c_button;
            time += Time.deltaTime;
            float radius = 30 * (1 - time / duration);
            blurMaterial.SetFloat("_Radius", radius);
            yield return new WaitForEndOfFrame();
        }
        title.SetActive(false);
        button.SetActive(false);
        cursor.SetActive(true);
    }
}
