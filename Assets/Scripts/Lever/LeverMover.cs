using System.Collections.Generic;
using UnityEngine;

public class LeverMover : MonoBehaviour
{
    private Animator leverAnimator; // Reference to the Animator on the LeverMover child
    private bool isRight = false; // Tracks whether the lever is on the right side
    private bool isActivated = false; // Checks if the lever is functioning or the player still has to find it and place it down
    private GameObject leverMoverObj;
    [SerializeField] private List<RotateWall> walls;
    // (id,isPickedUp,isCutApart,isPlaced,LeverName)
    private (int, bool, bool, bool,string) leverData;
    private void Awake()
    {
        leverData=GameSave.CurrentSave.FindLeverDataByName(this.gameObject.name);
        Debug.Log("Lever Data:"+leverData);
        isActivated = leverData.Item4;
        foreach(Transform child in transform)
        {
            if (child.name == "LeverMover")
            {
                leverMoverObj = child.gameObject;
                break;
            }
        } // Find the LeverMover child and get its Animator component
        Debug.Log("LeverMoverObj:" + leverMoverObj);
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
        if (!isActivated)
        {
            leverData = GameSave.CurrentSave.FindLeverDataByName(this.gameObject.name);
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
        
        //HANDLE SOUND FOR MOVING WALLS AND LEVER PULLING
        AudioManager.Instance.PlaySFX("Lever_Interact");
        AudioManager.Instance.PlaySFX("Wall_Movement");

        foreach(RotateWall wall in walls)
        {
            wall.AnimateWall();
        }


    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (leverData.Item2 == true && isActivated!=true)
            {
                isActivated = true;
                //GameSave.CurrentSave.GetIsPickedUp(leverData.Item1,false);
                GameSave.CurrentSave.GetIsPlaced(leverData.Item1,true);
                leverMoverObj.SetActive(true);

                if (leverMoverObj != null)
                    leverAnimator = leverMoverObj.GetComponent<Animator>();
            }
        }
    }
}   
