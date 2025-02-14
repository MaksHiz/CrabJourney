using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    #region REGION: Public Static Data
    // If the splash has been shown once, it won't be shown again.
    public static bool ShowSplash = true;
    public static MenuHandler Instance;
    #endregion

    #region REGION: Public Instance Data
    // Are we in the game or not.
    public bool InGame { get; private set; } = false;

    [Header("Screens")]
    public GameObject SplashScreen;
    public GameObject CursorScreen;
    public GameObject PauseScreen;
    public GameObject InGameUIScreen;
    public GameObject EndScreen;

    [Header("Title Menu")]
    public GameObject TitleMenu;
    public GameObject TitleMenuContinueText;

    [Header("Save Screen Data")]
    public GameObject[] SaveSlots;
    public GameObject[] SaveSlotInformationTexts;

    [Header("InGameUIScreen")]
    public GameObject TrashAmountText;
    
    [Header("EventSystem")]
    public GameObject EventSystem;
    #endregion

    #region REGION: Private Instance Data
    private GameObject _previous_screen = null;
    private GameObject _current_screen = null;

    // Current Menu State (used for Save Screen)
    private MenuState _state;

    // Is the game currently paused.
    private bool _paused = false;

    // Is there at least one save.
    private bool _can_continue = false;
    #endregion

    #region REGION: Unity Methods
    private void Awake()
    {
        if (Instance != null) 
        {
            Destroy(this.gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(this);

        if(!ShowSplash) Destroy(SplashScreen);

        EventSystem.SetActive(true);

        SplashScreen.SetActive(ShowSplash);
        CursorScreen.SetActive(true);

        _current_screen = TitleMenu;

        UpdateTitleContinueText();
        UpdateSaveSlots();
        SetState(0);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && (_paused || !InGame)) 
        {
            var aud = FindObjectOfType<AudioManager>();
            if(aud != null) aud.PlaySFX("Button Click");
        }
        // If we're in game, we can pause by pressing the ESCAPE button.
        if (InGame)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !EndScreen.activeInHierarchy && (SceneManager.GetActiveScene().name != "Introduction" && SceneManager.GetActiveScene().name != "Outro"))
            {
                if (_current_screen != PauseScreen) 
                {
                    ReturnToPreviousMenu();
                }
                TogglePause();
            }
        }
        // If we are not in game, and there is at least one save, by pressing any button on the keyboard (not ESCAPE), we continue the last played game.
        else if (_can_continue)
        {
            if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetKeyDown(KeyCode.Escape) && SplashScreen == null)
            {
                GameSave.CurrentSaveIndex = GameSave.LastPlayedSaveIndex;
                StartGame();
            }
        }
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Introduction"  && scene.name != "Outro" && InGame)
        {
            InGameUIScreen.SetActive(true);  // Now safe to activate after the scene load
        }

        // If the scene is the Introduction scene, make sure InGameUIScreen is disabled
        if ((scene.name == "Introduction" || scene.name == "Outro") && InGameUIScreen.activeInHierarchy)
        {
            InGameUIScreen.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    #endregion

    #region REGION: Public Methods
    public void GameEndButton() 
    {
        GameSave.SaveCurrentGame();
        if (GameSave.CurrentSave.TrashCount < GameSave.MAX_TRASH) 
        {
            GameSave.IsWin = 1;
        }
        else if (GameSave.CurrentSave.TrashCount >= GameSave.MAX_TRASH)
        {
            GameSave.IsWin = 2;
        }

        SceneManager.LoadScene("Outro");
    }

    public void BackToMainMenuAfterEnd() 
    {
        UpdateTitleContinueText();

        InGame = false;
        InGameUIScreen.SetActive(false);

        _paused = false;

        SceneManager.LoadScene("MainMenu");

        Destroy(gameObject);
    }

    public void LoadEndScreen() 
    {
        EndScreen.SetActive(true);
        CursorScreen.SetActive(true);
    }
    public void UnloadEndScreen() 
    {
        EndScreen.SetActive(false);
        CursorScreen.SetActive(false);
    }

    // Load a new menu specified as the parameter.
    public void LoadMenu(GameObject menu_to_load)
    {
        SwapScreens(_current_screen, menu_to_load);
    }

    // Return to the previous screen.
    public void ReturnToPreviousMenu()
    {
        if (_previous_screen != null) SwapScreens(_current_screen, _previous_screen);
    }

    // Set the state of the menu and update necessary parameters.
    public void SetState(int i)
    {
        UpdateSaveSlots();

        if (i == 1 || i == 0) _state = (MenuState)i;
        else return;

        if (i == 0) 
        {
            for (int ii = 0; ii < GameSave.Saves.Length; ii++)
            {
                SaveSlots[ii].GetComponent<Button>().interactable = true;
            }
        }
        else 
        {
            for (int ii = 0; ii < GameSave.Saves.Length; ii++)
            {
                if (GameSave.Saves[ii] == null) SaveSlots[ii].GetComponent<Button>().interactable = false;
            }
        }
    }

    // Toggle the pause.
    public void TogglePause()
    {
        if (SceneManager.GetActiveScene().name == "Introduction") return;

        _paused = !_paused;

        if (_paused)
        {
            Time.timeScale = 0f;
            CursorScreen.SetActive(true);
            Cursor.visible = false;
        }
        else
        {
            Time.timeScale = 1f;
            CursorScreen.SetActive(false);
            Cursor.visible = false;
        }

        PauseScreen.SetActive(_paused);
    }

    // Close the application.
    public void CloseGame()
    {
        Application.Quit();
    }

    // Start the game.
    public void StartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(GameSave.CurrentSave.CrabPositionScene);

        InGame = true;

        CursorScreen.SetActive(false);

        UpdateInGameUI(GameSave.CurrentSave.TrashCount);
        InGameUIScreen.GetComponent<InGameUIHandler>().UpdateFromSave();

        SwapScreens(_current_screen, PauseScreen);
        PauseScreen.SetActive(false);
    }

    // Process click on the save slot.
    public void SaveSlotClick(int i) 
    {
        if (_state == MenuState.SaveFromNew)
        {
            GameSave.Saves[i] = new GameSave(); 
        }

        GameSave.CurrentSaveIndex = i;
        _current_screen.SetActive(false);
        StartGame();
    }

    // Go back to main menu but save.
    public void BackToMainMenu() 
    {
        GameSave.SaveCurrentGame();
        UpdateTitleContinueText();

        InGame = false;
        InGameUIScreen.SetActive(false);

        _paused = false;

        SceneManager.LoadScene("MainMenu");

        Destroy(gameObject);
    }

    // Close the application but save.
    public void SaveAndQuit() 
    {
        GameSave.SaveCurrentGame();
        Application.Quit();
    }

    // Method which updates the InGameUI screen.
    public void UpdateInGameUI(int pickedUpTrash) 
    {
        TrashAmountText.GetComponent<TMP_Text>().text = pickedUpTrash.ToString("0");
    }
    #endregion

    #region REGION: Private Methods
    private void SwapScreens(GameObject current, GameObject next)
    {
        if (current != null) current.SetActive(false);
        if (next != null) next.SetActive(true);

        _previous_screen = current;
        _current_screen = next;
    }
    private void UpdateSaveSlots() 
    {
        for (int i = 0; i < GameSave.Saves.Length; i++)
        {
            if (GameSave.Saves[i] == null)
            {
                SaveSlotInformationTexts[i].GetComponent<Text>().text = "Empty";
            }
            else
            {
                string a = GameSave.Saves[i].PuzzleSolved ? "Yes" : "No";
                SaveSlotInformationTexts[i].GetComponent<Text>().text =
                    $"Time Played: {GameSave.Saves[i].TimeSpentPlaying.ToString("0")}s" +
                    $"\n\n" +
                    $"Trash Picked Up: {(int)(GameSave.Saves[i].TrashPickedUpPercent * 100)}%" +
                    $"\n\n" +
                    $"Puzzle Solved: {a}";
            }
        }
    }
    private void UpdateTitleContinueText()
    {
        if (GameSave.LastPlayedSaveIndex != -1)
        {
            TitleMenuContinueText.SetActive(true);
            _can_continue = true;
        }
        else
        {
            TitleMenuContinueText.SetActive(false);
            _can_continue = false;
        }
    }
    #endregion

    #region REGION: Enums
    public enum MenuState
    {
        SaveFromNew = 0,
        SaveFromLoad = 1
    };
    #endregion
}
