using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PearlDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered the trigger is the pearl
        if (other.CompareTag("Pearl")) // Assuming you tag the pearl with "Pearl"
        {
            // Destroy both the clam and the pearl
            Destroy(other.gameObject); // Destroy the pearl
            Destroy(transform.parent.gameObject); // Destroy the clam (parent object)
            Debug.Log("Pearl and Clam Destroyed");
        }
    }
}
