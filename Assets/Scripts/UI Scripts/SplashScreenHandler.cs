using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SplashScreenHandler : MonoBehaviour
{
    [Header("UI Elements")]
    public Image backgroundImage;        // White background
    public Image logoImage;              // Team logo
    public TextMeshProUGUI quoteText;    // Quote text

    [Header("Animation Settings")]
    public float fadeDuration = 1f;    // Time for fade-in/out (fixed seconds)
    public float displayDuration = 1f; // Hold time after fade-in (fixed seconds)

    [Header("Quotes Settings")]
    public string[] quotes;             // Array of quotes to randomize
    public string[] authors;            // Array of authors corresponding to the quotes

    private float timer = 0f;            // Timer to track animation progress
    private int stage = 0;               // 0: Fade-in, 1: Hold, 2: Fade-out

    private void Awake()
    {
        if (GameSettings.IsFullscreen) Debug.Log("Fullscreen");
    }

    void Start()
    {
        // Validate quotes and authors arrays
        if (quotes.Length > 0 && authors.Length > 0 && quotes.Length == authors.Length)
        {
            // Select a random index
            int randomIndex = Random.Range(0, quotes.Length);
            quoteText.text = $"{quotes[randomIndex]}\n\n-{authors[randomIndex]}";
        }
        else
        {
            // Fallback if arrays are mismatched or empty
            quoteText.text = "Default Quote\n\n-Default Author";
        }

        // Initialize all UI elements to be fully transparent
        SetAlpha(backgroundImage, 1); // Background is always visible
        SetAlpha(logoImage, 0);
        SetAlpha(quoteText, 0);
    }

    void Update()
    {
        HandleInput();
        AnimateSplash();
    }

    void HandleInput()
    {
        // If the user clicks or presses space, immediately advance to the next stage
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            SkipToNextStage();
        }
    }

    void AnimateSplash()
    {
        float delta = Time.deltaTime; // Regular animation progress
        timer += delta;

        if (stage == 0) // Fade-in stage
        {
            float alpha = Mathf.Clamp01(timer / fadeDuration); // Linear fade-in
            SetAlpha(logoImage, alpha);
            SetAlpha(quoteText, alpha);

            if (timer >= fadeDuration) // Ensure the fade-in completes after exactly fadeDuration
            {
                stage = 1; // Transition to hold stage
                timer = 0; // Reset timer
            }
        }
        else if (stage == 1) // Hold stage
        {
            if (timer >= displayDuration) // Hold the quote for exactly displayDuration
            {
                stage = 2; // Transition to fade-out stage
                timer = 0; // Reset timer
            }
        }
        else if (stage == 2) // Fade-out stage
        {
            float alpha = Mathf.Clamp01(1 - (timer / fadeDuration)); // Linear fade-out
            SetAlpha(backgroundImage, alpha);
            SetAlpha(logoImage, alpha);
            SetAlpha(quoteText, alpha);

            if (timer >= fadeDuration) // Ensure the fade-out completes after exactly fadeDuration
            {
                LoadNextScene(); // End the splash screen
            }
        }
    }

    void SkipToNextStage()
    {
        // Skip to the next stage immediately
        if (stage == 0) // Skip fade-in, make fully visible
        {
            stage = 1;
            timer = 0;
            SetAlpha(logoImage, 1);
            SetAlpha(quoteText, 1);
        }
        else if (stage == 1) // Skip hold, start fade-out
        {
            stage = 2;
            timer = 0;
        }
        else if (stage == 2) // Skip fade-out, end splash screen
        {
            LoadNextScene();
        }
    }

    void SetAlpha(Graphic uiElement, float alpha)
    {
        if (uiElement)
        {
            Color color = uiElement.color;
            color.a = alpha;
            uiElement.color = color;
        }
    }

    void LoadNextScene()
    {
        MenuHandler.ShowSplash = false;
        Destroy(this.gameObject);
    }
}
