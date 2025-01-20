using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class closeClam : MonoBehaviour
{
    private bool hasPearl=false;
    [SerializeField]
    private GameObject pearl;
    [SerializeField]
    private GameObject clam;
    private BoxCollider2D boxColClam;
    private Rigidbody2D pearlRigidBody;
    private void Start()
    {
        hasPearl = GameSave.CurrentSave.PuzzleSolved;
        boxColClam = clam.GetComponent<BoxCollider2D>();
        if (hasPearl)
        {
            Sprite clamSprite=clam.GetComponent<Sprite>();
            clamSprite = Resources.Load<Sprite>($"Sprites/Cave/skoljke3/skoljke3_6");
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
            clamAnimator.Play("ClamClosing");
            pearl.SetActive(false);
            GameSave.CurrentSave.GetPuzzleSolved(hasPearl);
            Debug.Log("Collided");
        }
        /*else
        {
            Debug.Log("Collided not with pearl");
        }*/
        
    }

}
