using System;
using System.Collections.Generic;
using UnityEngine;

public class GameSave
{
    public static int MAX_TRASH = 100;
    #region REGION: Properties
    // The three game saves, stores in an array.
    public static GameSave[] Saves { get; private set; }

    // The save index of the current save; default is 0 when no save is chosen yet.
    public static int CurrentSaveIndex { get; set; } = 0;

    // Gets the current save from the Saves array.
    public static GameSave CurrentSave => _is_valid_index(CurrentSaveIndex) ? Saves[CurrentSaveIndex] : null;

    // The index of the last played save; automatically decided in the static constructor.
    public static int LastPlayedSaveIndex { get; private set; }
    #endregion

    // All the game data that needs to be saved inside a game save.
    #region Fields
    // Last played timestamp.
    public bool LoadCrabToPosition = false;
    public DateTime LastPlayed { get; set; }

    // Has the puzzle cave been solved yet in this save or not.
    public bool PuzzleSolved { get; set; }

    // CRAB POSITION DATA
    public Vector3 CrabPosition { get; set; }
    public string CrabPositionScene { get; set; }

    // TRASH DATA
    public int TrashCount { get; set; }
    public float TrashPickedUpPercent { get { return TrashCount/(float)(MAX_TRASH); } }

    // (id,isPickedUp,isCutApart,isPlaced,LeverName)
    public List<(int,bool,bool,bool,string)> TrashData { get; set; }

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
        var crabPositionParts = parts[3].Split(';');
        foreach (var crab in crabPositionParts)
        {
            Debug.Log(crab);
        }
        // return $"{LastPlayed:o}|{PuzzleSolved}|{TrashCount}|{CrabPosition.x},{CrabPosition.y},{CrabPosition.z}|{CrabPositionScene}|{trashDataString}";
        if (crabPositionParts.Length != 3) throw new FormatException("Invalid CrabPosition data.");
        CrabPosition = new Vector3(
            float.Parse(crabPositionParts[0]),
            float.Parse(crabPositionParts[1]),
            float.Parse(crabPositionParts[2])
        );

        // Parse CrabPositionScene
        CrabPositionScene = parts[4];

        // change bool for positioning
        LoadCrabToPosition = true;

