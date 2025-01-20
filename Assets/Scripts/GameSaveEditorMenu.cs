using UnityEditor;
using UnityEngine;
using System;

public class GameSaveEditorWindow : EditorWindow
{
    private Vector2 scrollPosition;

    [MenuItem("Tools/Show Current Save")]
    public static void ShowWindow()
    {
        GetWindow<GameSaveEditorWindow>("Game Save Viewer");
    }

    private void OnGUI()
    {
        if (GameSave.CurrentSave == null)
        {
            EditorGUILayout.HelpBox("No current save is loaded. Please ensure there is an active save slot.", MessageType.Warning);
            if (GUILayout.Button("Refresh Saves"))
            {
                RefreshSaves();
            }
            return;
        }

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        // Display Current Save Index
        EditorGUILayout.LabelField("Current Save Index", GameSave.CurrentSaveIndex.ToString(), EditorStyles.boldLabel);

        // Display Last Played
        EditorGUILayout.LabelField("Last Played", GameSave.CurrentSave.LastPlayed.ToString("F"));

        // Display Puzzle Solved
        EditorGUILayout.LabelField("Puzzle Solved", GameSave.CurrentSave.PuzzleSolved.ToString());

        // Display Trash Count
        EditorGUILayout.LabelField("Trash Count", GameSave.CurrentSave.TrashCount.ToString());
        EditorGUILayout.LabelField("Trash Picked Up Percent", $"{GameSave.CurrentSave.TrashPickedUpPercent:P}");

        // Display Crab Position
        EditorGUILayout.LabelField("Crab Position", GameSave.CurrentSave.CrabPosition.ToString());
        EditorGUILayout.LabelField("Crab Position Scene", GameSave.CurrentSave.CrabPositionScene);

        // Display Trash Data
        EditorGUILayout.LabelField("Trash Data", EditorStyles.boldLabel);
        if (GameSave.CurrentSave.TrashData != null && GameSave.CurrentSave.TrashData.Count > 0)
        {
            for (int i = 0; i < GameSave.CurrentSave.TrashData.Count; i++)
            {
                var trash = GameSave.CurrentSave.TrashData[i];
                EditorGUILayout.LabelField($"Trash {i}",
                    $"ID: {trash.Item1}, PickedUp: {trash.Item2}, CutApart: {trash.Item3}, Placed: {trash.Item4}, LeverName: {trash.Item5}");
            }
        }
        else
        {
            EditorGUILayout.HelpBox("No trash data available.", MessageType.Info);
        }

        // Save & Refresh Buttons
        EditorGUILayout.Space(20);
        if (GUILayout.Button("Save Current Game"))
        {
            GameSave.SaveCurrentGame();
            Debug.Log("Current save saved.");
        }

        if (GUILayout.Button("Refresh Saves"))
        {
            RefreshSaves();
        }

        EditorGUILayout.EndScrollView();
    }

    private void RefreshSaves()
    {
        // Refreshes saves from PlayerPrefs
        var saves = GameSave.Saves;
        if (saves == null || saves.Length == 0)
        {
            Debug.LogError("No saves loaded.");
            return;
        }

        Debug.Log("Saves refreshed.");
        Repaint();
    }
}
