using UnityEngine;
using System.Collections;
using System;

public class TeleportationZone : TriggeredAction
{
    public GameObject destination;

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
        controllerGame.removeHookshot();
        controllerGame.disonnectRope();
        controllerGame.movePlayerToGameObject(destination);
    }

    public override void onTriggerStay(PlayerStats stats, ControllerGame controllerGame)
    {
    }

    public override void onTriggerExit(PlayerStats stats, ControllerGame controllerGame)
    {
    }
}
