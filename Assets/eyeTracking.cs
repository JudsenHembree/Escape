using System.Collections;
using UnityEngine;
using ViveSR.anipal.Eye;

public class eyeTracking: MonoBehaviour
{
    private static Ray testRay;
    private static FocusInfo focusInfo;

    interestLvl example;

    public GameObject board;
    public GameObject[] objectsOfInterest;
    public Transform keyLocation;

    private Transform lerpDest;
    private Transform home;



    // Start is called before the first frame update
    void Start()
    {
        home = board.transform.Find("ouija key");
    }

    // Update is called once per frame
    void Update()
    {
        checkFocus();
        /*
        if(FocusName()=="a"){
            Debug.Log("a");
        }
        */
    }

    //This function stores info in focus info
    public static GameObject Focus()
    {
        if (SRanipal_Eye.Focus(GazeIndex.COMBINE, out testRay, out focusInfo)) { }
        else if (SRanipal_Eye.Focus(GazeIndex.LEFT, out testRay, out focusInfo)) { }
        else if (SRanipal_Eye.Focus(GazeIndex.RIGHT, out testRay, out focusInfo)) { }
        else return null;

        return focusInfo.collider.gameObject;
    }


    //call and then grabs name from focus()
    public static string FocusName()
    {
        if (Focus() is null)
        {
            return "null";
        }
        else
        {
            return Focus().name;
        }  
    }

    public void grow(uint x){
        if(x<270){
            x++; 
        }
    }

    public void decay(uint x){
        if (x>0){
            x--;
        }
    }

    public void interest(GameObject[] objects){
        GameObject temp = Focus();
        if (temp != null){
            foreach(GameObject meme in objects){
                if(meme == temp){
                    if(meme.GetComponent<interestLvl>() != null){
                        grow(meme.GetComponent<interestLvl>().interestX);
                    }else{
                        Debug.Log(meme.name + " should have the interestLvl script attached to it.");
                    }
                }else{
                    if(meme.GetComponent<interestLvl>() != null){
                        decay(meme.GetComponent<interestLvl>().interestX);
                    }else{
                        Debug.Log(meme.name + " should have the interestLvl script attached to it.");
                    }
                }
            }

        }
    }
    private void checkFocus(){
        GameObject temp = Focus();
        if(temp is null)
        {
            return;
        }
        else if(temp.transform.parent is null){
            return;
        }
        else if(temp.transform.parent.parent is null){
            return;
        }
        else if(temp.transform.parent.parent.gameObject==board)
        {
            lerpDest = temp.transform.parent.parent.Find("key destinations").Find(temp.name);
        }
    }

    private Transform determineLerpDest(GameObject[] objects){
        Transform lerpDest = home;
        foreach(GameObject meme in objects){
            if(meme.GetComponent<interestLvl>() == null){
                Debug.Log("There should be an interestLvl script attached to object " + meme.name);
            }else{
                if(meme.GetComponent<interestLvl>().interest>=meme.GetComponent<interestLvl>().twoSecondMark){
                    string name = meme.name;
                    lerpDest = meme.transform.parent.parent.Find("key destinations").Find(name);
                }
            }
        }
        return lerpDest;
    }
    /*
    //TODO: set up the actual lerp
    private void lerp(){

    }
    //TODO: finish this lerpandslerp func
    private void lerpAndSlerp(){
        if(startLerp){
            if (lerpDest != keyLocation){
                lerp()
            }
        }
        
    }
    */
}
