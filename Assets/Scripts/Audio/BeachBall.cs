using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachBall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision) 
    { 
        if (collision.gameObject.CompareTag("Player")) 
        { 
            // if ( ! (AudioManager.Instance.sfxSource.clip != null 
            //     && AudioManager.Instance.sfxSource.clip.name == "beach_ball")   ) 
            //     {
                    AudioManager.Instance.PlaySFX("beach_ball");
                // }
        } 
    } 
}
