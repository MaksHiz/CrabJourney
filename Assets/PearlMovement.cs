using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PearlMovement : MonoBehaviour
{
    [SerializeField]
    private float force=1f;
    [SerializeField]
    private float switchDirection=2f;
    Rigidbody2D pearlrb;
    private Vector2 currDirect;
    private bool isMoving = true;
    void Start()
    {
       pearlrb=GetComponent<Rigidbody2D>();
       currDirect = Vector2.right;
        StartCoroutine(SwitchDirection());
    }
    void FixedUpdate()
    {
        if (isMoving)
        {
            pearlrb.AddForce(currDirect * force);
        }
    }
    private IEnumerator SwitchDirection()
    {
        while (true)
        {
            yield return new WaitForSeconds(switchDirection);
            currDirect = currDirect == Vector2.right ? Vector2.left : Vector2.right;
        }
    }
}
