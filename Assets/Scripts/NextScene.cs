using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public Animator animator;
    public string scene;
    void OnTriggerEnter2D(Collider2D other){
         if (other.CompareTag("Player")){
            animator.SetTrigger("FadOut");
            StartCoroutine(WaitForAnimationAndChangeScene());
         }
    }
    private IEnumerator WaitForAnimationAndChangeScene(){
         yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
         SceneManager.LoadScene(scene);
    }
}
