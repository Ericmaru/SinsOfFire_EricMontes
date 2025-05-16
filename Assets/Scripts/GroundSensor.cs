using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSensor : MonoBehaviour
{
    public bool isGrounded;
    public bool canDoubleJump = true;
    private Rigidbody2D _rigidBody;

    void Awake()
    {
        _rigidBody = GetComponentInParent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D collider) 
    {
        if(collider.gameObject.layer == 3)
        {
            isGrounded = true;
            canDoubleJump = true;
        }

    }

    void OnTriggerStay2D(Collider2D collider) 
    {
        if(collider.gameObject.layer == 3)
        {
            isGrounded = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider) 
    {
        if(collider.gameObject.layer == 3)
        {
            isGrounded = false;
        } 
    }
}
