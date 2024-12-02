using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private Canvas _canvas;
    private GameObject _menu;
    private GameObject _background;

    void Start()
    {
        if (Instance == null || Instance == this)
        {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;

            _canvas = GetComponentInChildren<Canvas>();
        }
        else 
        {
            Destroy(this);
        }
    }

    public GameObject LoadMenu(GameObject menu)
    {
        _menu.SetActive(false);
        GameObject forReturn = _menu;
        _menu = menu;
        _menu.SetActive(true);
        return forReturn;
    }

    public void DestroyAndLoadMenu(GameObject menu)
    {
        _menu.SetActive(false);
        Destroy(_menu);
        _menu = menu;
        _menu.SetActive(true);
    }

    public GameObject SetBackground(GameObject background) 
    {
        _background.SetActive(false);
        GameObject forReturn = _background;
        _background = background;
        _background.SetActive(true);
        return forReturn;
    }

    public void DestroyAndSetBackground(GameObject background) 
    {
        _background.SetActive(false);
        Destroy(_background);
        _background = background;
        _background.SetActive(true);
    }
}
