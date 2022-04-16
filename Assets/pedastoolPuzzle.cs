using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class pedastoolPuzzle : MonoBehaviour
{

    public GameObject desiredObject;
    public GameObject[] placeables;
    public GameObject[] placeablesDest;

    public bool AOIbool;

    // Start is called before the first frame update
    void Start()
    {
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

    private int grabePlaceableIndex(GameObject ob){
        for(int i = 0; i < placeables.Length; i++){
            if(ob == placeables[i]){
                return i;
            }
        }
        Debug.Log("dear god how did you get outside the for loop");
        return -1;
    }


    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject == desiredObject){
            AOIbool = true;
        }
    }

    private void OnTriggerExit(Collider other){
        if(other.gameObject == desiredObject){
            AOIbool = false;
        }
    }

    private IEnumerator lerpPedastool(GameObject ob, GameObject objectDest){
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
    }
}
