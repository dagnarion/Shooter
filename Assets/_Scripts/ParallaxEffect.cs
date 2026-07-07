using System;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private float xParallaxValue;
    [SerializeField] private float yParallaxValue;
    [SerializeField] private Sprite baseSprite;
    private Camera cam;
    private float spriteLength;
    private Vector3 lastCamPosition;
    private Vector3 deltaMovement;

    private void Start()
    {
        cam = Camera.main;
       // spriteLength = this.GetComponent<SpriteRenderer>().bounds.size.x;
        spriteLength = baseSprite.bounds.size.x;
        lastCamPosition = cam.transform.position;
    }
    
    private void LateUpdate()
    {
        deltaMovement = cam.transform.position - lastCamPosition;
        transform.position += new Vector3(xParallaxValue * deltaMovement.x, yParallaxValue * deltaMovement.y, 0);
        lastCamPosition = cam.transform.position;
        if (Mathf.Abs(cam.transform.position.x - transform.position.x) >= spriteLength)
        {
            int multi = (cam.transform.position.x - transform.position.x) > 0 ? 1 : -1;
            transform.position = new Vector3(cam.transform.position.x + multi*spriteLength, transform.position.y, 0);
        }
        
    }
}
