using System;
using System.Collections.Generic;
using UnityEngine;

public class GameSave
{
    public static int MAX_TRASH = 100;
    [SerializeField] private GameObject crab;
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
        if (parts.Length < 6) throw new FormatException("Invalid save data.");

        LastPlayed = DateTime.Parse(parts[0]);
        PuzzleSolved = bool.Parse(parts[1]);
        TrashCount = int.Parse(parts[2]);

        // Parse CrabPosition
        var crabPositionParts = parts[3].Split(',');
        if (crabPositionParts.Length != 3) throw new FormatException("Invalid CrabPosition data.");
        CrabPosition = new Vector3(
            float.Parse(crabPositionParts[0]),
            float.Parse(crabPositionParts[1]),
            float.Parse(crabPositionParts[2])
        );

        // Parse CrabPositionScene
        CrabPositionScene = parts[4];

        // Parse TrashData
        TrashData = new List<(Vector3, bool, bool)>();
        if (!string.IsNullOrEmpty(parts[5]))
        {
            var trashEntries = parts[5].Split(';');
            foreach (var entry in trashEntries)
            {
                var trashParts = entry.Split(',');
                if (trashParts.Length != 5) throw new FormatException("Invalid TrashData entry.");
                TrashData.Add((
                    new Vector3(
                        float.Parse(trashParts[0]),
                        float.Parse(trashParts[1]),
                        float.Parse(trashParts[2])
                    ),
                    bool.Parse(trashParts[3]),
                    bool.Parse(trashParts[4])
                ));
            }
        }
    }


    // Creates a new GameSave object with the default values for all the fields.
    public GameSave()
    {
        LastPlayed = DateTime.Now;
        TrashCount = 0;
        PuzzleSolved = false;
        TrashData = new List<(Vector3, bool, bool)>();
    }
    #endregion

    #region Public Methods
    // Generates a string from the object which can be saved into PlayerPrefs easily.
    public override string ToString()
    {
        // Serialize TrashData as a semicolon-separated string
        string trashDataString = string.Join(";", TrashData.ConvertAll(td =>
            $"{td.Item1.x},{td.Item1.y},{td.Item1.z},{td.Item2},{td.Item3}"));

        return $"{LastPlayed:o}|{PuzzleSolved}|{TrashCount}|{CrabPosition.x},{CrabPosition.y},{CrabPosition.z}|{CrabPositionScene}|{trashDataString}";
    }

    // Method which saves the current game.
    public static void SaveCurrentGame()
    {
        if (!_is_valid_index(CurrentSaveIndex) || Saves[CurrentSaveIndex] == null)
        {
            Debug.LogError("Invalid save slot.");
            return;
        }

        CurrentSave.LastPlayed = DateTime.Now;
        CurrentSave.GetTrashAmmount();
        CurrentSave.GetCrabPosition();

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
    
    //MaksH functions for simplifying GameSave logic

    //  GET = local data goes into storage data (particularly used when closing the save
    //  or when an action is performed that invokes changing the save)
    //  SET = storage data goes into local data (particularly used when opening the save)

    //Used when storing the trash count data in the game save
    //probably should make the crab a don't destroy on load object
    //to keep the trash count consistent through scenes before going for a save
    public void GetTrashAmmount() {
        GameObject obj = GameObject.Find("TrashPickupCollider");
        TrashPickup trashPickup = obj.GetComponent<TrashPickup>();
        TrashCount = trashPickup.pickedUpTrash;
    }
    //Used for setting the trash data for the very first time when loading in the save
    public void SetTrashAmmount()
    {
        GameObject obj = GameObject.Find("TrashPickupCollider");
        TrashPickup trashPickup = obj.GetComponent<TrashPickup>();
        trashPickup.pickedUpTrash = TrashCount;
    }
    // Crab Position will be determined by the crab object that is attached to the GameSave object
    // can be changed if necessary but needs to be communicated
    public void GetCrabPosition() { CrabPosition = crab.transform.position; }
    public void SetCrabPosition() { crab.transform.position = CrabPosition; }

    //used for clam & pearl communication, not used in GameSave
    public void GetPuzzleSolved(bool isSolved) { PuzzleSolved = isSolved; }

    //used for cuttable trash communication, not used in GameSave
    public void GetIsCutApart(int id, bool isCut) {
        var element = TrashData[id];
        element.Item3 = isCut;
    }
    //used for collectable trash communication, not used in GameSave
    public void GetIsPickedUp(int id, bool isPickedUp) {
        var element = TrashData[id];
        element.Item2= isPickedUp;
    }
}
