using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectPearl : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {

        }
    }
}
