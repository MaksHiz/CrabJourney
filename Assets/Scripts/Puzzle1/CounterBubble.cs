using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class CounterBubble : MonoBehaviour
{
    public GameObject canvasCounter;
    public GameObject Cage;
    public TMP_Text myText; 

    void Start(){
        canvasCounter.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.CompareTag("Player")){
            canvasCounter.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Player")){
            canvasCounter.SetActive(false);
        }
    }

    void Update(){
        if(myText.text != Cage.GetComponent<CageLogic>().getTrashAmount().ToString()){
            myText.text = Cage.GetComponent<CageLogic>().getTrashAmount().ToString();
        }
         if(Cage.GetComponent<CageLogic>().getTrashAmount() == 0){
            canvasCounter.SetActive(false);
         }
    }

}
