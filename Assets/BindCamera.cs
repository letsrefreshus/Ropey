using UnityEngine;
using System.Collections;

public class BindCamera : MonoBehaviour {
    public GameObject objCamera;
    public float topBound;
    public float bottomBound;
    public float leftBound;
    public float rightBound;

    private Camera _camera;

	// Use this for initialization
	void Start () {
        _camera = objCamera.GetComponent<Camera>();
        Vector3 initialCameraOffset = new Vector3(0f, 0f, -10f);
        _camera.gameObject.transform.position = gameObject.transform.position + initialCameraOffset;
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 objPosition = _camera.WorldToViewportPoint(gameObject.transform.position);
        Vector3 deltaPosition = new Vector3();
        bool changePosition = false;    //Yay throwaway code

        if (objPosition.x < leftBound)
        {
            //Move the camera left
            Vector3 bound = new Vector3(leftBound, 0.5f, 0.0f);
            float dX = gameObject.transform.position.x - _camera.ViewportToWorldPoint(bound).x;
            deltaPosition.x = dX;
            changePosition = true;
        }
        else if (objPosition.x > (1f - rightBound))
        {
            //Move the camera right
            Vector3 bound = new Vector3((1f - rightBound), 0.5f, 0.0f);
            float dX = gameObject.transform.position.x - _camera.ViewportToWorldPoint(bound).x;
            deltaPosition.x = dX;
            changePosition = true;
        }
        if (objPosition.y < bottomBound)
        {
            //Move the camera down
            Vector3 bound = new Vector3(0.5f, bottomBound, 0.0f);
            float dY = gameObject.transform.position.y - _camera.ViewportToWorldPoint(bound).y;
            deltaPosition.y = dY;
            changePosition = true;
        }
        else if (objPosition.y > (1f - topBound))
        {
            //Move the camera up
            Vector3 bound = new Vector3(0.5f, (1f - topBound), 0.0f);
            float dY = gameObject.transform.position.y - _camera.ViewportToWorldPoint(bound).y;
            deltaPosition.y = dY;
            changePosition = true;
        }

        if (changePosition == true)
        {
            _camera.gameObject.transform.position = _camera.gameObject.transform.position + deltaPosition;
        }

        //Vector3 move = new Vector3(0.0f, 1.0f, 0.0f);
        //objCamera.transform.position = objCamera.transform.position + move;
    }
}
