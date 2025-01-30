using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class shellHandle : MonoBehaviour
{
    [SerializeField] Sprite normalShell;
    [SerializeField] Sprite goldenShell;
    [SerializeField] GameObject TrashPickupCollider;
    void Awake()
    {
        Debug.Log("Percentage: "+GameSave.CurrentSave.TrashPickedUpPercent);
        if (GameSave.CurrentSave.TrashPickedUpPercent >= 1)
        {
            Debug.Log("Golden shell activated");
            this.gameObject.GetComponent<SpriteRenderer>().sprite = goldenShell;
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = normalShell;
        }
    }
    private void Update()
    {
        if (TrashPickupCollider.GetComponent<TrashPickup>().pickedUpTrash == 32)
        {
            Debug.Log("Golden shell activated updated");
            this.gameObject.GetComponent<SpriteRenderer>().sprite = goldenShell;
        }
    }
}
