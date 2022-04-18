using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class passwordCanvas : MonoBehaviour
{
    public GameObject[] AOIs;
    public TMP_Text password;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       revealPassword(); 
    }

    private void revealPassword(){
        if(checkItems()){
            password.text = "Judsen";
        }else{
            password.text = "I will tell you the password if you bring me the right items.";
        }
    }

    private bool checkItems(){
        foreach(GameObject meme in AOIs){
            if(!meme.GetComponent<pedastoolPuzzle>().AOIbool){
                return false;
            }
        }
        return true;
    }
}
