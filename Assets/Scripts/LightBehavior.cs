using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class LightBehavior : MonoBehaviour
{
    public float speed;
    public GameObject Target;
    public float initialRange;
    public float maxRange;
    public GameObject circularProgressBar;
    public GameObject character;
    [HideInInspector] public GameObject lightSpot;
    //[HideInInspector] public Vector3 lightSpotPosition;

    private enum State
    {
        IDLE,
        RISING,
        FALLING,
        RECOERING,
        MERGING,
        MERGED,
        HOLDING,
        DRAGGING,
        RESTORING,
        REPOSITIONING
    }

    private State currentState;
    private float minRange;
    private float boundDistance;
    private bool dropped;
    private Vector3 lightPosiPrev;
    private float mergingTime;
    private GameObject lightObject;
    private Vector3 camLocalPosiPrev;
    private Quaternion camLocalRotPrev;
    public bool isMerged()
    {
        return currentState == State.MERGED;
    }
    public void moveToLightSpot(Vector3 position)
    {
        transform.position = position;
    }
    // Use this for initialization
    private void Start()
    {
        minRange = initialRange - 5;
        currentState = State.IDLE;
        dropped = false;
    }

    private void FixedUpdate()
    {
        if (currentState == State.IDLE || currentState == State.FALLING || currentState == State.RISING || currentState == State.RECOERING)
        {
            UpdateLightPosition();
        }
    }

    private void Update()
    {
        if (GameState.GetGameState() != GameState.Game_State.IN_PROGRESS)
        {
            return;
        }
        Light light = GetComponent<Light>();
        ParticleSystem particle = GetComponent<ParticleSystem>();
        switch(currentState)
        {
            case State.IDLE:
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    lightSpot = getAvailableLightSpot();
                    if (lightSpot != null)
                    {
                        lightPosiPrev = transform.position;
                        camLocalPosiPrev = Camera.main.transform.localPosition;
                        camLocalRotPrev = Camera.main.transform.localRotation;
                        Camera.main.transform.parent = null;
                        character.gameObject.GetComponent<FirstPersonController>().setCameraSuspended(true);
                        lightSpot.GetComponentInParent<LightSpotArea>().FireNotifications();
                        currentState = State.MERGING;
                    } else
                    {
                        currentState = State.RISING;
                    }
                    
                } else if (Input.GetKeyDown(KeyCode.E))
                {
                    GameObject target = character.GetComponent<PickUpObjects>().pickUp();
                    if (target != null)
                    {
                        StartCoroutine(enterHoldingState(0.2f));
                        lightObject = target;
                        currentState = State.HOLDING;
                    }
                }
                break;
            case State.RISING:
                if (Input.GetKey(KeyCode.Mouse0) && light.range < maxRange)
                {
                    light.range = Mathf.Min(maxRange, light.range + (maxRange - initialRange) / 3 * Time.deltaTime);
                } else
                {
                    currentState = State.FALLING;
                }
                break;
            case State.FALLING:
                if (light.range != minRange)
                {
                    light.range = Mathf.Max(minRange, light.range - (maxRange - initialRange) * Time.deltaTime);
                }
                else
                {
                    currentState = State.RECOERING;
                }
                
                break;
            case State.RECOERING:
                if (light.range != initialRange)
                {
                    light.range = Mathf.Min(initialRange, light.range + (initialRange - minRange) / 3 * Time.deltaTime);
                }
                else
                {
                    currentState = State.IDLE;
                }
                break;
            case State.MERGING:
                transform.position = lightSpot.GetComponent<Shape>().initialLightPosition();
                mergingTime = Mathf.Min(mergingTime + Time.deltaTime, 1);
                Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, lightSpot.GetComponent<Shape>().viewPoint.rotation, mergingTime / 1);
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, lightSpot.GetComponent<Shape>().viewPoint.position, mergingTime / 1);
                transform.position = Vector3.Lerp(lightPosiPrev, lightSpot.transform.position, mergingTime / 1);
                if (mergingTime == 1)
                {
                    Camera.main.transform.SetParent(lightSpot.GetComponent<Shape>().viewPoint.transform);
                    currentState = State.MERGED;
                    lightSpot.GetComponentInParent<LightSpotArea>().setMerged(true);
                    foreach (ParticleSystem s in lightSpot.transform.parent.GetComponentsInChildren<ParticleSystem>())
                    {
                        if (s.CompareTag("LightIndicator"))
                        {
                            s.Play();
                        }
                    }
                }
                    
                break;
            case State.MERGED:
                //Cursor.visible = true;
                //character.gameObject.GetComponent<FirstPersonController>().setMouseLock(false);
                //Cursor.lockState = CursorLockMode.None;
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    currentState = State.DRAGGING;
                }
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    Camera.main.transform.parent = character.transform;
                    UIEventSystem.clear();
                    currentState = State.REPOSITIONING;
                    
                }
                transform.position = lightSpot.GetComponent<Shape>().initialLightPosition();
                break;
            case State.DRAGGING:
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    // update dragging distance
                    transform.position = lightSpot.GetComponent<Shape>().gameInputOnShape();
                } else
                {
                    // restore states
                    Cursor.visible = false;
                    currentState = State.RESTORING;
                }
                break;
            case State.RESTORING:
                //call light spot method, updates platform position
                if (lightSpot.GetComponent<Shape>().isRestored())
                {
                    currentState = State.MERGED;
                }
                transform.position = lightSpot.GetComponent<Shape>().getRestorePosition();
                break;
            case State.REPOSITIONING:
                mergingTime = Mathf.Max(mergingTime - Time.deltaTime, 0);
                Camera.main.transform.rotation = Quaternion.Lerp(character.transform.rotation * camLocalRotPrev, Camera.main.transform.rotation, mergingTime / 1);
                Camera.main.transform.position = Vector3.Lerp(character.transform.position + camLocalPosiPrev, Camera.main.transform.position, mergingTime / 1);
                transform.position = Vector3.Lerp(lightPosiPrev, lightSpot.transform.position, mergingTime / 1);
                if (mergingTime == 0)
                {
                    character.gameObject.GetComponent<FirstPersonController>().setCameraSuspended(false);
                    Camera.main.transform.SetParent(character.transform);
                    character.gameObject.GetComponent<FirstPersonController>().setMouseLock(true);
                    Cursor.visible = false;
                    lightSpot.GetComponentInParent<LightSpotArea>().setMerged(false);
                    currentState = State.IDLE;
                    
                }
                    
                break;
            case State.HOLDING:
                if (Input.GetKeyDown(KeyCode.E))
                {
                    transform.SetParent(null);
                    currentState = State.IDLE;
                }
                break;
        }
        circularProgressBar.GetComponent<CircularProgressBar>().SetProgress(
            (light.range - initialRange) / (maxRange - initialRange));
    }

    private void UpdateLightPosition()
    {
        RaycastHit hit;
        Vector3 destination = Target.transform.position;
        Vector3 castDir = (Target.transform.position - Camera.main.transform.position).normalized;
        boundDistance = (Target.transform.position - Camera.main.transform.position).magnitude;
        if (Physics.Raycast(Camera.main.transform.position, castDir, out hit, boundDistance + 1, ~(1 << 2)))
        {
            if (!Equals(hit.collider.name, "Point Light"))
            {
                destination = hit.point - castDir;
            }
        }

        transform.position = Vector3.Lerp(transform.position, destination, 0.1f);
    }

    private GameObject getAvailableLightSpot()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            if (hit.collider.CompareTag("LightSpot"))
            {
                if(hit.collider.GetComponentInParent<LightSpotArea>().isInLightSpotArea()) {
                    return hit.collider.gameObject;
                }
            }
        }
        return null;
    }

    private IEnumerator enterHoldingState(float duration)
    {
        Vector3 origin = transform.position;
        Vector3 dest = character.transform.position + Camera.main.transform.forward * 3;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            print(transform.position + "" + dest + time / duration);
            transform.position = Vector3.Lerp(origin, dest, time / duration);
            yield return new WaitForEndOfFrame();
        }
        transform.SetParent(lightObject.transform);
        transform.localPosition = new Vector3(0, 0, 0);
    }
}
