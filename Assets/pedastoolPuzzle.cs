using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;


public class pedastoolPuzzle : MonoBehaviour
{

    public GameObject desiredObject;
    public GameObject[] placeables;
    public GameObject[] placeablesDest;
    public GameObject currentObject;

    public bool AOIbool;
    public bool objectInPlace;

    // Start is called before the first frame update
    void Start()
    {
        currentObject = null;
        objectInPlace = false;
        AOIbool = false;
    }

    private bool isPlaceable(GameObject ob){
        foreach(GameObject meme in placeables){
            if(meme == ob){
                return true;
            }
        }
        return false;
    }

    public int grabePlaceableIndex(GameObject ob){
        for(int i = 0; i < placeables.Length; i++){
            if(ob == placeables[i]){
                return i;
            }
        }
        Debug.Log("dear god how did you get outside the for loop");
        return -1;
    }

private void OnTriggerEnter(Collider other)
{
    Debug.Log("entering" + other.gameObject);
    if(other.gameObject.GetComponent<Interactable>()){
        if(!other.gameObject.GetComponent<Interactable>().attachedToHand){
            if(!objectInPlace){
                Debug.Log("lerping");
                objectInPlace = true;
                StartCoroutine(lerpPedastool(other.gameObject, placeablesDest[grabePlaceableIndex(other.gameObject)]));
            }
        }
    }
}

    private void OnTriggerStay(Collider other)
    {
        if(desiredObject){
            if(other.gameObject == desiredObject){
                AOIbool = true;
            }
        }
    }

    private void OnTriggerExit(Collider other){
        if(other.gameObject.GetComponent<Interactable>()){
            if(other.gameObject.GetComponent<Interactable>().attachedToHand){
                if(other.gameObject == desiredObject){
                    AOIbool = false;
                }
                if(other.gameObject == currentObject){
                    objectInPlace = false;
                    currentObject = null;
                }
                Debug.Log("exiting" + other.gameObject);
            }
        }
    }

    private IEnumerator lerpPedastool(GameObject ob, GameObject objectDest){
        currentObject = ob;
        ob.GetComponent<Interactable>().enabled = false;
        Vector3 startPos = ob.transform.position;
        Quaternion startRot = ob.transform.rotation;
        Vector3 endPos = objectDest.transform.position;
        Quaternion endRot = objectDest.transform.rotation;
        float startDuration = 0;
        float lerpDuration = 1;
        while(startDuration < lerpDuration){
            ob.transform.position = Vector3.Lerp(startPos,endPos,startDuration/lerpDuration);
            ob.transform.rotation = Quaternion.Lerp(startRot,endRot,startDuration/lerpDuration);
            startDuration += Time.deltaTime;
            yield return null;
        }
        ob.transform.position = endPos;
        ob.transform.rotation = endRot;
        ob.GetComponent<Interactable>().enabled = true;
        ob.GetComponent<Rigidbody>().isKinematic = true;
    }
}
