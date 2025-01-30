using UnityEngine;

public class CursorHandler : MonoBehaviour
{
    public static CursorHandler Instance;

    [Header("Cursor Images")]
    public GameObject CursorPoint;  // The regular cursor (pointer image)
    public GameObject CursorClick;   // The clicked cursor (e.g., when the mouse is pressed)

    // Current cursor
    private GameObject _current_cursor;
    private bool _cursor_locked;

    [Header("Canvas Settings")]
    public RectTransform canvasRectTransform;  // The canvas the cursor is attached to

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;

            Cursor.visible = false;  // Hide the default system cursor
            _current_cursor = CursorPoint;  // Set initial cursor to normal

            if (_cursor_locked) return;  // Skip if the cursor is locked

            CursorPoint.SetActive(true);  // Show normal cursor
            CursorClick.SetActive(false);  // Hide clicked cursor
        }
        else
        {
            Destroy(this.gameObject);  // Ensure only one CursorManager exists
        }
    }

    private void Update()
    {
        if (Cursor.visible) Cursor.visible = false;  // Always hide system cursor

        if (_cursor_locked) return;  // Don't update cursor if locked

        // Update the cursor position based on the mouse position
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

            // Apply pixel-perfect rounding to snap to grid
            localPosition.x = Mathf.Round(localPosition.x);
            localPosition.y = Mathf.Round(localPosition.y);

            // Set the cursor's local position on the canvas
            _current_cursor.transform.localPosition = localPosition;
        }
    }

    private void HandleMouseClick()
    {
        if (Input.GetMouseButtonDown(0)) // Mouse click down
        {
            SwitchCursor(CursorClick);  // Switch to clicked cursor
        }
        else if (Input.GetMouseButtonUp(0)) // Mouse click up
        {
            SwitchCursor(CursorPoint);  // Switch to normal cursor
        }
    }

    private void SwitchCursor(GameObject newCursor)
    {
        if (_current_cursor != newCursor)
        {
            if (_current_cursor != null)
            {
                _current_cursor.SetActive(false);  // Hide the current cursor
                newCursor.transform.position = _current_cursor.transform.position;  // Keep the position
            }
            _current_cursor = newCursor;  // Switch cursor
            _current_cursor.SetActive(true);  // Show the new cursor
        }
    }

    public void DisableCursor()
    {
        _cursor_locked = true;  // Set locked state
        Cursor.lockState = CursorLockMode.Locked;  // Lock the cursor

        CursorPoint.SetActive(false);  // Hide the cursors while locked
        CursorClick.SetActive(false);
    }

    public void EnableCursor()
    {
        _cursor_locked = false;  // Unlock the cursor
        Cursor.lockState = CursorLockMode.None;  // Unlock the cursor in the system

        _current_cursor.SetActive(true);  // Show the current cursor
    }

    public void ToggleCursor()
    {
        if (_cursor_locked)
        {
            EnableCursor(); // Unlock and show the cursor
        }
        else
        {
            DisableCursor(); // Lock and hide the cursor
        }
    }
}
