using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private Canvas _canvas;
    public GameObject Menu { set; get; }
    public GameObject Background { set; get; }

    void Awake()
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
        if (Menu != null) 
        { 
            Menu.SetActive(false); 
        }

        GameObject forReturn = Menu;
        Menu = menu;
        Menu.SetActive(true);
        return forReturn;
    }
}
