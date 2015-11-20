using UnityEngine;
using System.Collections;
using System;

public class LossZone : TriggeredAction
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
        controllerGame.gameOver(false);
    }
}
