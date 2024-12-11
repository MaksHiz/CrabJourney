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
        boxColClam = clam.GetComponent<BoxCollider2D>();
        pearlRigidBody= pearl.GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pearl") && !hasPearl)
        {
            hasPearl = true;
            boxColClam.enabled = false;
            pearl.transform.position = new Vector2((float)26.2299995, (float)-10.0270004);
            pearlRigidBody.bodyType = RigidbodyType2D.Static;
            Animator clamAnimator = clam.GetComponent<Animator>();
            clamAnimator.Play("ClamClosing");
            GameObject.Destroy(pearl);
            Debug.Log("Collided");
        }
        /*else
        {
            Debug.Log("Collided not with pearl");
        }*/
        
    }

}
