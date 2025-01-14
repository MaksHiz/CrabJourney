using UnityEngine;


public class CollectableTrash : MonoBehaviour
{
    [SerializeField] private string leverName="";
    void Awake()
    {
        // save ovdje
        // this.transform.gameObject.SetActive(false);
    }

    public void Spawn()
    {
        // this.transform.gameObject.SetActive(true);
        // idk jel treba ista
    }

    public void PickedUp()
    {
        this.transform.gameObject.SetActive(false);
        if (leverName!="")
        {
            GameObject lever = GameObject.Find(leverName);
            LeverMover levermover = lever.GetComponent<LeverMover>();
            levermover.setActivated(true);
        }
        
        // play animation ?
    }

}