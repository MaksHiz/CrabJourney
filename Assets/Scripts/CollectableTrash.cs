using UnityEngine;


public class CollectableTrash : MonoBehaviour
{

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
        // play animation ?
    }

}