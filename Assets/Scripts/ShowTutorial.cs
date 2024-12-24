using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTutorial : MonoBehaviour
{
    public GameObject bubbletext;

    void Start(){
        bubbletext.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")){
            bubbletext.SetActive(true);
        }
        Debug.Log("Omg");
    }
    private void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Player")){
            bubbletext.SetActive(false);
        }
    }
}
