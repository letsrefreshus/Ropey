using UnityEngine;
using System.Collections;
using System;

public class SpeedLimitZone : TriggeredAction
{
    public float maxSpeed = 15.0f;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Overrides
    public override void onTriggerEnter(PlayerStats stats, ControllerGame controllerGame)
    {
        onTriggerStay(stats, controllerGame);
    }

    public override void onTriggerStay(PlayerStats stats, ControllerGame controllerGame)
    {
        Vector2 velocity = stats.gameObject.GetComponent<Rigidbody2D>().velocity;
        if(velocity.magnitude > maxSpeed)
        {
            velocity.Normalize();
            velocity *= maxSpeed;
            stats.gameObject.GetComponent<Rigidbody2D>().velocity = velocity;
        }
    }

    public override void onTriggerExit(PlayerStats stats, ControllerGame controllerGame)
    {
    }
}
