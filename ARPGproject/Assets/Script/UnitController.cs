using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Transform unitBody;
    [SerializeField]
    private Transform unitCamera;
    [SerializeField]
    private Transform cameraArm;

    [SerializeField]
    private Quaternion targetRotation;

    [SerializeField]
    private Vector2 keyDelta = Vector2.zero;

    [SerializeField]
    private float rotationSpeed = 150;
    [SerializeField]
    private Vector2 turnAngle = new Vector2(10, 30);
    [SerializeField] 
    private float Distance = -4;

    Animator animator;
    Rigidbody unitRigidbody;

    //Animator animator;
    void Start()
    {
        animator = unitBody.GetComponent<Animator>();
        unitRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        LookAround();
        Move();
    }
    private void Move()
    {



        bool isMove = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || 
            Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow);

        float hAxis = 0f;
        float vAxis = 0f;

        if (Input.GetKey(KeyCode.UpArrow)) { vAxis = 1f; }

        if (Input.GetKey(KeyCode.DownArrow)) { vAxis = -1f; }

        if (Input.GetKey(KeyCode.RightArrow)) { hAxis = 1f; }

        if (Input.GetKey(KeyCode.LeftArrow)) { hAxis = -1f; }


        animator.SetBool("isMove", isMove);


        Vector3 cameraForward = unitCamera.forward; // ?????? ???? ????
        Vector3 cameraRight = unitCamera.right;     // ?????? ?????? ????

        Vector3 inputDir = cameraForward * vAxis + cameraRight * hAxis;

        inputDir.y = 0f;
        inputDir = inputDir.normalized;

        // ĳ????? ?????? ?????? ??????? ????????.
        if (inputDir != Vector3.zero)
        {
            Quaternion moveRotation = Quaternion.LookRotation(inputDir);

            // ?ε??? ?????? ????????.
            unitBody.rotation = Quaternion.Slerp(unitBody.rotation, moveRotation, 20f * Time.deltaTime);
        }

        unitRigidbody.velocity = inputDir * 5;
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

        targetRotation = Quaternion.Euler(keyDelta.y, keyDelta.x, 0);
        cameraArm.rotation = Quaternion.Euler(camAngle.x, camAngle.y, 0); //z축 회전 고정

        // 시간을 사용하여 회전 보간
        cameraArm.rotation = Quaternion.Slerp(cameraArm.rotation, targetRotation, Time.deltaTime * 5f);

        Distance += Input.GetAxis("Mouse ScrollWheel") * 5;
        unitCamera.localPosition = new Vector3(0, 0, Distance);

    }

}
