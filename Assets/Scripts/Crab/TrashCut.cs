using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class TrashCut : MonoBehaviour
{
    public bool debug = false;

    private List<GameObject> _currentyCuttable;

    public string cuttableTrashTag = "CuttableTrash";

    void Awake()
    {
        _currentyCuttable = new List<GameObject>();
    }

    public void Cut()
    {
            GameObject closest = FindClosest(_currentyCuttable);
            CutTrash();
            if (closest != null)
            {
                closest.GetComponent<CuttableTrash>().Cut();
                // closest.SetActive(false);
                // Destroy(closest); 
            }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (debug) Debug.Log("Cut collision with " + collision.tag);
        if(collision.tag==cuttableTrashTag)
        {
            if (debug) Debug.Log("Trying to add " + collision.tag);
            if (!_currentyCuttable.Contains(collision.gameObject))
            {
                _currentyCuttable.Add(collision.gameObject);
                if (debug) Debug.Log("Added " + collision.tag);
            }
                
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag==cuttableTrashTag)
        {
            _currentyCuttable.Remove(collision.gameObject); // ili set to remove soon da moze i nes sto je nedavno proso pokupit
        }
    }

    private GameObject FindClosest(List<GameObject> _currentyCuttable)
    {
        // List<GameObject> sortedByDist = _currentyCuttable.OrderBy(o=>(
        //     Mathf.Abs(o.transform.position.sqrMagnitude - transform.position.sqrMagnitude)
        //     )).ToList();
        // return sortedByDist.Count >= 1 ? sortedByDist[0] : null;
        return _currentyCuttable[0];
    }

    public bool HasCloseCuttableTrash()
    {
        return _currentyCuttable.Count >= 1 ? true : false;
    }

    private void CutTrash(){
        //TODO animation
    }
}