using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSFX : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    private AudioManager aud;
    private void Awake()
    {
        aud = FindObjectOfType<AudioManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        aud.PlaySFX("Button Hover");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        aud.PlaySFX("Button Click");
    }
}
