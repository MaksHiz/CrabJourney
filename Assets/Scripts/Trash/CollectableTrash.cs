using UnityEngine;
using System;

public class CollectableTrash : MonoBehaviour
{
    [SerializeField] private string leverName="";
    private bool isPickedUp = false;
    public int id=0;
    
    void Awake()
    {
        var element = GameSave.CurrentSave.TrashData[id];
        isPickedUp = element.Item2;
        if (!isPickedUp)
        {
            DateTime currentTime = DateTime.Now;
            int timeAsInt = (currentTime.Hour * 10000) + (currentTime.Minute * 100) + currentTime.Second;
            UnityEngine.Random.InitState(timeAsInt);
        }
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
        //Trash pickup sound
        if (UnityEngine.Random.Range(1,3)==1)  { AudioManager.Instance.PlaySFX("trash_pickup1"); }
        else { AudioManager.Instance.PlaySFX("trash_pickup2"); }

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