        // Parse TrashData
        TrashData = new List<(int, bool, bool, bool, string)>();
        if (!string.IsNullOrEmpty(parts[5]))
        {
            var trashEntries = parts[5].Split(';');
            foreach (var entry in trashEntries)
            {
                var trashParts = entry.Split(',');
                if (trashParts.Length != 5) throw new FormatException("Invalid TrashData entry.");

                int index = int.Parse(trashParts[0]);

                // Ensure TrashData has enough capacity
                while (TrashData.Count <= index)
                {
                    TrashData.Add((0, false, false, false, ""));
                }

                TrashData[index] = (
                    index,
                    bool.Parse(trashParts[1]),
                    bool.Parse(trashParts[2]),
                    bool.Parse(trashParts[3]),
                    trashParts[4]
                );
            }
        }
    }


    // Creates a new GameSave object with the default values for all the fields.
    public GameSave()
    {
        this.CrabPositionScene="Introduction";
        LastPlayed = DateTime.Now;
        TrashCount = 0;
        PuzzleSolved = false;
        TrashData = new List<(int, bool, bool, bool, string)>();
        CrabPosition = new Vector3(0, 0, 0);

        for (int i = 0; i < 100; i++)
        {
            TrashData.Add((i, false, false, false, ""));
        }

        TrashData[0] = (0, false, false, false, "RedLever");
        TrashData[1] = (1, false, false, false, "GreenLever");
        TrashData[2] = (2, false, false, false, "BlueLever");
    }
    #endregion

    #region Public Methods
    // Generates a string from the object which can be saved into PlayerPrefs easily.
    public override string ToString()
    {
        // Serialize TrashData as a semicolon-separated string
        string trashDataString = string.Join(";", TrashData.ConvertAll(td =>
            $"{td.Item1},{td.Item2},{td.Item3},{td.Item4},{td.Item5}"));

        return $"{LastPlayed:o}|{PuzzleSolved}|{TrashCount}|{CrabPosition.x};{CrabPosition.y};{CrabPosition.z}|{CrabPositionScene}|{trashDataString}";
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
        CurrentSave.SetTrashAmount();
        CurrentSave.GetCrabPosition();

        PlayerPrefs.SetString($"GameSave{CurrentSaveIndex}", CurrentSave.ToString());
        PlayerPrefs.Save();

        _load_saves();

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
    public void SetTrashAmount()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("TrashPickupCollider");
        if(obj != null) TrashCount = obj.GetComponent<TrashPickup>().pickedUpTrash;
    }

    public void SetTrashAmountFromInt(int trash_amount)
    {
        TrashCount = trash_amount;
    }
    //Used for setting the trash data for the very first time when loading in the save
    public void GetTrashAmount()
    {
        GameObject obj = GameObject.Find("TrashPickupCollider");
        TrashPickup trashPickup = obj.GetComponent<TrashPickup>();
        trashPickup.pickedUpTrash = TrashCount;
    }
    // Crab Position will be determined by the crab object that is attached to the GameSave object
    // can be changed if necessary but needs to be communicated
    public void GetCrabPosition() { CrabPosition = GameObject.Find("Crab").transform.position; }
    public void SetCrabPosition() { GameObject.Find("Crab").transform.position = CrabPosition; }

    //used for clam & pearl communication, not used in GameSave
    public void GetPuzzleSolved(bool isSolved) { PuzzleSolved = isSolved; }

    //used for cuttable trash communication, not used in GameSave
    public void GetIsCutApart(int id, bool isCut)
    {
        // Get the existing tuple
        var existingTuple = GameSave.CurrentSave.TrashData[id];

        // Replace the tuple with a new one that updates Item3 (isCut)
        GameSave.CurrentSave.TrashData[id] = (
            existingTuple.Item1, // Keep the same ID
            existingTuple.Item2, // Keep the same value for isPickedUp
            isCut,               // Update isCutApart
            existingTuple.Item4, // Keep the same value for isPlaced
            existingTuple.Item5  // Keep the same lever name
        );
    }
    //used for collectable trash communication, not used in GameSave
    public void GetIsPickedUp(int id, bool isPickedUp) {
        var existingTuple = GameSave.CurrentSave.TrashData[id];

        GameSave.CurrentSave.TrashData[id] = (
        existingTuple.Item1, // Keep the same ID
        isPickedUp,          // Update isPickedUp
        existingTuple.Item3, // Keep the same value for isCutApart
        existingTuple.Item4, // Keep the same value for isPlaced
        existingTuple.Item5  // Keep the same lever name
        );
    }
    //used for getting info of levers being placed down, not used in GameSave
    public void GetIsPlaced(int id, bool isPlaced)
    {
        var existingTuple = GameSave.CurrentSave.TrashData[id];

        GameSave.CurrentSave.TrashData[id] = (
        existingTuple.Item1, // Keep the same ID
        existingTuple.Item2,          // Update isPickedUp
        existingTuple.Item3, // Keep the same value for isCutApart
        isPlaced, // Keep the same value for isPlaced
        existingTuple.Item5  // Keep the same lever name
        );
    }
    //used for storing the levers name, not used in GameSave
    public void GetLeverName(int id, string lever_name)
    {
        var existingTuple = GameSave.CurrentSave.TrashData[id];

        GameSave.CurrentSave.TrashData[id] = (
        existingTuple.Item1, // Keep the same ID
        existingTuple.Item2,          // Update isPickedUp
        existingTuple.Item3, // Keep the same value for isCutApart
        existingTuple.Item4, // Keep the same value for isPlaced
        lever_name  // Keep the same lever name
        );
    }
    //used for finding the specific levers name we have to be searching for and giving data regarding if its picked up, placed or still at the same spot
    public (int, bool, bool, bool, string) FindLeverDataByName(string lever_name)
    {
        for(int i = 0; i < TrashData.Count; i++)
        {
           var element = TrashData[i];
           if (element.Item5 == lever_name)
            {
                return TrashData[i];
            }
        }
        return (-1,false,false,false,"none");
    }
}
