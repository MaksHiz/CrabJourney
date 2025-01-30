using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogeIntro : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    [TextArea]
    public string[] lines;
    public float textSpeed;
    // Start is called before the first frame update
    public float fadeDuration = 2f;
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
        yield return StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut(){
        float startAlfa = textComponent.alpha;
        float timeElapsed = 0f;
        while(timeElapsed < fadeDuration){
            timeElapsed += Time.deltaTime;
            float alfa = Mathf.Lerp(startAlfa, 0f, timeElapsed / fadeDuration);
            textComponent.alpha = alfa;
            yield return null;
        }
        textComponent.alpha = 0f;
    }

}
