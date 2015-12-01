using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ControllerGame : MonoBehaviour
{
    public static ControllerGame instance;

    public static float MAX_ROPE_WIDTH = 3f;
    public static float MAX_TOUCH_TIME_FOR_PRESS = 0.2f;

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
    public float maxForce = 8f;
    public float forcePerPixel = 0.25f;
    public float playerGravity = 9.8f;
    public bool tiltGravity = true;

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
    private Vector2 _lastMousePosition;
    private float _touchTime = 0f;
    private bool _stationaryPress = false;
    private bool _justShot = false;

    void Awake()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Hookshot"), true);
    }

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

            bool newClick = false;

#if UNITY_ANDROID && !UNITY_EDITOR
            if(Input.touchCount > 0)
            {
                touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    //Debug.Log("Click");
                    _touchTime = 0f;
                    _stationaryPress = true;
                    if (_objAttachmentLine && _objAttachmentPoint)
                    {
                        //disonnectRope();
                    }
                    else
                    {
                        if(_objHookshot != null)
                        {
                            Destroy(_objHookshot);
                        }
                        _justShot = true;
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
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    if (_objAttachmentLine && _objAttachmentPoint && _touchTime <= MAX_TOUCH_TIME_FOR_PRESS && _stationaryPress == true && _justShot == false)
                    {
                        disonnectRope();
                    }
                    _justShot = false;
                }
#else
            if (Input.GetMouseButtonDown(0) == true)
            {
                newClick = true;
                //Debug.Log("Click");
                if (_objAttachmentLine && _objAttachmentPoint)
                {
                    disonnectRope();
                }
                else
                {
                    if (_objHookshot != null)
                    {
                        Destroy(_objHookshot);
                    }
                    _objHookshot = (GameObject)Instantiate(prefabHookshot, objPlayer.transform.position, Quaternion.identity);

                    Vector3 deltaPosition = (Vector3)touchPosition - objPlayer.transform.position;
                    deltaPosition.Normalize();
                    float rotZRaw = Mathf.Atan2(deltaPosition.y, deltaPosition.x);
                    float rotZ = rotZRaw * Mathf.Rad2Deg;
                    Debug.Log(rotZ.ToString());
                    _objHookshot.transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90);
                    _objHookshot.GetComponent<Rigidbody2D>().velocity = new Vector2(hookshotSpeed * Mathf.Cos(rotZRaw), hookshotSpeed * Mathf.Sin(rotZRaw));
                }
            }
#endif

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

                //Swipe Movement Controls.
                /*
#if UNITY_ANDROID && !UNITY_EDITOR
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    _stationaryPress = false;
                    addClockwiseForce(-Input.GetTouch(0).deltaPosition.x * forcePerPixel);  //Note this will only work for testing. smaller devices will need some consideration of the actual screen width.
                }
#else
                if (_lastMousePosition != (Vector2)Input.mousePosition)
                {
                    if (newClick == false)
                    {
                        Vector2 deltaMousePosition = (Vector2)Input.mousePosition - _lastMousePosition;
                        addClockwiseForce(-deltaMousePosition.x * forcePerPixel);
                    }
                    _lastMousePosition = Input.mousePosition;
                }
#endif
                */
                //addClockwiseForce(5f);
            }

            applyGravitationalForce();

            updateScoreUI();    //The fast to code way...
        }
    }

    private void applyGravitationalForce()
    {
        Vector2 accel = new Vector2(0f, -1f);
        if(tiltGravity == true)
        {
            accel = Input.acceleration;

            //Debug.Log("Acceleration Raw : " + accel);

            if (accel.x == 0 && accel.y == 0)
            {
                accel.y = -1;
            }

            accel.Normalize();

            //Debug.Log("Acceleration Normal : " + accel);
        }

        _rigidbodyPlayer.AddForce(playerGravity * accel);
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
        if(objPlayer.GetComponent<DistanceJoint2D>() != null)
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

    public void addClockwiseForce(float force)
    {
        if (_objAttachmentLine == null || _objAttachmentPoint == null)
        {
            return;
        }

        Vector2 vTemp = new Vector2();
        vTemp = (Vector3)_attachmentPoint - objPlayer.transform.position;
        vTemp.Normalize();

        /* long way for if i want to mess around with stuff.
        float cacheCos = 0;// Mathf.Cos(Mathf.Deg2Rad * 90f);   //The long way first. woo vector math
        float cacheSin = 1;// Mathf.Sin(Mathf.Deg2Rad * 90f);
        Debug.Log("cacheCos : " + cacheCos + " - cacheSin : " + cacheSin);
        Vector2 vForce = new Vector2(((vTemp.x * cacheCos) - (vTemp.y * cacheSin)), ((vTemp.x * cacheSin) - (vTemp.y * cacheCos)));
        */

        Vector2 vForce = new Vector2(-vTemp.y, vTemp.x);
        vForce *= force;

        _rigidbodyPlayer.AddForce(vForce);
    }

    public void applyForceToPlayer(Vector2 force)
    {
        _rigidbodyPlayer.AddForce(force);
    }

    public Vector3 getPlayerPosition()
    {
        return objPlayer.transform.localPosition;
    }

    public void removeHookshot()
    {
        if (_objHookshot != null)
        {
            Destroy(_objHookshot);
        }
    }

    public void movePlayerToGameObject(GameObject target)
    {
        objPlayer.transform.localPosition = target.transform.localPosition;
    }
}
