using System;
using System.Collections.Generic;
using UnityEngine;

public class GameSave
{
    public static int MAX_TRASH = 100;

    #region REGION: Properties
    // The three game saves, stores in an array.
    public static GameSave[] Saves { get; private set; }

    // The save index of the current save; default is -1 when no save is chosen yet.
    public static int CurrentSaveIndex { get; set; } = -1;

    // Gets the current save from the Saves array.
    public static GameSave CurrentSave => _is_valid_index(CurrentSaveIndex) ? Saves[CurrentSaveIndex] : null;

    // The index of the last played save; automatically decided in the static constructor.
    public static int LastPlayedSaveIndex { get; private set; }
    #endregion

    // All the game data that needs to be saved inside a game save.
    #region Fields
    // Last played timestamp.
    public DateTime LastPlayed { get; set; }

    // Has the puzzle cave been solved yet in this save or not.
    public bool PuzzleSolved { get; set; }

    // CRAB POSITION DATA
    public Vector3 CrabPosition { get; set; }
    public string CrabPositionScene { get; set; }

    // TRASH DATA
    public int TrashCount { get; set; }
    public float TrashPickedUpPercent { get { return TrashCount/(float)(MAX_TRASH); } }

    // (pozicija,isPickedUp,isCutApart)
    public List<(Vector3,bool,bool)> TrashData { get; set; }

    // LEVER DATA

    #endregion

    #region Constructor
    static GameSave()
    {
        _load_saves();
        _set_last_played_save();
    }

    // Loads a GameSave object from a string in case it is valid.
    public GameSave(string data)
    {
        var parts = data.Split('|');
        if (parts.Length < 3) throw new FormatException("Invalid save data.");

        LastPlayed = DateTime.Parse(parts[0]);
        PuzzleSolved = bool.Parse(parts[1]);
        TrashCount = int.Parse(parts[2]);
    }

    // Creates a new GameSave object with the default values for all the fields.
    public GameSave()
    {
        LastPlayed = DateTime.Now;
        TrashCount = 0;
        PuzzleSolved = false;
    }
    #endregion

    #region Public Methods
    // Generates a string from the object which can be saved into PlayerPrefs easily.
    public override string ToString()
        => $"{LastPlayed:o}|{PuzzleSolved}|{TrashCount}";

    // Method which saves the current game.
    public static void SaveCurrentGame()
    {
        if (!_is_valid_index(CurrentSaveIndex) || Saves[CurrentSaveIndex] == null)
        {
            Debug.LogError("Invalid save slot.");
            return;
        }

        CurrentSave.LastPlayed = DateTime.Now;
        PlayerPrefs.SetString($"GameSave{CurrentSaveIndex}", CurrentSave.ToString());
        PlayerPrefs.Save();

        LastPlayedSaveIndex = CurrentSaveIndex;
        Debug.Log($"Saved slot {CurrentSaveIndex}.");
    }
    #endregion

    #region Private Methods
    // Loads saves from PlayerPrefs.
    private static void _load_saves()
    {
        Saves = new GameSave[3];
        for (int i = 0; i < Saves.Length; i++)
        {
            if (PlayerPrefs.HasKey($"GameSave{i}"))
                Saves[i] = new GameSave(PlayerPrefs.GetString($"GameSave{i}"));
        }
    }

    // Sets the last played save from the Saves array.
    private static void _set_last_played_save()
    {
        LastPlayedSaveIndex = -1;
        for (int i = 0; i < Saves.Length; i++)
        {
            if (Saves[i] != null &&
                (LastPlayedSaveIndex == -1 || Saves[LastPlayedSaveIndex].LastPlayed < Saves[i].LastPlayed))
            {
                LastPlayedSaveIndex = i;
            }
        }
    }

    // Checks if the index for the save is valid.
    private static bool _is_valid_index(int index) => index >= 0 && index < Saves.Length;
    #endregion
}
