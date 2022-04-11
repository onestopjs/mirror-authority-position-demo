using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Vehicle : NetworkBehaviour
{
    [SyncVar] public bool controlled = false;
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (hasAuthority)
        {
            var throttle = Input.GetAxis("Vertical");
            var steer = Input.GetAxis("Horizontal");
            rb.AddForce((transform.forward * 20 * throttle) + (transform.right * 20 * steer));
        }
    }
}
