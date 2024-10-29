using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private Canvas canvas;
    public GameObject Current_Menu;

    public void ChangeScene(string scene_name) 
    {
        SceneManager.LoadScene(scene_name);
    }

    public void LoadMenu(GameObject menu) 
    {
        Current_Menu.SetActive(false);
        Current_Menu = menu;
        Current_Menu.SetActive(true);
    }

    private void InitializeCanvas() 
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        foreach (Transform child in canvas.transform)
        {
            child.gameObject.SetActive(false);
        }
        if (Current_Menu != null) 
        {
            Current_Menu.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeCanvas();
    }

    // Update is called once per frame
    void Update()
    {
        if (canvas == null) 
        {
            InitializeCanvas();
            if (canvas == null) Debug.LogError("This scene does not contain a Canvas tagged with 'Canvas' tag.");
        }
    }
}
