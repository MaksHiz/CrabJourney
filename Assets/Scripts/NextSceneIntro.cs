using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NextSceneIntro : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameSave.CurrentSave.CrabPositionScene="beach";
        SceneManager.LoadScene("Beach");
    }

    // Update is called once per frame

}
