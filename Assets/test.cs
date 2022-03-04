using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Photon.Pun;
using System;
using System.IO;
using ViveSR.anipal.Eye;

public class test : MonoBehaviour
{
    private static Ray testRay;
    private static FocusInfo focusInfo;
    private bool cornerCheckingBool;
    public GameObject column;
    public GameObject corner;
    public Behaviour cornerLight;
    
    // Start is called before the first frame update
    void Start()
    {
        cornerCheckingBool = false;
    }

    // Update is called once per frame
    void Update()
    {
        columnRecolor();
        checkCorner();
    }

    public static GameObject Focus()
    {
        //if (EyeTool.GetVersion() == 1)
        //{
            if (SRanipal_Eye.Focus(GazeIndex.COMBINE, out testRay, out focusInfo)) { }
            else if (SRanipal_Eye.Focus(GazeIndex.LEFT, out testRay, out focusInfo)) { }
            else if (SRanipal_Eye.Focus(GazeIndex.RIGHT, out testRay, out focusInfo)) { }
            else return null;
        //}
        //else if (EyeTool.GetVersion() == 2)
        //{
            //if (SRanipal_Eye_v2.Focus(GazeIndex.COMBINE, out testRay, out focusInfo)) { }
            //else if (SRanipal_Eye_v2.Focus(GazeIndex.LEFT, out testRay, out focusInfo)) { }
            //else if (SRanipal_Eye_v2.Focus(GazeIndex.RIGHT, out testRay, out focusInfo)) { }
            //else return null;
        //}

        return focusInfo.collider.gameObject;
    }

    /// <summary>
    /// Checks name for current object in focus.
    /// </summary>
    /// 

    private void checkCorner()
    {
        if (FocusName() == "hint corner")
        {
            //Debug.Log(FocusName());
            corner.GetComponent<Renderer>().material.color = Color.black;
            if (!cornerCheckingBool)
            {
                StartCoroutine(recognizeCorner());
            }
        }
        else
        {
            corner.GetComponent<Renderer>().material.color = Color.magenta;
        }
    }

    IEnumerator recognizeCorner()
    {
        cornerCheckingBool = true;
        if(FocusName() == "hint corner")
        {
            yield return new WaitForSeconds(1);
            if(FocusName() == "hint corner")
            {
                yield return new WaitForSeconds(1);
                if(FocusName() == "hint corner")
                {
                    StartCoroutine(flashCorner());
                }
            }
        }
        cornerCheckingBool = false;
    }

    IEnumerator flashCorner()
    {
        cornerLight.enabled = !cornerLight.enabled;
        yield return new WaitForSeconds(1);
        cornerLight.enabled = !cornerLight.enabled;
        yield return new WaitForSeconds(1);
        cornerLight.enabled = !cornerLight.enabled;
        yield return new WaitForSeconds(1);
        cornerLight.enabled = !cornerLight.enabled;
    }

    private void columnRecolor()
    {
        if (FocusName() == "column")
        {
            //Debug.Log(FocusName());
            column.GetComponent<Renderer>().material.color = Color.black;
        }
        else
        {
            column.GetComponent<Renderer>().material.color = Color.magenta;
        }
    }

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
}
