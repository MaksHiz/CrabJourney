using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        OnSceneLoaded(new Scene(), LoadSceneMode.Single);
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
                Debug.Log("NORMAL! " + GameSave.IsWin);
                break;
            case 1:
                win.SetActive(true);
                Debug.Log("WIN! " + GameSave.IsWin);
                break;
            case 2:
                gold_win.SetActive(true);
                Debug.Log("GOLD WIN! " + GameSave.IsWin);
                break;
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
