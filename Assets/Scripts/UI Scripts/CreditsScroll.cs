using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScroll : MonoBehaviour
{
    public float scrollSpeed = 50f;  // Speed of the scrolling text
    public float endY = 1000f;       // Y position where the credits should stop
    private RectTransform rectTransform;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.gameObject.GetComponent<TMP_Text>().text = $"You've completed this save in {GameSave.CurrentSave.TimeSpentPlaying.ToString("0")} seconds. \n\n" + rectTransform.gameObject.GetComponent<TMP_Text>().text;
    }

    void Update()
    {
        // Move text up
        rectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        // Stop scrolling when reaching endY
        if (rectTransform.anchoredPosition.y >= endY)
        {
            scrollSpeed = 0f; // Stop movement
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait(){
        yield return new WaitForSeconds(4f);
        MenuHandler.Instance.BackToMainMenuAfterEnd();
    }
}

