using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class TrashPickup : MonoBehaviour
{
    private List<GameObject> _closeByTrash;

    public int pickedUpTrash; 

    public string pickUppableTrashTag = "CollectableTrash";

    void Awake()
    {
        _closeByTrash = new List<GameObject>();
        if (GameSave.CurrentSave != null)
        {
            pickedUpTrash = GameSave.CurrentSave.TrashCount;
        }
        else
        {
            pickedUpTrash = 0;
        }
        
    }
    public void OnDestroy()
    {
        GameSave.CurrentSave.SetTrashAmountFromInt(pickedUpTrash);
    }

    public void Collect()
    {
            GameObject closest = FindClosest(_closeByTrash);
            if (closest != null)
            {
                closest.GetComponent<CollectableTrash>().PickedUp();
                if (closest.GetComponent<CollectableTrash>().GetLeverName() == "")
                {
                    PickupTrash();
                }
                 // activate pickup -> +1 counter...
                // closest.SetActive(false);
                // Destroy(closest); 
            }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag==pickUppableTrashTag)
        {
            if (!_closeByTrash.Contains(collision.gameObject))
                _closeByTrash.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag==pickUppableTrashTag)
        {
            _closeByTrash.Remove(collision.gameObject); // ili set to remove soon da moze i nes sto je nedavno proso pokupit
        }
    }

    private GameObject FindClosest(List<GameObject> _closeByTrash)
    {
        // List<GameObject> sortedByDist = _closeByTrash.OrderBy(o=>(
        //     Mathf.Abs(o.transform.position.sqrMagnitude - transform.position.sqrMagnitude)
        //     )).ToList();
        // return sortedByDist.Count >= 1 ? sortedByDist[0] : null;
        return _closeByTrash[0];
    }

    public bool HasCloseTrash()
    {
        return _closeByTrash.Count >= 1 ? true : false;
    }

    private void PickupTrash(){
        pickedUpTrash++;
        this.gameObject.GetComponentInParent<Animator>().Play("Collecting");
        MenuHandler.Instance.UpdateInGameUI(pickedUpTrash);
    }

}