using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class pedastoolPuzzleHintSystem : pedastoolPuzzle
{
    public GameObject canvasWithTMP;
    public TMP_Text textmeshPro;
    private Dictionary<int, string> hints;
    // Start is called before the first frame update
    void Start()
    {
        hints = new Dictionary<int, string>();
        hints[0] = "A bloody book that seems disturbing. There are simbles you feel are occult. It seems worthy of burning.";
        hints[1] = "A black book full of mysterious text that is unintelligble. You feel it unworthy of display.";
        hints[2] = "They say the pen is mightier than the dagger, but I don't think this right. It seems worthy of display.";
        hints[3] = "A book of beautiful runes. An interesting seal lies at the center. It seems worthy of display.";
        hints[4] = "A book about beasts and greenery. A simple book of much use, but it's simple nonetheless. You feel it unworthy of display.";
        hints[5] = "This book seems unfinished. The text in a language you are familiar with. It must be fairly new. You feel it unworthy of display.";
        hints[6] = "A potion with little contents left. It has a potent smell that indicates it's power. It seems worthy of display.";
    }

    // Update is called once per frame
    void Update()
    {
        updateText();    
    }

    private int grabIndex(GameObject ob){
        int i = 0;
        foreach(GameObject meme in placeables){
            if (meme == ob){
                return i;
            }
            i++;
        }
        return -1;
    }

    public void updateText(){
        if(currentObject != null){
            int index = grabIndex(currentObject);
            Debug.Log(index);
            if(index != -1){
                textmeshPro.text = hints[index];
            }
        }else{
            textmeshPro.text = "Place an Item on this table. I can help you.";
        }
    }
}
