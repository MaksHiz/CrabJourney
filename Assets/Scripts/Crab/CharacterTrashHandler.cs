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
    }

    private void Update()
    {
        if (Input.GetButtonDown("TrashAction") && trashPickup.HasCloseTrash())
        {
            desiredTrashAction = true;
            if (debug) Debug.Log("TrashAction True");
        }
        else if (Input.GetButton("TrashAction") && trashCut.HasCloseCuttableTrash()) // While the button is held down
        {
            if (!soundIsPlayed) { Debug.Log("Cutting Trash"); AudioManager.Instance.PlaySFX("looped_cutting"); soundIsPlayed = true; }
            this.gameObject.GetComponent<Animator>().SetBool("isCutting", true);
            waitTimer += Time.deltaTime; // Increment the timer
            if (waitTimer >= cuttingTime)
            {
                desiredTrashAction = true; // Action is triggered after 3 seconds
                if (debug) Debug.Log("TrashAction True");
            }
        }
        else if (Input.GetButtonUp("TrashAction"))
        {
            desiredTrashAction = false;
            AudioManager.Instance.StopSFX();
            soundIsPlayed = false;
            waitTimer = 0f;
            if (debug) Debug.Log("TrashAction False");
        }
        else
        {
            if(this.gameObject.GetComponent<Animator>().GetBool("isCutting"))
                this.gameObject.GetComponent<Animator>().SetBool("isCutting", false);
        }
    }


    private void FixedUpdate()
    {

        if (desiredTrashAction)
        {
            desiredTrashAction = false;
            if (debug) Debug.Log("Attempting Trash Action");
            if (trashPickup.HasCloseTrash())
            {
                if (debug) Debug.Log("Attempting Collection");
                trashPickup.Collect();
            }
            else if (trashCut.HasCloseCuttableTrash())
            {
                if (debug) Debug.Log("Attempting Cutting");
                trashCut.Cut();
            }
        }

    }
}