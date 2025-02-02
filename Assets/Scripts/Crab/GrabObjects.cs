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
    private float rayDistance = 2f;
    [SerializeField]
    private float throwForce = 5f;
    private GameObject grabbedObject; 
    private LayerMask grabLayerMask; 
    private LayerMask leverLayerMask; 
    private bool isFacingRight = true; 
    private characterMovement characterMovement;

    public Animator animator;
    
    public GameObject getGrabObject()
    {
        return grabbedObject;
    }

    private void Awake()
    {
        characterMovement = GetComponent<characterMovement>();
    }

    private void Start()
    {
        grabLayerMask = LayerMask.GetMask("GrabObject");
        leverLayerMask = LayerMask.GetMask("Lever");
    }

    private void Update()
    {
        HandleMovement();
        Vector2 rayDirection = isFacingRight ? Vector2.right : Vector2.left;

        Debug.DrawRay(rayPoint.position, rayDirection * rayDistance, Color.green);

        RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, rayDirection, rayDistance, grabLayerMask | leverLayerMask);

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
    // DON'T REMOVE, NECESSARY FOR GRAB AND LEVER INTERACTION LOGIC
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
        if (Input.GetKeyDown(KeyCode.E) && !characterMovement.inShell)
        {
            // Grab the object
            AudioManager.Instance.PlaySFX("Pearl_Pickup");
            grabbedObject = targetObject;
            grabbedObject.GetComponent<Rigidbody2D>().isKinematic = true; // Disable physics
            grabbedObject.transform.position = grabPoint.position; // Move to grab point
            grabbedObject.GetComponent<Collider2D>().enabled = false; // Disable collider

            animator.SetBool("isHolding", true);
        }
    }

    private void HandleLever(GameObject leverObject)
    {
        LeverMover levermover=leverObject.GetComponent<LeverMover>();
        if (levermover.getActivated())
        {
            if (Input.GetKeyDown(KeyCode.E) && !characterMovement.inShell)
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
    }



    public void ReleaseGrabbedObject()
    {
        if (grabbedObject != null)
        {
            AudioManager.Instance.PlaySFX("Pearl_Throw");
            Rigidbody2D rb = grabbedObject.GetComponent<Rigidbody2D>();
            rb.isKinematic = false;
            grabbedObject.GetComponent<Collider2D>().enabled = true;
            rb.bodyType = RigidbodyType2D.Dynamic;
            grabbedObject = null; // Clear reference

            // Delay force application to ensure physics updates
            StartCoroutine(ApplyForceDelayed(rb));

            animator.SetBool("isHolding", false);
        }
    }
    private IEnumerator ApplyForceDelayed(Rigidbody2D rb)
    {
        yield return new WaitForFixedUpdate(); // Wait for physics update
        rb.AddForce(isFacingRight ? Vector2.right * throwForce : Vector2.left * throwForce, ForceMode2D.Impulse);
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

