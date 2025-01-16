using UnityEditor;
using UnityEngine;

public class ClearPlayerPrefsEditor : Editor
{
    [MenuItem("Tools/Clear PlayerPrefs")]
    private static void ClearPlayerPrefs()
    {
        if (EditorUtility.DisplayDialog(
            "Clear PlayerPrefs",
            "Are you sure you want to clear all PlayerPrefs? This action cannot be undone.",
            "Yes",
            "No"))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("PlayerPrefs cleared successfully.");
        }
    }
}
