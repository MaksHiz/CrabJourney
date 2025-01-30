using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public float followSpeedX = 1f; // Speed for X-axis
    public float followSpeedY = 3f; // Faster speed for Y-axis
    public Transform target;
    public Vector3 offset = new Vector3(2f, 1f, 0f);

    private void Start()
    {
        transform.position = target.position + offset;
    }

    void LateUpdate()
    {
        Vector3 targetPos = target.position + offset;
        Vector3 newPos = new Vector3(
            Mathf.Lerp(transform.position.x, targetPos.x, followSpeedX * Time.deltaTime), // Smooth follow on X
            Mathf.Lerp(transform.position.y, targetPos.y, followSpeedY * Time.deltaTime), // Faster follow on Y
            -10f // Fixed Z-position
        );

        transform.position = newPos;
    }
}
