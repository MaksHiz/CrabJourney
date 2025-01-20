using System.Collections.Generic;
using UnityEngine;

public class LeverMover : MonoBehaviour
{
    private Animator leverAnimator; // Reference to the Animator on the LeverMover child
    private bool isRight = false; // Tracks whether the lever is on the right side
    private bool isActivated = false; // Checks if the lever is functioning or the player still has to find it and place it down
    private GameObject leverMoverObj;
    [SerializeField] private List<RotateWall> walls;
    // (pozicija,isPickedUp,isCutApart,isPlaced,LeverName,id)
    private (Vector3, bool, bool, bool,string, int) leverData;
    private void Start()
    {
        leverData=GameSave.CurrentSave.FindLeverDataByName(this.GetComponent<GameObject>().name);
        isActivated = leverData.Item4;
        leverMoverObj = GameObject.Find("LeverMover"); // Find the LeverMover child and get its Animator component

        if (isActivated)
        {
            leverMoverObj.SetActive(true);
            if (leverMoverObj != null)
                leverAnimator = leverMoverObj.GetComponent<Animator>();
            else
                Debug.LogError("LeverMover child not found or missing Animator component!");
        }
        else
        {
            leverMoverObj.SetActive(false);
        }    
    
    }
    private void Update()
    {
        //Debug.Log(this+" has been activated?" + isActivated + "contains: "+leverMoverObj.transform.position);
        if (isActivated)
        {
            leverMoverObj.SetActive(true);
            
            if (leverMoverObj != null)
                leverAnimator = leverMoverObj.GetComponent<Animator>();
        }
    }
    public void setActivated(bool activated) { isActivated = activated;}
    public bool getActivated() {  return isActivated; }
    public GameObject getLeverObject() { return leverMoverObj; }
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

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (leverData.Item2 == true)
            {
                isActivated = true;
                GameSave.CurrentSave.GetIsPlaced(leverData.Item6,true);
            }
        }
    }
}   
