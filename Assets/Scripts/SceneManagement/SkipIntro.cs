using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SkipIntro : MonoBehaviour
{
     public PlayableDirector playableDirector;
     void Update()
    {
        // Detect mouse press
        if (Input.GetKeyDown("space")) // ESC stisnut
        {
            playableDirector.time = playableDirector.duration; // Skip to the end
            playableDirector.Evaluate(); 
        }
    }
}
