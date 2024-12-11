using System.Collections.Generic;
using UnityEngine;

public class LeverMover : MonoBehaviour
{
    private Animator leverAnimator; // Reference to the Animator on the LeverMover child
    private bool isRight = false; // Tracks whether the lever is on the right side
    [SerializeField] private List<RotateWall> walls;
    private void Start()
    {
        // Find the LeverMover child and get its Animator component
        Transform leverMover = transform.Find("LeverMover");
        if (leverMover != null)
        {
            leverAnimator = leverMover.GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("LeverMover child not found or missing Animator component!");
        }
    }

    public void Interact()
    {
        if (leverAnimator == null)
        {
            Debug.LogWarning("Animator not set up correctly!");
            return;
        }

        // Toggle the isRight state
        isRight = !isRight;

        // Update the Animator parameter on the LeverMover child
        leverAnimator.SetBool("isRight", isRight);

        foreach(RotateWall wall in walls)
        {
            wall.AnimateWall();
        }

    }
}
