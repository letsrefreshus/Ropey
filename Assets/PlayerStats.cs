using UnityEngine;
using System.Collections;
using System;

public class PlayerStats : MonoBehaviour {
    private ControllerGame _controllerGame;

    public int _score { get; private set; } //Trying this out for size.

	// Use this for initialization
	void Start () {
        _score = 0;
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void addScore(int amount)
    {
        _score += amount;
    }

    public void resetScore()
    {
        _score = 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D Called");
        TriggeredAction triggeredAction = other.gameObject.GetComponent<TriggeredAction>();
        if(triggeredAction != null)
        {
            triggeredAction.onTriggerEnter(this, _controllerGame);
        }
    }

    public void setControllerGame(ControllerGame controllerGame)
    {
        _controllerGame = controllerGame;
    }
}
