using UnityEngine;
using System.Collections;
using System;

public class VortexZone : TriggeredAction
{
    public float forceAtCenter = 50.0f;
    public float forceHalflife = 10.0f;
    

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
    }

    public override void onTriggerStay(PlayerStats stats, ControllerGame controllerGame)
    {
        Vector3 dPosition = gameObject.transform.localPosition - controllerGame.getPlayerPosition();

        float forceToApply = forceAtCenter * (Mathf.Pow(0.5f, dPosition.magnitude / forceHalflife));
        //Debug.Log("Force to apply : " + forceToApply);

        dPosition.Normalize();
        dPosition *= forceToApply;

        controllerGame.applyForceToPlayer(dPosition);
    }

    public override void onTriggerExit(PlayerStats stats, ControllerGame controllerGame)
    {
    }
}
