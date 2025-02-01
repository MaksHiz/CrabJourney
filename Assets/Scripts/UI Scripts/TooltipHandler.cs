using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tooltipText;  // Assign this in the Inspector

    private void Start()
    {
        tooltipText.SetActive(false);  // Ensure tooltip is hidden at start
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipText.SetActive(true);  // Show tooltip
        UpdateTooltipPosition();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipText.SetActive(false);  // Hide tooltip when leaving button
    }

    private void Update()
    {
        if (tooltipText.activeSelf)
        {
            UpdateTooltipPosition();  // Keep tooltip following cursor
        }
    }

    private void UpdateTooltipPosition()
    {
        Vector2 mousePosition = Input.mousePosition;

        // Offset tooltip above cursor (adjust offset as needed)
        mousePosition.y += 105f;

        tooltipText.transform.position = mousePosition;
    }
}
