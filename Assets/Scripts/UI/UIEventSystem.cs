using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEventSystem : MonoBehaviour {
    public static UIEventSystem instance;
    public static GameObject hGroup;

    private static int minSiblingIndex;
    private void Start()
    {
        hGroup = GameObject.Find("hGroup");
        instance = this;
        minSiblingIndex = 10;
    }
    public static void createTextHint(string content, float duration)
    {
        GameObject go = (GameObject)Instantiate(Resources.Load("Prefabs/UI/HintText"));
        go.GetComponentInChildren<Text>().text = content;
        if (hGroup == null)
        {
            hGroup = GameObject.Find("hGroup");
        }
        go.transform.SetParent(hGroup.transform);

        if (duration > 0)
        {
            instance.StartCoroutine(DeleteAfterSeconds(go, duration));
        }
        
    }

    public static void clear()
    {
        foreach(Transform child in hGroup.transform)
        {
            minSiblingIndex = 10;
            Destroy(child.gameObject);
        }
    }

    private static IEnumerator DeleteAfterSeconds(GameObject go, float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(go);
        yield return null;
    }
}
