using UnityEngine;
using System.Collections;

public class HookshotBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collision Detected");
        if (collision.gameObject.CompareTag("Terrain") == true)
        {
            ControllerGame.instance.connectRope(collision.contacts[0].point);
            Destroy(gameObject);
        }
    }

    //Old way.
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Terrain") == true)
        {
            ControllerGame.instance.connectRope(gameObject.transform.position);
            Destroy(gameObject);
        }
    }
}
