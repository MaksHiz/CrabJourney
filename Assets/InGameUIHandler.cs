using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text timer_text;
    public Image blue_bottle, green_bottle, red_bottle;

    // Update is called once per frame
    void Update()
    {
        timer_text.text = GameSave.CurrentSave.TimeSpentPlaying.ToString("0") + "s";
    }

    public void UpdateFromSave()
    {
        blue_bottle.gameObject.SetActive(true);
        red_bottle.gameObject.SetActive(true);
        green_bottle.gameObject.SetActive(true);

        var a = GameSave.CurrentSave.TrashData;
        if (a.Count > 3) 
        {
            if (a[0].Item2) Collect("red");
            if (a[1].Item2) Collect("green");
            if (a[2].Item2) Collect("blue");
            if (a[0].Item4) Place("red");
            if (a[1].Item4) Place("green");
            if (a[2].Item4) Place("blue");
        }
    }
    public void Collect(string b) 
    {
        switch (b)
        {
            case "red":
                red_bottle.color = new Color(255,255,255,255);
                break;
            case "blue":
                blue_bottle.color = new Color(255,255,255,255);
                break;
            case "green":
                green_bottle.color = new Color(255,255,255,255);
                break;
            default:
                Debug.Log("What the hell did you pick up whattttt!?");
                break;
        }
    }
    public void Place(string b) 
    {
        switch (b)
        {
            case "red":
                red_bottle.gameObject.SetActive(false);
                break;
            case "blue":
                blue_bottle.gameObject.SetActive(false);
                break;
            case "green":
                green_bottle.gameObject.SetActive(false);
                break;
            default:
                Debug.Log("What the hell did you place whattttt!?");
                break;
        }
    }
}
