using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class interestLvl : MonoBehaviour
{
    // Start is called before the first frame update
    public int interestX;
    public double interest;

    public double twoSecondMark;
    void Start()
    {
        twoSecondMark = checkValue();
        interestX=0;
    }

    // Update is called once per frame
    void Update()
    {
        interestFunc();    
    }

    private void interestFunc(){
        interest = 1*(Math.Pow(Math.E,(0.01*interestX)));  
    }

    //1 second continuous gaze @ 90 frames per second
    private double checkValue(){
        return (1*(Math.Pow(Math.E,(0.01*90))));
    }

    public void grow(){
        if(interestX<90){
            interestX++; 
        }
    }

    public void decay(){
        if (interestX>0){
            interestX--;
        }
    }
}
