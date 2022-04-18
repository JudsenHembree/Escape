using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorFade : MonoBehaviour
{
    public GameObject scripts;
    public GameObject doorMat;
    private bool gameOver;
    private Material doorMatCpy;
    // Start is called before the first frame update
    void Start()
    {
        doorMatCpy = doorMat.GetComponent<Renderer>().material;
        gameOver = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameOver){
            checkTask3();
        }
    }

    private IEnumerator fadeDoor(){
        gameOver = true;
        Color startColor = doorMatCpy.GetColor("_Color"); 
        Color endColor = new Color(0f,0f,0f,0f);
        Color shifting = doorMatCpy.GetColor("_Color");
        float lerpDuration = 10;
        float time = 0;
        while(time<lerpDuration){
            shifting = Color.Lerp(startColor, endColor, time/lerpDuration);
            doorMatCpy.SetColor("_Color", shifting);
            time += Time.deltaTime;
            yield return null;
        }
        doorMatCpy.SetColor("_Color", endColor);
        doorMat.GetComponent<BoxCollider>().enabled = false;
    }

    private void checkTask3(){
        if(scripts.GetComponent<story>().task3){
            StartCoroutine(fadeDoor());
        }
    }
}
