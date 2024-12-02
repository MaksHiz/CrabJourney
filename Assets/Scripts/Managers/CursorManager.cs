using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public CursorManager Instance;

    [Header("Cursor Images")]
    public GameObject normalCursor;  // The regular cursor (pointer image)
    public GameObject clickCursor;   // The clicked cursor (e.g., when the mouse is pressed)

    // Current cursor
    private GameObject _current_cursor;
    private bool _locked;

    [Header("Canvas Settings")]
    public RectTransform canvasRectTransform;

    private void Start()
    {
        if (Instance == null) 
        {
            Instance = this;

            Cursor.visible = false;
            _current_cursor = normalCursor;

            if (_locked) return;

            normalCursor.SetActive(true);
            clickCursor.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(Cursor.visible) Cursor.visible = false;
        if (_locked) return;

        UpdateCursorPosition();
        HandleMouseClick();
    }

    private void UpdateCursorPosition()
    {
        if (_current_cursor != null)
        {
            // Get the mouse position in screen space
            Vector2 mousePosition = Input.mousePosition;

            // Convert the screen position to local position on the canvas
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRectTransform, mousePosition, null, out Vector2 localPosition
            );

            // Apply pixel-perfect rounding
            localPosition.x = Mathf.Round(localPosition.x);
            localPosition.y = Mathf.Round(localPosition.y);

            // Set the cursor's local position on the canvas
            _current_cursor.transform.localPosition = localPosition;
        }
    }

    private void HandleMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SwitchCursor(clickCursor);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            SwitchCursor(normalCursor);
        }
    }

    private void SwitchCursor(GameObject newCursor)
    {
        if (_current_cursor != newCursor)
        {
            if (_current_cursor != null) 
            {
                _current_cursor.SetActive(false);
                newCursor.transform.position = _current_cursor.transform.position;
            }
            _current_cursor = newCursor;
            _current_cursor.SetActive(true);
        }
    }

    public void LockCursor()
    {
        _locked = true;
        Cursor.lockState = CursorLockMode.Locked;

        normalCursor.SetActive(false);
        clickCursor.SetActive(false);
    }

    public void UnlockCursor()
    {
        _locked = false;
        Cursor.lockState = CursorLockMode.None;

        _current_cursor.SetActive(true);

    }

    public void ToggleCursor()
    {
        if (_locked)
        {
            UnlockCursor(); // Unlock and show the cursor
        }
        else
        {
            LockCursor(); // Lock and hide the cursor
        }
    }
}
