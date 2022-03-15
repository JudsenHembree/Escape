using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class interestLvl : MonoBehaviour
{
    // Start is called before the first frame update
    public int interestX;
    public double interest;
    public Image progress;

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
        interest = 1*(Math.Pow(Math.E,(0.1*interestX)));  
        updateSprite();
    }

    //janky ui for showing a person they are selecting a letter
    private void updateSprite(){
        if(progress != null){
            progress.fillAmount = (float)interest/(float)twoSecondMark;
        }
    }

    //1 second continuous gaze @ 90 frames per second
    private double checkValue(){
        return (1*(Math.Pow(Math.E,(0.1*90))));
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
