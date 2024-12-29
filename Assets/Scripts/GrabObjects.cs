using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObjects : MonoBehaviour
{
    [SerializeField]
    private Transform grabPoint; // Position to hold the grabbed object
    [SerializeField]
    private Transform rayPoint; // Position from where the raycast originates
    [SerializeField]
    private float rayDistance = 2f; // Raycast distance

    private GameObject grabbedObject; // Currently grabbed object
    private LayerMask grabLayerMask; // Mask for grabbable objects
    private LayerMask leverLayerMask; // Mask for lever objects
    private bool isFacingRight = true; // Current facing direction of the player
    
    public GameObject getGrabObject()
    {
        return grabbedObject;
    }

    private void Start()
    {
        grabLayerMask = LayerMask.GetMask("GrabObject"); // Mask for grabbable objects
        leverLayerMask = LayerMask.GetMask("Lever"); // Mask for levers
    }

    private void Update()
    {
        HandleMovement();

        // Determine ray direction based on facing direction
        Vector2 rayDirection = isFacingRight ? Vector2.right : Vector2.left;

        /*if (isFacingRight)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }*/

        // Debug: Draw the ray in the scene view
        Debug.DrawRay(rayPoint.position, rayDirection * rayDistance, Color.green);

        // Raycast to detect grabbable or interactable objects
        RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, rayDirection, rayDistance, grabLayerMask | leverLayerMask);

        //Debug.Log(grabbedObject);
        // Update the position of the grabbed object if holding one
        if (grabbedObject != null)
        {
            grabbedObject.transform.position = grabPoint.position;
            // Always release the object first if grabbedObject is not null
            if (Input.GetKeyDown(KeyCode.E))
            {
                ReleaseGrabbedObject();
            }
        }

        if (hitInfo.collider != null)
        {
            //Debug.Log("Deciding what to do");
            if (grabbedObject == null) // Only interact if no object is being held
            {
                if ((grabLayerMask & (1 << hitInfo.collider.gameObject.layer)) != 0)
                {
                    HandleGrabbableObject(hitInfo.collider.gameObject);
                }
                else if ((leverLayerMask & (1 << hitInfo.collider.gameObject.layer)) != 0)
                {
                    //Debug.Log("Lever Interaction");
                    HandleLever(hitInfo.collider.gameObject);
                }
            }
        }
    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetAxis("Horizontal") < 0)
        {
            if (isFacingRight)
            {
                FlipDirection(false); // Flip to face left
            }
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetAxis("Horizontal") > 0)
        {
            if (!isFacingRight)
            {
                FlipDirection(true); // Flip to face right
            }
        }
    }

    private void HandleGrabbableObject(GameObject targetObject)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Grab the object
            grabbedObject = targetObject;
            grabbedObject.GetComponent<Rigidbody2D>().isKinematic = true; // Disable physics
            grabbedObject.transform.position = grabPoint.position; // Move to grab point
            grabbedObject.GetComponent<Collider2D>().enabled = false; // Disable collider
        }
    }

    private void HandleLever(GameObject leverObject)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Get the Lever component from the object
            LeverMover lever = leverObject.GetComponent<LeverMover>();
            if (lever != null)
            {
                lever.Interact(); // Trigger the interaction logic
            }
            else
            {
                Debug.LogWarning("No Lever component found on the Lever object.");
            }
        }
    }



    private void ReleaseGrabbedObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject.GetComponent<Rigidbody2D>().isKinematic = false; // Enable physics
            grabbedObject.GetComponent<Collider2D>().enabled = true; // Enable collider
            grabbedObject = null; // Clear reference
        }
    }

    private void FlipDirection(bool facingRight)
    {
        isFacingRight = facingRight;

        if (isFacingRight)
        {
            // Face right
            rayPoint.localPosition = new Vector3(0.5f, 0, 0); // Adjust position for facing right
            rayPoint.localRotation = Quaternion.Euler(0, 0, 0); // Reset rotation
        }
        else
        {
            // Face left
            rayPoint.localPosition = new Vector3(0.65f, 0, 0); // Adjust position for facing left
            rayPoint.localRotation = Quaternion.Euler(0, 180, 0); // Rotate to face left
        }
    }
}

