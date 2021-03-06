using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ViveSR.anipal.Eye;

public class eyeTracking: MonoBehaviour
{
    private static Ray testRay;
    //eye memes
    private static FocusInfo focusInfo;
    //eye memes

    public GameObject story;
    public GameObject door;
    public GameObject board;
    //the ouija board object
    public GameObject castlePortrait;
    public GameObject womanPortrait;
    //the portrait
    public Material portraitMat;
    public Text portraitNormalText;
    public Text portraitRayText;
    public Text portraitAngleText;

    public float lerpDuration;
    //how long lerp last
    private float timeElapsed;
    //used in lerp
    public GameObject[] objectsOfInterest;
    //objects of whose interest we track
    //namely the letters on board
    private Transform keyLocation;
    //current location of ouija key. updated throughout execution
    private Transform keyLocationResting;
    //used as varable startlocation for lerps

    private Transform lerpDest;
    //where key is headed
    //will be current keyLocation unless player is looking at letter
    private Transform home;
    //original location of key. It's like the center of the board


    private bool lerpin;
    //used to prevent overlapping lerps. makes shit jump around no good. 

    private string ouijaWord; 
    //collect sequence entered on ouija board to pass to an array of words, 
    //call a func based of sequence
    private List<string> commands;
    //sequence of commands

    public Light[] lightsInScene;

    // Start is called before the first frame update
    void Start()
    {

        home = board.transform.Find("ouija home");
        keyLocation = board.transform.Find("ouija key");
        lerpDest = home;
        setResting();
        lerpin=false;
        commands = new List<string>();
        commands.Add("test");
        commands.Add("lights");
        commands.Add("judsen");

    }

    // Update is called once per frame
    void Update()
    {
        interest(objectsOfInterest);
        determineLerpDest(objectsOfInterest);
        lerpandslerp();
        sendKeyHome();
        portraitInterest();
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

    //func that determines if player has stopped looking at board
    private bool noInterest(GameObject[] objects){
        foreach(GameObject meme in objects){
            if (meme.GetComponent<interestLvl>().interest != 1){
                return false;
            }
        }
        return true;
    }

    //if we ain't lerpin or looking send key to home
    private void sendKeyHome(){
        if(!lerpin){
            if(noInterest(objectsOfInterest)){
                if(keyLocation.position != home.position){
                    checkWordForCommand(commands);
                    lerpDest = home;
                    ouijaWord="";
                    lerpin = true;
                    StartCoroutine(lerp());
                }
            }
        }
    }

    //check ouija word against list of commands then do stuff
    private void checkWordForCommand(List<string> x){
        if(!string.IsNullOrEmpty(ouijaWord)){
            foreach(string command in commands){
                if(ouijaWord == command){
                    if(command!="lights"){
                        Invoke(command, 0);
                    }else{
                        StartCoroutine(lights());
                    }
                }
            }
        }
    }

    private IEnumerator lights(){
        Debug.Log("calling lights()");
        float timeElapsed = 0;
        while(timeElapsed<3){
            foreach(Light meme in lightsInScene){
                meme.enabled = !meme.enabled;
            }
            timeElapsed+=.5f;
            yield return new WaitForSeconds(.5f);
        }
        foreach(Light meme in lightsInScene){
            meme.enabled = true;
        }
    }

    private void judsen(){
        Debug.Log("calling jud");
        story.GetComponent<story>().task3 = true;
    }

    private void test(){
        Debug.Log("test");
    }

    //calulates interest metric for each object we care about
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

    public void mapToLerpValue(GameObject port, float x){
        Material portraitMat = port.GetComponent<Renderer>().material;
        x = x/80;
        portraitMat.SetFloat("_LerpValue", x);
        if (x>.9){
            if (port == castlePortrait){
               GetComponent<story>().castle = true;
            }else{
               GetComponent<story>().woman = true;
            }
        }else{
            if (port == castlePortrait){
               GetComponent<story>().castle = false;
            }else{
               GetComponent<story>().woman = false;
            }
        }
    }

    public void portraitInterest(){
        GameObject temp = Focus();
        if (temp != null){
            if(temp == castlePortrait || temp == womanPortrait){

                Vector3 rayOrigin;
                Vector3 rayDirection;
                //focusInfo is the information from sranipal
                //Vector3 portraitNormal = focusInfo.normal;
                Vector3 portraitNormal = new Vector3(0,0,1);
                //testRay is the sranipal raycast
                SRanipal_Eye.GetGazeRay(GazeIndex.COMBINE, out rayOrigin, out rayDirection);

                rayOrigin=Camera.main.transform.TransformPoint(rayOrigin);
                rayDirection=Camera.main.transform.TransformDirection(rayDirection);
                float angle = Vector3.Angle(portraitNormal, rayDirection);

                /*
                portraitRayText.text = "Ray Direction: " +rayDirection.ToString();
                portraitNormalText.text = "Portrait Normal: "+portraitNormal.ToString();
                portraitAngleText.text = "Out Angle: "+angle.ToString();
                */

                mapToLerpValue(temp, angle);
            }
        }
    }
 


    //determines ouija key destination based of interest metric
    private void determineLerpDest(GameObject[] objects){
        if(!lerpin){
            lerpDest = keyLocation;
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
            //Debug.Log("Final lerp dest " + lerpDest.gameObject);
        }
    }

    //moves the key also manages the ouija word sequence
    private IEnumerator lerp(){
        //Debug.Log("lerpin");
        addLetter();
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

    //lerpandslerpbby... call this in update to start lerps
    private void lerpandslerp(){
        if(keyLocation.position != lerpDest.position && !lerpin){
            lerpin = true;
            StartCoroutine(lerp());
        }
    }

    //append ouija word
    private void addLetter(){
        if(lerpDest.name!="ouija home" && lerpDest.name != "ouija key"){
            ouijaWord+=lerpDest.name;
        }
    }

    //set resting ouija key position
    private void setResting(){
        keyLocationResting=keyLocation;
    }
}
