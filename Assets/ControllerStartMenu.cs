using UnityEngine;
using System.Collections;

public class ControllerStartMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void loadLevel(int levelNum)
    {
        Application.LoadLevel("Level" + levelNum);
    }
}
