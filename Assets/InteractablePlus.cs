using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class InteractablePlus : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        setOnAttachEvent();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void kinematicFalse(Hand hand){
        hand.currentAttachedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    private void setOnAttachEvent(){
        GetComponent<Interactable>().onDetachedFromHand += kinematicFalse;        
    }

}
