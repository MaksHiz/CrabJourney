using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CounterBubble : MonoBehaviour
{
    public GameObject canvasCounter;
    public GameObject Cage;
    public TMP_Text myText; 

    void Start(){
        canvasCounter.SetActive(false);
    }

    void onTriggerEnter2D(Collider Other){
        if(Other.CompareTag("Player")){
            canvasCounter.SetActive(true);
        }
    }

    void Update(){
        if(myText.text != Cage.GetComponent<CageLogic>().getTrashAmount().ToString()){
            myText.text = Cage.GetComponent<CageLogic>().getTrashAmount().ToString();
        }
    }

}
