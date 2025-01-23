using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageLogic : MonoBehaviour
{
    [SerializeField] private int neededTrashAmmount=20;
    [SerializeField] private Sprite emptyCage;
    [SerializeField] private GameObject redBottle;
    //(id,isPickedUp,isCaged,isPlaced,leverName)
    // 3rd one is usually isCutApart but it has a new function for checking what cage logic has to do
    private (int, bool, bool, bool, string) leverData;

    private void Awake()
    {
        leverData = GameSave.CurrentSave.FindLeverDataByName("RedLever");
        if (leverData.Item3 == true)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = emptyCage;
            redBottle.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && leverData.Item3!=true)
        {
            if (collision.gameObject.GetComponentInChildren<TrashPickup>().pickedUpTrash >= neededTrashAmmount)
            {
                GameSave.CurrentSave.GetIsCutApart(leverData.Item1,true);
                collision.gameObject.GetComponentInChildren<TrashPickup>().pickedUpTrash -= neededTrashAmmount;
                redBottle.SetActive(true);
                AudioManager.Instance.PlaySFX("Cage_Open");
                this.gameObject.GetComponent<SpriteRenderer>().sprite = emptyCage;
            }
            else
            {
                //add bubble tip if you can't get the bottle yet, not enough trash collected
            }
        }
    }
}
