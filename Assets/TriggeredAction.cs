using UnityEngine;
using System.Collections;

public abstract class TriggeredAction : MonoBehaviour {
    public abstract void onTriggerEnter(PlayerStats stats, ControllerGame controllerGame);
    public abstract void onTriggerStay(PlayerStats stats, ControllerGame controllerGame);
    public abstract void onTriggerExit(PlayerStats stats, ControllerGame controllerGame);
}
