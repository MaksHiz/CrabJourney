using UnityEngine;
using UnityEngine.UI;

public class CoreManager : MonoBehaviour
{
    public static CoreManager Instance {  get; private set; }

    public UIManager UIManager { get; private set; }
    public AudioManager AudioManager { get; private set; }

    #region INITIALIZATION

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeManagers();
    }
    private void InitializeManagers()
    {
        UIManager = GetComponentInChildren<UIManager>();
        if (UIManager == null) 
        {
            Debug.LogWarning("UIManager of CoreManager not set to an instance.");
        }
        AudioManager = GetComponentInChildren<AudioManager>();
        if (AudioManager == null)
        {
            Debug.LogWarning("AudioManager of CoreManager not set to an instance.");
        }
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
