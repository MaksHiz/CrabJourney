using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class closeClam : MonoBehaviour
{
    private bool hasPearl=false;
    [SerializeField] private GameObject pearl;
    [SerializeField] private GameObject clam;
    [SerializeField] private Sprite finishedPuzzleSprite;
    private BoxCollider2D boxColClam;
    private Rigidbody2D pearlRigidBody;
    [SerializeField] private AudioSource pearlSource;
    [SerializeField] private AudioClip puzzleSolved;
    [SerializeField] private AudioClip close;
    private void Awake()
    {
        hasPearl = GameSave.CurrentSave.PuzzleSolved;
        boxColClam = clam.GetComponent<BoxCollider2D>();
        if (hasPearl)
        {
            Debug.Log("SolvedPuzzle!!");
            this.GetComponent<Animator>().enabled = false;
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = finishedPuzzleSprite;
            boxColClam.enabled = false;
        }
        else
        {
            pearlRigidBody= pearl.GetComponent<Rigidbody2D>();
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pearl") && !hasPearl)
        {
            hasPearl = true;
            boxColClam.enabled = false;
            pearl.transform.position = new Vector2((float)33.8300018,(float) -9.71000004);
            pearlRigidBody.bodyType = RigidbodyType2D.Static;
            Animator clamAnimator = clam.GetComponent<Animator>();
            pearlSource.PlayOneShot(close);    
            clamAnimator.Play("ClamClosing");
            GameSave.CurrentSave.GetPuzzleSolved(hasPearl);
            pearl.SetActive(false);
            // Debug.Log("Collided");
            StartCoroutine(DelayAudio(1.5f));
            
        }
        /*else
        {
            Debug.Log("Collided not with pearl");
        }*/
        
    }
    IEnumerator DelayAudio(float delay)
    {
        yield return new WaitForSeconds(delay);
        pearlSource.PlayOneShot(puzzleSolved);
    }

}
