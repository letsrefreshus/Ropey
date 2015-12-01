using UnityEngine;
using System.Collections;
using System;

public class TriggerSwitch : TriggeredAction
{
    public GameObject target;
    public bool setActiveTo = true;

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
        target.SetActive(setActiveTo);
    }

    public override void onTriggerStay(PlayerStats stats, ControllerGame controllerGame)
    {
    }

    public override void onTriggerExit(PlayerStats stats, ControllerGame controllerGame)
    {
    }
}
