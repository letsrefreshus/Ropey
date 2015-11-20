using UnityEngine;
using System.Collections;

public abstract class TriggeredAction : MonoBehaviour {
    public abstract void onTriggerEnter(PlayerStats stats, ControllerGame controllerGame);
}
