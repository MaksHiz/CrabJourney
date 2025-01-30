using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text timer_text;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer_text.text = GameSave.CurrentSave.TimeSpentPlaying.ToString("0") + "s";
    }
}
