using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTutorial : MonoBehaviour
{
    public GameObject bubbletext;

    void Start(){
        bubbletext.SetActive(false);
        if (bubbletext == null)
    {
        Debug.LogError("Bubbletext GameObject is not assigned in the Inspector!");
        return;
    }
    }
    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            bubbletext.SetActive(true);
        }
        Debug.Log("Omg");
    }
    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Player")){
            bubbletext.SetActive(false);
        }
    }
}
