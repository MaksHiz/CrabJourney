using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundHandler : MonoBehaviour
{
    public GameObject normal;
    public GameObject win;
    public GameObject gold_win;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene arg0, LoadSceneMode arg1)
    {
        normal.SetActive(false);
        win.SetActive(false);
        gold_win.SetActive(false);
        switch (GameSave.IsWin)
        {
            case 0:
                normal.SetActive(true);
                break;
            case 1:
                win.SetActive(true);
                break;
            case 2:
                gold_win.SetActive(true);
                break;
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
