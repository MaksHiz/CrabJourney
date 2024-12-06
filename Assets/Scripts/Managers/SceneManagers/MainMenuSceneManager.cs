using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSceneManager : MonoBehaviour
{
    public GameObject Backgrounds;
    public GameObject Menus;

    public GameObject Background;
    public GameObject StartMenu;

    void Start()
    {
        LoadMenu(StartMenu);
        LoadBackground(Background);
    }

    public void LoadScene(string name)
    { 
        SceneManager.LoadScene(name);
        if(UIManager.Instance.Background != null) UIManager.Instance.Background.SetActive(false);
        if (UIManager.Instance.Menu != null) UIManager.Instance.Menu.SetActive(false);
    }

    public void LoadMenu(GameObject menuToLoad) 
    {
        GameObject instantiated = Instantiate(menuToLoad, Menus.transform);
        if (UIManager.Instance.Menu != null) UIManager.Instance.Menu.SetActive(false);
        UIManager.Instance.Menu = instantiated;
    }

    public void LoadBackground(GameObject backgroundToLoad) 
    {
        GameObject instantiated = Instantiate(backgroundToLoad, Backgrounds.transform);
        if (UIManager.Instance.Background != null) UIManager.Instance.Background.SetActive(false);
        UIManager.Instance.Background = instantiated;
    }
}
