using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour {
    public static GameObject character;

    private static Respawn instance;
    private static CheckPoint currentPoint;
    private void Start()
    {
        character = GameObject.Find("Character");
        instance = this;
    }

    public static void onPlayerReachCheckPoint(CheckPoint cp)
    {
        currentPoint = cp;
    }

    public static void respawnPlayer()
    {
        if (character == null)
        {
            character = GameObject.Find("Character");
        }
        instance.StartCoroutine(waitForSeconds(1));
        
    }

    static IEnumerator waitForSeconds(float duration)
    {
        yield return new WaitForSeconds(duration);
        character.transform.position = currentPoint.transform.position;
        character.transform.rotation = currentPoint.transform.rotation;
        Camera.main.transform.localRotation = new Quaternion(0, 0, 0, 1);
    }

}
