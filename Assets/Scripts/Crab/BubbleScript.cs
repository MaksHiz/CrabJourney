using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleScript : MonoBehaviour
{
     public Transform target; // The object to follow (e.g., your character)
    public Vector3 offset;  


     void Start(){
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
         if (target != null)
        {
            // Update the position of the bubble
            transform.position = target.position + offset;
        }
    }
}
