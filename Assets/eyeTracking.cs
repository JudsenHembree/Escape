using System.Collections;
using UnityEngine;
using ViveSR.anipal.Eye;

public class eyeTracking: MonoBehaviour
{
    private static Ray testRay;
    private static FocusInfo focusInfo;

    //the ouija board object
    public GameObject board;
    public float lerpDuration;
    private float timeElapsed;
    //objects of whose interest we track
    //namely the letters on board
    public GameObject[] objectsOfInterest;
    //current location of ouija key. updated throughout execution
    private Transform keyLocation;
    private Transform keyLocationResting;

    //where key is headed
    //will be home unless player is looking at letter
    private Transform lerpDest;
    //original location of key. It's like the center of the board
    private Transform home;

    private bool lerpin;



    // Start is called before the first frame update
    void Start()
    {
        home = board.transform.Find("ouija home");
        keyLocation = board.transform.Find("ouija key");
        lerpDest = home;
        setResting();
        lerpin=false;
    }

    // Update is called once per frame
    void Update()
    {
        interest(objectsOfInterest);
        lerpDest = determineLerpDest(objectsOfInterest);
        lerpandslerp();
        sendKeyHome();
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

    private bool noInterest(GameObject[] objects){
        foreach(GameObject meme in objects){
            if (meme.GetComponent<interestLvl>().interest != 1){
                return false;
            }
        }
        return true;
    }

    private void sendKeyHome(){
        if(noInterest(objectsOfInterest)){
            if(keyLocation != home){
                lerpDest = home;
                lerpin = true;
                StartCoroutine(lerp());
            }
        }
    }


    public void interest(GameObject[] objects){
        GameObject temp = Focus();
        if (temp != null){
            foreach(GameObject meme in objects){
                if(meme == temp){
                    if(meme.GetComponent<interestLvl>() != null){
                        meme.GetComponent<interestLvl>().grow();
                        //Debug.Log("growing "+meme);
                    }else{
                        Debug.Log(meme.name + " should have the interestLvl script attached to it.");
                    }
                }else{
                    if(meme.GetComponent<interestLvl>() != null){
                        meme.GetComponent<interestLvl>().decay();
                        //Debug.Log("decaying "+meme);
                    }else{
                        Debug.Log(meme.name + " should have the interestLvl script attached to it.");
                    }
                }
            }
        }
    }
    private Transform determineLerpDest(GameObject[] objects){
        Transform lerpDest = keyLocation;
        foreach(GameObject meme in objects){
            if(meme.GetComponent<interestLvl>() == null){
                Debug.Log("There should be an interestLvl script attached to object " + meme.name);
            }else{
                //probably need to adjust for multiple letters over the twosecondmark
                if(meme.GetComponent<interestLvl>().interest>=meme.GetComponent<interestLvl>().twoSecondMark){
                    //Debug.Log("setting new lerp dest " + meme);
                    string name = meme.name;
                    lerpDest = meme.transform.parent.parent.Find("key destinations").Find(name);
                }
            }
        }
        Debug.Log("Final lerp dest " + lerpDest.gameObject);
        return lerpDest;
    }
    //TODO: set up the actual lerp
    private IEnumerator lerp(){
        lerpin = true;
        timeElapsed=0;
        while(timeElapsed<lerpDuration){
            keyLocation.position=Vector3.Lerp(keyLocationResting.position,lerpDest.position,timeElapsed/lerpDuration);
            timeElapsed+=Time.deltaTime;
            yield return null;
        }
        keyLocation.position = lerpDest.position;
        setResting();
        lerpin = false;
    }

    private void lerpandslerp(){
        if(keyLocation.position != lerpDest.position && !lerpin){
            Debug.Log("lerpin");
            StartCoroutine(lerp());
        }
    }


    private void setResting(){
        keyLocationResting=keyLocation;
    }
}
