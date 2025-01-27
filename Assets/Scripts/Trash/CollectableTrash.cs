using UnityEngine;
using System;

public class CollectableTrash : MonoBehaviour
{
    [SerializeField] private string leverName="";
    private bool isPickedUp = false;
    public int id=0;
    
    public string GetLeverName()
    {
        return leverName;
    }

    void Awake()
    {
        var element = GameSave.CurrentSave.TrashData[id];
        isPickedUp = element.Item2;
        if (!isPickedUp){ }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    public void Spawn()
    {
        // this.transform.gameObject.SetActive(true);
        // idk jel treba ista
    }

    public void PickedUp()
    {
        isPickedUp = true;
        GameSave.CurrentSave.GetIsPickedUp(id, isPickedUp);
        if (leverName!="")
        {
            GameSave.CurrentSave.GetLeverName(id,leverName);
            //GameObject lever = GameObject.Find(leverName);
            //LeverMover levermover = lever.GetComponent<LeverMover>();
            //levermover.setActivated(true);
        }

        // play animation ?
        this.transform.gameObject.SetActive(false);
    }

}