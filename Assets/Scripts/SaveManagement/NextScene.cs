using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public Animator animator;
    public string nextScene;
    public string currentScene;
    void OnTriggerEnter2D(Collider2D other){
         if (other.CompareTag("Player")){
            GameSave.CurrentSave.LoadCrabToPosition = true;
            GameSave.CurrentSave.CrabPositionScene = nextScene;
            AudioManager.Instance.PlaySFX("jump_into_water");
            animator.SetTrigger("FadOut");
            StartCoroutine(WaitForAnimationAndChangeScene());
            if (currentScene == "Beach" && nextScene == "Level1") { GameSave.CurrentSave.CrabPosition = new Vector3((float)-20.1800003, (float)75.6600037, 0); }
            if (currentScene == "Level1" && nextScene == "Beach" ) { GameSave.CurrentSave.CrabPosition = new Vector3((float)97.8000031, (float)-3.16000009, 0); }
            if (currentScene == "Level1" && nextScene == "Cave1") { GameSave.CurrentSave.CrabPosition = new Vector3((float)68.4300003, (float)-10.3400002, 0); }
            if (currentScene == "Cave1" && nextScene == "Level1") { GameSave.CurrentSave.CrabPosition = new Vector3((float)-14.04, (float)23.6599998, 0); }
            if (currentScene == "Cave1" && nextScene == "DeepWater") { GameSave.CurrentSave.CrabPosition = new Vector3((float)-25.0400009, (float)-0.230387881, 0); }
            if (currentScene == "DeepWater" && nextScene == "Cave1") { GameSave.CurrentSave.CrabPosition = new Vector3((float)28.1900005, (float)-10.3400002, 0);  }
         }
    }
    private IEnumerator WaitForAnimationAndChangeScene(){
         yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
         SceneManager.LoadScene(nextScene);
    }
}
