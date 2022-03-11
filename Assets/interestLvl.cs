using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class interestLvl : MonoBehaviour
{
    // Start is called before the first frame update
    public uint interestX;
    public double interest;

    public double twoSecondMark;
    void Start()
    {
        twoSecondMark = checkValueAtTwoSeconds();
        interestX=0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void interestFunc(){
        interest = 1*(Math.Pow(Math.E,(0.01*interestX)));  
    }

    //2 second continuous gaze @ 90 frames per second
    private double checkValueAtTwoSeconds(){
        return (1*(Math.Pow(Math.E,(0.01*180))));
    }
}
