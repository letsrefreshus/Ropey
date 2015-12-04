using UnityEngine;
using System.Collections;
using System;

public class OneTimeSwitch : TriggeredAction
{
    public GameObject[] targets;
    public bool setActiveTo = true;
    public Sprite afterActivatedSprite;

    private bool _triggered = false;

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
        if (_triggered == false)
        {
            foreach (GameObject target in targets)
            {
                target.SetActive(setActiveTo);
            }
            if (afterActivatedSprite != null)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = afterActivatedSprite;
            }
            else
            {
                Destroy(gameObject);
            }
            _triggered = true;
        }
    }

    public override void onTriggerStay(PlayerStats stats, ControllerGame controllerGame)
    {
    }

    public override void onTriggerExit(PlayerStats stats, ControllerGame controllerGame)
    {
    }
}
