using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogeIntro : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    // Start is called before the first frame update
    void OnEnable(){
        StartCoroutine(TypeLine());
    }

    // Update is called once per frame
/*    void Update()
    {
         if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)){
            textSpeed = textSpeed * 0.3f; //ako se stisne space, enter ili desni klik miša text se ubrza
         }
   }*/
    IEnumerator TypeLine(){
        foreach (char c in lines[0].ToCharArray()){
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);

        }
    }

}
