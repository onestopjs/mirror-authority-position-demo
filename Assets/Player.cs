using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SyncVar] private bool controllingVehicle = false;
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && hasAuthority)
        {
            if(!controllingVehicle)
            {
                CmdRequestVehicleControl();
            }else
            {
                CmdDropVehicleControl();
            }
        }
    }

    [Command]
    void CmdRequestVehicleControl()
    {
        var vehicle = FindObjectOfType<Vehicle>();
        if(!vehicle.controlled)
        {
            vehicle.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
            vehicle.controlled = true;
            controllingVehicle = true;
        }
    }

    [Command]
    void CmdDropVehicleControl()
    {
        var vehicle = FindObjectOfType<Vehicle>();
        vehicle.GetComponent<NetworkIdentity>().RemoveClientAuthority();
        vehicle.controlled = false;
        controllingVehicle = false;
    }

    private void FixedUpdate()
    {
        if (controllingVehicle || !hasAuthority) return;
        var movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        rb.AddForce(movement * 10);

        if(rb.velocity.magnitude > 0.5f)
        {
            var eulerRotation = Quaternion.LookRotation(rb.velocity.normalized).eulerAngles;
            transform.eulerAngles = new Vector3(0, eulerRotation.y, 0);
        }
    }
}
