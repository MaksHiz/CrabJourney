using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EndScreenHandler : MonoBehaviour
{
    public float CursorShowAlpha;
    public SpriteRenderer Background;

    private CanvasGroup _endScreen;
    private float _radius;
    private GameObject _crab;
    // Start is called before the first frame update
    void Start()
    {
        _endScreen = MenuHandler.Instance.EndScreen.GetComponent<CanvasGroup>();
        _radius = GetComponent<CircleCollider2D>().radius;
    }

    private void Update()
    {
        if (_crab == null) return;
        _endScreen.alpha = (_radius - Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(_crab.transform.position.x, _crab.transform.position.y)))/_radius;
        Background.color = new Color(0,0,0,_endScreen.alpha*2);
        if (_endScreen.alpha > CursorShowAlpha && _endScreen.gameObject.activeInHierarchy)
        {
            MenuHandler.Instance.CursorScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
        else 
        {
            MenuHandler.Instance.CursorScreen.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            _crab = collision.gameObject;
            _endScreen.gameObject.SetActive(true);
            Background.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            _crab = null;
            _endScreen.gameObject.SetActive(false);
            Background.gameObject.SetActive(false);
            _endScreen.alpha = 0;
            Cursor.lockState = CursorLockMode.None;
        }
    }


}
