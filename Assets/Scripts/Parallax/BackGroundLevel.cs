using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundLevelController : MonoBehaviour
{
    private float startPosX, startPosY, lengthX, lengthY;
    public GameObject cam;
    public float parallaxEffectX;
    public float parallaxEffectY;

    // Start is called before the first frame update
    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        lengthX = GetComponent<SpriteRenderer>().bounds.size.x;
        lengthY = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Calculate parallax effect for both axes
        float distanceX = cam.transform.position.x * parallaxEffectX;
        float distanceY = cam.transform.position.y * parallaxEffectY;
        float movementX = cam.transform.position.x * (1 - parallaxEffectX);
        float movementY = cam.transform.position.y * (1 - parallaxEffectY);

        // Update the position based on parallax effect
        transform.position = new Vector3(startPosX + distanceX, startPosY + distanceY, transform.position.z);

        // Loop the background horizontally
        if (movementX > startPosX + lengthX)
        {
            startPosX += lengthX;
        }
        else if (movementX < startPosX - lengthX)
        {
            startPosX -= lengthX;
        }

        // Loop the background vertically
        if (movementY > startPosY + lengthY)
        {
            startPosY += lengthY;
        }
        else if (movementY < startPosY - lengthY)
        {
            startPosY -= lengthY;
        }
    }
}
