using UnityEngine;
using System.Collections;
using System;

public class ToggleSwitch : TriggeredAction
{
    public GameObject[] targets;
    public Sprite onSprite;
    public Sprite offSprite;

    private SpriteRenderer _renderer;
    private bool _isOn = false;

    // Use this for initialization
    void Start()
    {
        _renderer = gameObject.GetComponent<SpriteRenderer>();
        updateSprite();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void updateSprite()
    {
        if(_isOn == true)
        {
            _renderer.sprite = offSprite;
        }
        else
        {
            _renderer.sprite = onSprite;
        }
    }

    //Overrides
    public override void onTriggerEnter(PlayerStats stats, ControllerGame controllerGame)
    {
        _isOn = !_isOn;
        foreach (GameObject target in targets)
        {
            target.SetActive(!target.activeSelf);
            updateSprite();
        }
    }

    public override void onTriggerStay(PlayerStats stats, ControllerGame controllerGame)
    {
    }

    public override void onTriggerExit(PlayerStats stats, ControllerGame controllerGame)
    {
    }
}
