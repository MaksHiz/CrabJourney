using UnityEngine;
using UnityEngine.UI;

public class BackgroundAnimator : MonoBehaviour
{
    [Tooltip("Assign each frame from the sliced spritesheet manually")]
    public Sprite[] frames; // Array to hold sliced frames from the spritesheet
    public float baseFrameRate = 0.1f; // Base time between frames in seconds

    private Image uiImage;
    private int currentFrame;
    private float timer;

    void Start()
    {
        // Get the Image component on this GameObject
        uiImage = GetComponent<Image>();

        // Ensure there are frames assigned
        if (frames.Length == 0)
        {
            Debug.LogError("No frames assigned to the BackgroundAnimator!");
        }
    }

    void Update()
    {
        // Increment the timer based on elapsed time
        timer += Time.deltaTime;

        // If the timer exceeds the effective frame rate, switch frames
        if (timer >= baseFrameRate && frames.Length > 0)
        {
            // Reset the timer
            timer = 0f;

            // Move to the next frame, looping back if at the end
            currentFrame = (currentFrame + 1) % frames.Length;

            // Set the current frame as the source image
            uiImage.sprite = frames[currentFrame];
        }
    }
}