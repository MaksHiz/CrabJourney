using UnityEngine;
using UnityEditor;

public class FindMissingScripts : EditorWindow
{
    [MenuItem("Tools/Find Missing Scripts")]
    public static void ShowWindow()
    {
        GetWindow(typeof(FindMissingScripts), false, "Find Missing Scripts");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Find Missing Scripts in Scene"))
        {
            FindInScene();
        }
    }

    private static void FindInScene()
    {
        GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>();
        int missingCount = 0;

        foreach (GameObject gameObject in gameObjects)
        {
            Component[] components = gameObject.GetComponents<Component>();

            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == null)
                {
                    missingCount++;
                    Debug.LogError($"Missing script found on GameObject: {gameObject.name}", gameObject);
                }
            }
        }

        Debug.Log($"Finished! Found {missingCount} missing scripts.");
    }
}
