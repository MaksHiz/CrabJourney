using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    // Start is called before the first frame update
    public float followspeed = 2f;
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        Vector3 newPOs = new Vector3(target.position.x, target.position.y, -10f);
        transform.position = Vector3.Slerp(transform.position, newPOs, followspeed*Time.deltaTime);
    }
}
