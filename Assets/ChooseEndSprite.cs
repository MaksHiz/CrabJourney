using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseEndSprite : MonoBehaviour
{
    public GameObject WinScreen;
    public GameObject GoldWinSprite;

    private void Awake()
    {
        if (GameSave.CurrentSave.TrashCount < GameSave.MAX_TRASH)
        {
            WinScreen.SetActive(true);
            GoldWinSprite.SetActive(false);
        }
        else 
        {
            WinScreen.SetActive(false);
            GoldWinSprite.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        GameSave.DeleteCurrentGameSave();
    }
}
