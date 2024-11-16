using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObjects : MonoBehaviour
{
    [SerializeField]
    private Transform grabPoint;
    [SerializeField]
    private Transform rayPoint;
    [SerializeField]
    private float rayDistance;

    private GameObject grabbedObject;
    private LayerMask grabLayerMask;
    private bool isFacingRight = true;

    // Start is called before the first frame update
    private void Start()
    {
        grabLayerMask = LayerMask.GetMask("GrabObject"); // Mask for raycast
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetAxis("Horizontal") < 0)
        {
            // Player is moving left
            if (isFacingRight)
            {
                FlipDirection(false); // Flip to face left
            }
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetAxis("Horizontal") > 0)
        {
            // Player is moving right
            if (!isFacingRight)
            {
                FlipDirection(true); // Flip to face right
            }
        }
        Vector2 rayDirection = isFacingRight ? Vector2.right : Vector2.left; // Determine ray direction
        
        // Draw the ray in the Scene view for debugging
        Debug.DrawRay(rayPoint.position, transform.right * rayDistance, Color.green);

        // Raycast to detect objects in the "GrabObject" layer
        RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, transform.right, rayDistance, grabLayerMask);

        // If we hit an object with the correct layer, check if we want to grab it
        if (hitInfo.collider != null && grabbedObject == null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Grab the object
                grabbedObject = hitInfo.collider.gameObject;
                grabbedObject.GetComponent<Rigidbody2D>().isKinematic = true; // Prevent physics interactions
                grabbedObject.transform.position = grabPoint.position; // Move to grab position
                grabbedObject.GetComponent<Collider2D>().enabled = false; // Disable the collider
                //Debug.Log("Object Grabbed");
            }
        }
        // If we already have a grabbed object, check if we want to release it
        else if (grabbedObject != null && Input.GetKeyDown(KeyCode.E))
        {
            // Release the object
            grabbedObject.GetComponent<Rigidbody2D>().isKinematic = false; // Re-enable physics
            grabbedObject.GetComponent<Collider2D>().enabled = true; // Re-enable the collider
            grabbedObject = null;
            //Debug.Log("Object Released");
        }

        // If holding an object, keep it positioned at the grab point
        if (grabbedObject != null)
        {
            grabbedObject.transform.position = grabPoint.position;
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
            rayPoint.localPosition = new Vector3(1.58f, 0, 0); // Adjust position for facing left
            rayPoint.localRotation = Quaternion.Euler(0, 180, 0); // Rotate to face left
        }
    }
}
