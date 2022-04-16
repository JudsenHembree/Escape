using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class story : MonoBehaviour
{
    public bool task1;
    public bool task2;
    public bool task3;

    public bool castle;
    public bool woman;

    public GameObject AOI1;
    public GameObject AOI2;
    public GameObject AOI3;

    public GameObject taskOneBookcaseStart;
    public GameObject taskOneBookcaseEnd;

    // Start is called before the first frame update
    void Start()
    {
        task1 = false;
        task2 = false;
        task3 = false;
        castle = false;
        woman = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!task1){
            task1 = checkPortraits();
        }
        if(!task2){
            task2 = checkAOIs();
        }
    }

    private bool checkAOIs(){
        bool aoi1 = AOI1.GetComponent<pedastoolPuzzle>().AOIbool;
        bool aoi2 = AOI2.GetComponent<pedastoolPuzzle>().AOIbool;
        bool aoi3 = AOI3.GetComponent<pedastoolPuzzle>().AOIbool;
        if(aoi1 && aoi2 && aoi3){
            return true;
        }
        return false;
    }
    private bool checkPortraits(){
        if(castle && woman){
            StartCoroutine(lerpBookcase());
            task1 = true;
            return true;
        }
        return false;
    }

    //move bookcase reveailing another room. 
    private IEnumerator lerpBookcase(){
        float timeElapsed=0;
        Vector3 start = taskOneBookcaseStart.transform.position;
        Vector3 dest = taskOneBookcaseEnd.transform.position;
        while(timeElapsed<10){
            taskOneBookcaseStart.transform.position=Vector3.Lerp(start,dest,timeElapsed/10);
            timeElapsed+=Time.deltaTime;
            yield return null;
        }
        taskOneBookcaseStart.transform.position = dest;
    }
}
