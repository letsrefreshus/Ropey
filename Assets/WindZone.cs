using UnityEngine;
using System.Collections;
using System;

public class WindZone : TriggeredAction
{
    public float windForce = 15.0f;

    private float _windDirection;
    private Vector2 _force;

    // Use this for initialization
    void Start()
    {
        _windDirection = gameObject.transform.rotation.eulerAngles.z;
        updateForceVector();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void updateForceVector()
    {
        float radDirection = Mathf.Deg2Rad * _windDirection;
        _force = new Vector2(Mathf.Cos(radDirection) * windForce, Mathf.Sin(radDirection) * windForce);
    }


    //Overrides
    public override void onTriggerEnter(PlayerStats stats, ControllerGame controllerGame)
    {
    }

    public override void onTriggerStay(PlayerStats stats, ControllerGame controllerGame)
    {
        controllerGame.applyForceToPlayer(_force);
    }

    public override void onTriggerExit(PlayerStats stats, ControllerGame controllerGame)
    {
    }
}
