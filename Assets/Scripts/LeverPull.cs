using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField]
    private Animator leverAnimator; // Reference to the Animator component

    private bool isRight = false; // Current state of the lever

    // Method to interact with the lever, called by the Player script
    public void Interact()
    {
        ToggleLever();
    }

    private void ToggleLever()
    {
        isRight = !isRight; // Toggle the state
        leverAnimator.SetBool("isRight", isRight); // Update the animator parameter
        Debug.Log($"Lever toggled: Now isRight = {isRight}");
    }
}

