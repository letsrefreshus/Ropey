using UnityEngine;
using System.Collections;

public class WheelSpin : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 rotationSpeed = new Vector3(0f, 0f, 1f);
        gameObject.transform.Rotate(rotationSpeed);
	}
}
