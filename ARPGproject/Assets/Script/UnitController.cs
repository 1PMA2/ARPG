using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnitController;
using static UnityEngine.GraphicsBuffer;
using PlayerController;
using StateMachine = PlayerController.StateMachine;

public class UnitController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject cameraPrefeb;
    [SerializeField] private Transform cameraArm;
    [SerializeField] private Transform unitCamera;
     
    [SerializeField] private Quaternion targetRotation;
    [SerializeField] private Vector2 keyDelta = Vector2.zero;
    [SerializeField] private Vector3 inputDir;
    [SerializeField] private float rotationSpeed = 150f;
    private float moveSpeed = 0f;
    //private float animSpeed = 0f;
    [SerializeField] float targetMoveSpeed = 5;


    [SerializeField] private Vector2 turnAngle = new Vector2(10, 30);
    [SerializeField] private Vector3 actionZoom = Vector3.zero;

    [SerializeField] private float distance = -5f;

    //[SerializeField] private float jumpPower = 10f;

    private Animator animator;
    private CharacterController characterController;
    private StateMachine stateMachine;





    //Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        cameraArm = Instantiate(cameraPrefeb).transform;
        unitCamera = cameraArm.GetChild(0);

        UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false;
        //stateMachine = new StateMachine(UnitState.MOVE, new)
    }

    private void FixedUpdate()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        LookDiraction();
        Move();
    }

    private void LateUpdate()
    {
        LookAround();
    }
    private void Move()
    {
        float lerpSpeed = 10f;
        float moveSync = 5f;

        if(0f < inputDir.magnitude)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, targetMoveSpeed, Time.deltaTime * lerpSpeed);

            characterController.Move(inputDir * Time.deltaTime * moveSpeed * moveSync);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, 0f, Time.deltaTime * lerpSpeed);

            characterController.Move(transform.forward * Time.deltaTime * moveSpeed * moveSync);
        }

        float animSpeed = Mathf.Max(1f, moveSpeed);

        animator.SetFloat("MoveSpeed", moveSpeed);
        animator.SetFloat("AnimSpeed", animSpeed);
    }


    private void LookDiraction() //카메라 방향이 정면으로 되는 움직임 구현
    {
       

        float hAxis = 0f;
        float vAxis = 0f;

        if (Input.GetKey(KeyCode.UpArrow)) { vAxis = 1f; }

        if (Input.GetKey(KeyCode.DownArrow)) { vAxis = -1f; }

        if (Input.GetKey(KeyCode.RightArrow)) { hAxis = 1f; }

        if (Input.GetKey(KeyCode.LeftArrow)) { hAxis = -1f; }

        Vector3 cameraForward = unitCamera.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 cameraRight = unitCamera.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        inputDir = cameraForward * vAxis + cameraRight * hAxis;
        inputDir = inputDir.normalized;

        if (inputDir.magnitude != 0f)
        {
            Quaternion moveRotation = Quaternion.LookRotation(inputDir);

            transform.rotation = Quaternion.Slerp(transform.rotation, moveRotation, 20f * Time.deltaTime);
        }

    }

    private void LookAround()
    {
        Vector3 camAngle = cameraArm.rotation.eulerAngles;

        if (Input.GetKey("q"))
        {
            keyDelta.x -= rotationSpeed * Time.deltaTime;
        }
        else if (Input.GetKey("e"))
        {
            keyDelta.x += rotationSpeed * Time.deltaTime;
        }


        if (Input.GetKeyUp("q"))
        {
            keyDelta.x -= turnAngle.x;
        }
        else if (Input.GetKeyUp("e"))
        {
            keyDelta.x += turnAngle.x;
        }
        else if (Input.GetKeyDown("r"))
        {
            keyDelta.y -= turnAngle.y;
        }
        else if (Input.GetKeyDown("f"))
        {
            keyDelta.y += turnAngle.y;
        }

        keyDelta.x %= 360f;
        keyDelta.y %= 360f;

        targetRotation = Quaternion.Euler(keyDelta.y, keyDelta.x, 0);
        cameraArm.rotation = Quaternion.Euler(camAngle.x, camAngle.y, 0); //z축 회전 고정
        // 시간을 사용하여 회전 보간
        cameraArm.rotation = Quaternion.Slerp(cameraArm.rotation, targetRotation, Time.deltaTime * 10f);

        // 스무딩 카메라
        //Vector3 targetPos = new Vector3(transform.position.x, transform.position.y + cameraArm.position.y, transform.position.z);
        cameraArm.position = Vector3.Lerp(cameraArm.position, transform.position, Time.deltaTime * 5f);
        cameraArm.position = new Vector3(cameraArm.position.x, transform.position.y, cameraArm.position.z);

        //휠로 캐릭터 줌
        distance += Input.GetAxis("Mouse ScrollWheel") * 5;

        unitCamera.localPosition = Vector3.Lerp(unitCamera.localPosition, new Vector3(0, 2, distance), Time.deltaTime * 30f);
    }



    private void ActionZoom()
    {

        //if (activeZoom)
        //    unitCamera.localPosition = Vector3.Slerp(unitCamera.localPosition, new Vector3(0, 1, distance + 2f), Time.deltaTime * 50f);
        //else
        //    
    }

}
