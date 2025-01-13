using UnityEngine;


public class CuttableTrash : MonoBehaviour
{
    public bool debug = false;
    private CollectableTrash[] _childTrash;

    public string pickUppableTrashTag = "CollectableTrash";

    void Awake()
    {
        _childTrash = this.transform.parent.gameObject.GetComponentsInChildren<CollectableTrash>();
        if (debug) 
        {
            Debug.Log(this.transform.parent.gameObject.name);
            foreach (CollectableTrash child in _childTrash)
            {
                Debug.Log(child);
            }
        }
        SetAllInactive();
        // 
    }

    public void Cut()
    {
        if (debug) Debug.Log("Cutting this object");
        // play animation
        SetAllActive();
        // foreach (CollectableTrash child in _childTrash)
        // {
        //     if (debug) Debug.Log("Spawning child object");
        //     child.Spawn();
        // }
        this.transform.gameObject.SetActive(false);
    }

    private void SetAllInactive()
    {
        foreach (CollectableTrash child in _childTrash)
        {
            child.transform.gameObject.SetActive(false);
        }
    }

    private void SetAllActive()
    {
        foreach (CollectableTrash child in _childTrash)
        {
            child.transform.gameObject.SetActive(true);
        }
    }

}