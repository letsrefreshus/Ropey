using UnityEngine;
using System.Collections;
using System;

public class PointPickup : TriggeredAction {
    public int value = 10;
    public bool destroyOnPickup = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void onTriggerEnter(PlayerStats stats, ControllerGame controllerGame)
    {
        stats.addScore(value);
        if(destroyOnPickup == true)
        {
            Destroy(gameObject);
        }
    }
}
