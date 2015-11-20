using UnityEngine;
using System.Collections;

public class TimedLife : MonoBehaviour {
    public float lifespan;

    private float _timeLived = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (ControllerGame.instance.isGameActive() == true)
        {
            _timeLived += Time.deltaTime;
            if(_timeLived > lifespan)
            {
                Destroy(gameObject);
            }
        }
	}
}
