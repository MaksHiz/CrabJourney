using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerCve : MonoBehaviour
{
    // Start is called before the first frame update
    public float followspeed = 0.6f;
    public Transform target;
    public Vector3 offset = new Vector3(-2f, -1f, 0f); 

    // Update is called once per frame
    void Update()
    {
        Vector3 newPOs = new Vector3(target.position.x, target.position.y, -10f)+ offset;
        transform.position = Vector3.Slerp(transform.position, newPOs, followspeed*Time.deltaTime);
    }
}
