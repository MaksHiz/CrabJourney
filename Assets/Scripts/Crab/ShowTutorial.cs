using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTutorial : MonoBehaviour
{
    public GameObject bubbletext;

    public Animator animator2;
    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")){
            animator2.SetBool("Entering", true);
        }
        // Debug.Log("Omg");
    }
    private void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Player")){
            animator2.SetBool("Entering", false);
        }
    }
}
