using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PearlMovement : MonoBehaviour
{
    [SerializeField]
    private float force;
    private float internalForce;
    [SerializeField]
    private Vector2 startPos; 
    Rigidbody2D pearlrb;
    private Vector2 currDirect;
    private Vector2 prevPos;
    private Vector2 currPos;
    private bool isMoving = true;
    private float timer = 0f;
    private bool isPuzzleSolved = false;
    private int forceBehaviour = 0;
    void Awake()
    {
        isPuzzleSolved = GameSave.CurrentSave.PuzzleSolved;
        internalForce = force;
        if (!isPuzzleSolved)
        {
            pearlrb=GetComponent<Rigidbody2D>();
            currDirect = Vector2.left;
            prevPos = transform.position;
        }
        else
        {
            isMoving = false;
            this.gameObject.SetActive(false);
        }
       
    }
    void Update()
    {
        if (isMoving) { 
            currPos= transform.position;
            timer += Time.deltaTime;
            if (!AudioManager.Instance.sfxSource.isPlaying && Mathf.Abs(this.gameObject.GetComponent<Rigidbody2D>().velocity.x) > 1)
            {
                AudioManager.Instance.PlaySFX("Pearl_Roll");
            }
            if (Mathf.Abs(this.gameObject.GetComponent<Rigidbody2D>().velocity.x) < 1)
            {
                AudioManager.Instance.StopSFX();
            }
            if (timer >= 10f && internalForce!=0)
            {
                if (Mathf.Abs(prevPos.x-currPos.x)<1f && Mathf.Abs(prevPos.y - currPos.y) < 1f)
                {
                    transform.position = startPos;
                    currDirect = Vector2.left;
                }
                prevPos = currPos;
                timer = 0f;
            }
            pearlrb.AddForce(currDirect * internalForce);
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("DirectionSwitcher") && isMoving)
        {
            Debug.Log("Switched direction");
            if (forceBehaviour == 0)
            {
                internalForce = force + 0.5f;
                forceBehaviour = 1;
            }
            else
            {
                internalForce = force - 0.5f;
                forceBehaviour = 0;
            }
            currDirect = currDirect == Vector2.right ? Vector2.left : Vector2.right;
        }
        else if (collider.CompareTag("ForceDisabler") && isMoving)
        {
            Debug.Log("Switched direction");
            isMoving = false;
            force = 0;
            currDirect = Vector2.zero;
        }
        else if(collider.CompareTag("PearlReseter") && isMoving)
        {
            transform.position = startPos;
            currDirect = Vector2.left;
        }
    }
    

}
