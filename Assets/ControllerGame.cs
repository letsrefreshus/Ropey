using UnityEngine;
using System.Collections;

public class ControllerGame : MonoBehaviour
{
    //Editor Member Variables
    public GameObject objPlayer;
     
    //Private Member Variables
    private Rigidbody2D _rigidbodyPlayer;
    private Vector2 _attachmentPoint;
    
	// Use this for initialization
	void Start ()
    {
        _rigidbodyPlayer = objPlayer.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0) == true)
        {
            Debug.Log("Click");
            _attachmentPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("Attachment Point : " + _attachmentPoint);
            DistanceJoint2D joint = objPlayer.AddComponent<DistanceJoint2D>();
            joint.anchor = new Vector2();
            joint.connectedAnchor = _attachmentPoint;
            joint.distance = Vector2.Distance(_attachmentPoint, objPlayer.transform.localPosition);
            joint.enableCollision = true;
        }
        if (Input.GetMouseButtonUp(0) == true)
        {
            Debug.Log("Unclick");
            Destroy(objPlayer.GetComponent<DistanceJoint2D>());
        }

        if (Input.GetMouseButton(0) == true)
        {
            //Debug.Log("Action Framey Stuff");
        }
	}
}
