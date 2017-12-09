using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PlayerMovement : MonoBehaviour
{
    Vector3 _mousePos;
    Vector3 newPos;
    Vector3 targetPos;
    float MovementSpeed;
    bool CoolDownBarContinues;
    IEnumerator _movementRoutine;
    IEnumerator _movePosCoroutine;
    IEnumerator _coolDownCoroutine;
    static Action OnEmptyDash;         // I need to call the cooldown routine in case the player dashes to empty space (couldn't hit any enemy)
    static Action OnMouseClick;

    public float CooldownDecreaseAmount;
    public bool HasClicked;

    public static PlayerMovement Instance
    {
        get; private set;
    }

    void Awake()
    {
        _mousePos = Input.mousePosition;
        Instance = this;
        MovementSpeed = 0.15f;
        CooldownDecreaseAmount = 0.011f;
        newPos = new Vector3();
    }

    void OnEnable()
    {
        OnEmptyDash += StartCoolDown;
        OnMouseClick += StartMoveToMouse;
        _movementRoutine = Move(CoolDownBarContinues);
        StartCoroutine(_movementRoutine);
    }
    
    void OnDisable()
    {
        StopCoroutine(_movementRoutine);
    }

    void Update()
    {
        _mousePos =  Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RotatePlayer();
    }

    void FireOnEmptyDash()
    {
        if (OnEmptyDash != null)
            OnEmptyDash();
    }

    void FireOnMouseClick()
    {
        if (OnMouseClick != null)
            OnMouseClick();
    }

    void StartCoolDown()
    {
        _coolDownCoroutine = StartCoolDownBar();
        StartCoroutine(_coolDownCoroutine);  // if the enemy has hit by the player
    }

    void StartMoveToMouse()
    {
        _movePosCoroutine = MoveToTheMousePos(targetPos, newPos);
        StartCoroutine(_movePosCoroutine);
    }

    void RotatePlayer()
    {
        float angle = Mathf.Atan2(_mousePos.x - transform.position.x, _mousePos.y - transform.position.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, 90-angle);
    }

    public void ResetCooldown()
    {
        if (_coolDownCoroutine != null)
            StopCoroutine(_coolDownCoroutine); 
        MakeCooldownDeactive();
    }
         
    IEnumerator StartCoolDownBar()
    {
        CoolDownBarContinues = true;
        Player.Instance.CoolDownCanvas.gameObject.SetActive(true);        // make cool down bar visible

        while (Player.Instance.CoolDownBar.fillAmount >= 0.1f)
        {
           Player.Instance.CoolDownBar.fillAmount -= CooldownDecreaseAmount;
           yield return null;
        }
        MakeCooldownDeactive();
    }

    void MakeCooldownDeactive()
    {
        Player.Instance.CoolDownCanvas.gameObject.SetActive(false);        // erase the cooldown bar from the scene
        Player.Instance.CoolDownBar.fillAmount = 1;
        CoolDownBarContinues = false;
    }

    IEnumerator Move(bool coolDownBarContinues)
    {
        while (Player.Instance.PlayersState != PlayerState.Dead)
        {
            _mousePos.z = transform.position.z;
            targetPos = _mousePos;
            MoveTowardsTheMouse(targetPos);

            if (Input.GetMouseButtonDown(0))                            // if cooldown bar has finished, then move to the clicked mouse pos     
            {
                if (!CoolDownBarContinues)
                {
                    if (_movePosCoroutine != null)
                        StopCoroutine(_movePosCoroutine);

                    FireOnEmptyDash();
                    HasClicked = true;

                    FireOnMouseClick();
                }
            }
            yield return null;
        }
    }

    IEnumerator MoveToTheMousePos(Vector3 targetPos, Vector3 newPos)
    {
        while (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            Vector3 _movementSpeedVector = Vector3.zero;
            newPos = Vector3.SmoothDamp(transform.position, targetPos, ref _movementSpeedVector, Time.deltaTime * 5);
            transform.position = new Vector3(newPos.x, newPos.y, targetPos.z);
            transform.position = Boundaries.Instance.GetBoundaryPosition(transform.position);
            yield return null;
        }
        MoveTowardsTheMouse(targetPos);
        HasClicked = false;
    }

    void MoveTowardsTheMouse(Vector3 posToGo)
    {
        // this movement towards to the mouse pos, always will be running whether the player hit the enemy or not
        // Player needs to Move to the direction of the mouse pos all the time
        transform.position = Vector3.MoveTowards(transform.position, posToGo, MovementSpeed);
        transform.position = Boundaries.Instance.GetBoundaryPosition(transform.position);
    }

    public void IncreaseSpeed(float increaseAmount)
    {
        MovementSpeed += increaseAmount;
    }
}
