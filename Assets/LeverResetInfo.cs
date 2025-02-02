using UnityEngine;
using TMPro;

public class LeverResetInfo : MonoBehaviour
{
    public TMP_Text infoText; // Reference to the TextMeshPro Text
    public float moveDistance = 50f; // Distance the text moves up

    void Start()
    {
        if (infoText != null)
        {
            Color color = infoText.color;
            color.a = 0;
            infoText.color = color;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && infoText != null)
        {
            Color color = infoText.color;
            color.a = 1;
            infoText.color = color;
            infoText.transform.position += new Vector3(0, moveDistance, 0);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && infoText != null)
        {
            Color color = infoText.color;
            color.a = 0;
            infoText.color = color;
            infoText.transform.position -= new Vector3(0, moveDistance, 0);
        }
    }
}
