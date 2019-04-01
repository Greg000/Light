using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class LightBehavior_old : MonoBehaviour
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
        HOLDING
    }

    private State currentState;
    private float minRange;
    private float boundDistance;
    private bool dropped;
    private Vector3 positionBeforeMerging;
    private float mergingTime;
    private GameObject lightObject;
    
    public bool isMerged()
    {
        return currentState == State.MERGED;
    }
    public void moveToLightSpot(Vector3 position)
    {
        transform.position = position;
    }
    // Use this for initialization
    private void Awake()
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
        
        //If the light is rising, change gravity to 1;
        if (currentState == State.RISING)
        {
            Physics.gravity = new Vector3(0, -2f, 0);
        } else
        {
            Physics.gravity = new Vector3(0, -9.8f, 0);
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
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    lightSpot = getAvailableLightSpot();
                    if (lightSpot != null)
                    {
                        character.gameObject.GetComponent<FirstPersonController>().locked = true;
                        //lightSpot.transform.Find("ShrinkingAura").GetComponent<ParticleSystem>().Play();
                        positionBeforeMerging = transform.position;
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
                ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
                var sz = ps.sizeOverLifetime;

                sz.enabled = true;
                if (Input.GetKey(KeyCode.Mouse0) && mergingTime < 2)
                {
                    float value = (mergingTime / 2 * mergingTime / 2);
                    light.gameObject.transform.position = Vector3.Lerp(positionBeforeMerging, lightSpot.transform.position, value);
                    //sz.sizeMultiplier = 1 + 4 * (mergingTime / 5);
                    mergingTime += Time.deltaTime;
                }
                else
                {
                    character.gameObject.GetComponent<FirstPersonController>().locked = false;
                    //lightSpot.transform.Find("ShrinkingAura").GetComponent<ParticleSystem>().Stop();
                    if (mergingTime >= 2)
                         currentState = State.MERGED;
                    else
                    {
                        mergingTime = 0;
                        sz.sizeMultiplier = 1;
                        currentState = State.IDLE;
                    }                 
                }   
                break;
            case State.MERGED:
                lightSpot.transform.parent.GetComponent<LightSpotArea>().setMerged(true);
                //The lightspot could be moving, so set the parent of Light to the lightSpot.
                transform.SetParent(lightSpot.transform);
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    currentState = State.IDLE;
                    transform.SetParent(null);
                    mergingTime = 0;
                    lightSpot.transform.parent.GetComponent<LightSpotArea>().setMerged(false);
                }
                if (Input.GetKey(KeyCode.Mouse0) && mergingTime > 0)
                {
                    //sz.sizeMultiplier = 1 + 4 * (mergingTime / 2);
                    mergingTime -= Time.deltaTime;
                }
                else
                {
                    //lightSpot.transform.Find("ExpandingAura").GetComponent<ParticleSystem>().Stop();
                    if (mergingTime <= 0)
                    {
                        currentState = State.IDLE;
                    }
                    else
                    {
                        mergingTime = 2;
                        //sz.sizeMultiplier = 2;
                        currentState = State.MERGED;
                    }
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
                if(hit.collider.transform.parent.GetComponent<LightSpotArea>().isInLightSpotArea()) {
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
