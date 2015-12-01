using UnityEngine;
using System.Collections;
using System;

public class VictoryZone : TriggeredAction
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void onTriggerEnter(PlayerStats stats, ControllerGame controllerGame)
    {
        controllerGame.gameOver(true);
    }

    public override void onTriggerStay(PlayerStats stats, ControllerGame controllerGame)
    {
    }

    public override void onTriggerExit(PlayerStats stats, ControllerGame controllerGame)
    {
    }
}
