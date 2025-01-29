using UnityEngine;
using System;
using UnityEngine.UIElements;

public class CharacterTrashHandler : MonoBehaviour
{
    public bool debug = false;

    public GameObject trashCutCollider;
    public GameObject trashPickupCollider;

    private TrashCut trashCut;
    private TrashPickup trashPickup;
    private float waitTimer=0f;
    [SerializeField] private float cuttingTime = 3f;
    private bool soundIsPlayed = false;

    public bool desiredTrashAction;

    private void Awake()
    {
        trashCut = trashCutCollider.GetComponent<TrashCut>();
        trashPickup = trashPickupCollider.GetComponent<TrashPickup>();
        DateTime currentTime = DateTime.Now;
        int timeAsInt = (currentTime.Hour * 10000) + (currentTime.Minute * 100) + currentTime.Second;
        UnityEngine.Random.InitState(timeAsInt);
        desiredTrashAction = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("TrashAction") && trashPickup.HasCloseTrash())
        {
            Debug.Log("If One");
            desiredTrashAction = true;
            if (debug) Debug.Log("TrashAction True");
        }
        else if (Input.GetButton("TrashAction") && trashCut.HasCloseCuttableTrash()) // While the button is held down
        {
            Debug.Log("If Two");
	    if (!soundIsPlayed) { Debug.Log("Cutting Trash"); AudioManager.Instance.PlaySFX("looped_cutting"); soundIsPlayed = true; }
            this.gameObject.GetComponent<Animator>().SetBool("isCutting", true);
            waitTimer += Time.deltaTime; // Increment the timer
            if (waitTimer >= cuttingTime)
            {
                desiredTrashAction = true; // Action is triggered after 3 seconds
                if (debug) Debug.Log("TrashAction True");
            }
        }
        else //if (Input.GetButtonUp("TrashAction") && trashCut.HasCloseCuttableTrash())
        {
	    Debug.Log("If Three");
            desiredTrashAction = false;
            if (debug) Debug.Log("TrashAction False");
        // }
        // else
        // {
            if (this.gameObject.GetComponent<Animator>().GetBool("isCutting"))
            {
		waitTimer = 0f;
                soundIsPlayed = false;
                this.gameObject.GetComponent<Animator>().SetBool("isCutting", false);
                AudioManager.Instance.StopSFX();
            }
                
        }
        if (desiredTrashAction)
        {
            desiredTrashAction = false;
            if (debug) Debug.Log("Attempting Trash Action");
            if (trashPickup.HasCloseTrash())
            {
                if (debug) Debug.Log("Attempting Collection");
                if (UnityEngine.Random.Range(1, 3) == 1) { AudioManager.Instance.PlaySFX("trash_pickup1"); }
                else { AudioManager.Instance.PlaySFX("trash_pickup2"); }
                trashPickup.Collect();
            }
            else if (trashCut.HasCloseCuttableTrash())
            {
                if (debug) Debug.Log("Attempting Cutting");
                trashCut.Cut();
            }
        }
    }


    private void FixedUpdate()
    {

    }
}