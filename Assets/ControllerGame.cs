using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ControllerGame : MonoBehaviour
{
    public static ControllerGame instance;

    public static float MAX_ROPE_WIDTH = 3f;

    //Editor Member Variables
    public GameObject objPlayer;
    public GameObject prefabAttachmentPoint;
    public GameObject prefabAttachmentLine;
    public GameObject prefabHookshot;
    public GameObject objTime;
    public GameObject objScore;
    public GameObject objGameOverUI;
    public GameObject objGameUI;
    public GameObject objTotalTime;
    public GameObject objFinalScore;
    public GameObject objWinLose;
    public float hookshotSpeed = 10f;

    //Private Member Variables
    private PlayerStats _playerStats;
    private Rigidbody2D _rigidbodyPlayer;
    private Vector2 _attachmentPoint;
    private GameObject _objAttachmentPoint;
    private GameObject _objAttachmentLine;
    private float _attachmentDistance;
    private bool _gameActive = false;
    private Text _txtTime;
    private Text _txtScore;
    private Text _txtTotalTime;
    private Text _txtFinalScore;
    private Text _txtWinLose;
    private double _timeElapsed;
    private GameObject _objHookshot;

    // Use this for initialization
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        _playerStats = objPlayer.GetComponent<PlayerStats>();
        _rigidbodyPlayer = objPlayer.GetComponent<Rigidbody2D>();
        _txtTime = objTime.GetComponent<Text>();
        _txtScore = objScore.GetComponent<Text>();
        _txtTotalTime = objTotalTime.GetComponent<Text>();
        _txtFinalScore = objFinalScore.GetComponent<Text>();
        _txtWinLose = objWinLose.GetComponent<Text>();

        _playerStats.setControllerGame(this);

        startNewGame();
    }

    private void startNewGame()
    {
        _gameActive = true;
        _timeElapsed = 0;
        _playerStats.resetScore();
        objGameOverUI.SetActive(false);
        objGameUI.SetActive(true);

        Time.timeScale = 1;
    }

    void Update()
    {
        if (_gameActive == true)
        {
            //Update Time.
            _timeElapsed += Time.deltaTime;
            _txtTime.text = _timeElapsed.ToString("0.00");
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);


#if UNITY_ANDROID && !UNITY_EDITOR
            if(Input.touchCount > 0)
            {
                touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
                if (Input.GetTouch(0).phase == TouchPhase.Began)
#else
            if (Input.GetMouseButtonDown(0) == true)
#endif
            {
                //Debug.Log("Click");
                if(_objAttachmentLine && _objAttachmentPoint)
                {
                    disonnectRope();
                }
                else
                {
                    if(_objHookshot != null)
                    {
                        Destroy(_objHookshot);
                    }
                    _objHookshot = (GameObject)Instantiate(prefabHookshot, objPlayer.transform.position, Quaternion.identity);

                    Vector3 deltaPosition = (Vector3) touchPosition - objPlayer.transform.position;
                    deltaPosition.Normalize();
                    float rotZRaw = Mathf.Atan2(deltaPosition.y, deltaPosition.x);
                    float rotZ = rotZRaw * Mathf.Rad2Deg;
                    Debug.Log(rotZ.ToString());
                    _objHookshot.transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90);
                    _objHookshot.GetComponent<Rigidbody2D>().velocity = new Vector2(hookshotSpeed * Mathf.Cos(rotZRaw), hookshotSpeed * Mathf.Sin(rotZRaw));
                }
            }

#if UNITY_ANDROID && !UNITY_EDITOR
                if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary)
#else
            if (Input.GetMouseButton(0) == true)
#endif
            {
                //Add some controls for swipes here.
            }

#if UNITY_ANDROID && !UNITY_EDITOR
                if(Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)
#else
            if (Input.GetMouseButtonUp(0) == true)
#endif
            {
                //Debug.Log("Unclick");
            }
#if UNITY_ANDROID && !UNITY_EDITOR
            }
#endif

            if (_objAttachmentPoint && _objAttachmentLine)
            {
                //Update the rotation of the attachment line.
                Vector3 delta = _objAttachmentPoint.transform.position - objPlayer.transform.position;
                delta.Normalize();
                float rotZ = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
                _objAttachmentLine.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + 180);

                //Update the size of the attachment line.
                float oneToXScale = 1f;
                float newDistance = Vector3.Distance(_objAttachmentPoint.transform.position, objPlayer.transform.position);
                Vector3 newScale = new Vector3(newDistance * (1f / oneToXScale), Math.Min(_attachmentDistance / newDistance, MAX_ROPE_WIDTH), 1f);
                _objAttachmentLine.transform.localScale = newScale;
            }

            updateScoreUI();    //The fast to code way...
        }
    }

    public void connectRope(Vector3 attachmentPoint)
    {
        _attachmentPoint = attachmentPoint;
        _attachmentDistance = Vector2.Distance(_attachmentPoint, objPlayer.transform.localPosition);
        Debug.Log("Attachment Point : " + _attachmentPoint);
        DistanceJoint2D joint = objPlayer.AddComponent<DistanceJoint2D>();
        joint.anchor = new Vector2();
        joint.connectedAnchor = _attachmentPoint;
        joint.distance = _attachmentDistance;
        joint.enableCollision = true;
        joint.maxDistanceOnly = true;

        _objAttachmentPoint = (GameObject)Instantiate(prefabAttachmentPoint, _attachmentPoint, Quaternion.identity);
        Vector3 scale = new Vector3(3f, 3f, 3f);
        _objAttachmentPoint.transform.localScale = scale;

        _objAttachmentLine = (GameObject)Instantiate(prefabAttachmentLine, _attachmentPoint, Quaternion.identity);
        float oneToXScale = 1f;
        float newDistance = Vector3.Distance(_objAttachmentPoint.transform.position, objPlayer.transform.position);
        Vector3 newScale = new Vector3(newDistance * (1f / oneToXScale), _attachmentDistance / newDistance, 1f);
        _objAttachmentLine.transform.localScale = newScale;
    }

    public void disonnectRope()
    {
        Destroy(objPlayer.GetComponent<DistanceJoint2D>());
        if (_objAttachmentLine)
            Destroy(_objAttachmentLine);
        if (_objAttachmentPoint)
            Destroy(_objAttachmentPoint);
    }

    public void addScore(int amount)
    {
        _playerStats.addScore(amount);
        updateScoreUI();
    }

    private void updateScoreUI()
    {
        _txtScore.text = _playerStats._score.ToString("00000");
    }

    public void gameOver(bool win = true)
    {
        _gameActive = false;

        updateGameOverUI(win);
        showGameOverUI();

        Time.timeScale = 0;
    }

    private void updateGameOverUI(bool win)
    {
        _txtWinLose.text = (win == true) ? ("Win!") : ("Lose!");
        _txtFinalScore.text = _playerStats._score.ToString("00000");
        _txtTotalTime.text = _timeElapsed.ToString("0.00") + " Seconds";
    }

    private void showGameOverUI()
    {
        objGameOverUI.SetActive(true);
        objGameUI.SetActive(false);
    }

    public void endGame()
    {
        Application.LoadLevel("Start");
    }

    public bool isGameActive()
    {
        return _gameActive;
    }
}
