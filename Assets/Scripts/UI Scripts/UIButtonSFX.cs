using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSFX : MonoBehaviour, IPointerEnterHandler
{
    private AudioManager aud;
    private void Awake()
    {
        aud = FindObjectOfType<AudioManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(aud == null) aud = FindObjectOfType<AudioManager>();
        aud.PlaySFX("Button Hover");
    }
}
