using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class shellHandle : MonoBehaviour
{
    [SerializeField] Sprite normalShell;
    [SerializeField] Sprite goldenShell;
    [SerializeField] GameObject TrashPickupCollider;
    private bool played=false;
    void Start()
    {
        Debug.Log("Percentage: "+GameSave.CurrentSave.TrashPickedUpPercent);
        if (GameSave.CurrentSave.TrashPickedUpPercent >= 1)
        {
            Debug.Log("Golden shell activated");
            AudioManager.Instance.PlaySFX("Gold_Effect");
            played = true;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = goldenShell;
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = normalShell;
        }
    }
    private void Update()
    {
        if (TrashPickupCollider.GetComponent<TrashPickup>().pickedUpTrash == 32 && played==false)
        {
            played = true;
            AudioManager.Instance.PlaySFX("Gold_Effect");
            Debug.Log("Golden shell activated updated");
            this.gameObject.GetComponent<SpriteRenderer>().sprite = goldenShell;
        }
    }
}
