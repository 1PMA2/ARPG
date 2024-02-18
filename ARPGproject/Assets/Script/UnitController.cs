using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class UnitController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject cameraPrefeb;
    [SerializeField]
    private Transform cameraArm;
    [SerializeField]
    private Transform unitCamera;

    [SerializeField]
    private Quaternion targetRotation;

    [SerializeField]
    private Vector2 keyDelta = Vector2.zero;

    [SerializeField]
    private float rotationSpeed = 150f;
    [SerializeField]
    private Vector2 turnAngle = new Vector2(10, 30);
    [SerializeField] 
    private float Distance = -4f;

    [SerializeField]
    private float jumpPower = 10f;

    [SerializeField]
    UnitState state;

    private Animator animator;
    private CharacterController characterController;

    [SerializeField]
    Vector3 inputDir;

    public enum UnitState
    {
        IDLE,
        MOVE,
        COMBAT_IDLE,
        ATTACK,
        SMASH_START,
        SMASH_CHARGE,
        SMASH,
        END
    }

    //Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        cameraArm = Instantiate(cameraPrefeb).transform;
        unitCamera = cameraArm.GetChild(0);

        Application.targetFrameRate = 144;
    }

    private void FixedUpdate()
    {
       
        //unitRigidbody.MovePosition(transform.position + inputDir * Time.deltaTime * 5f);
        //unitRigidbody.AddForce(inputDir * 100f);
    }

    // Update is called once per frame
    void Update()
    {
        state = (UnitState)animator.GetInteger("State");

        switch (animator.GetInteger("State"))
        {
            case (int)UnitState.MOVE:
                Move();
                LookDiraction();
                break;
            case (int)UnitState.SMASH:
                break;
            default:
                LookDiraction();
                break;
        }

    }

    private void LateUpdate()
    {
        LookAround();
    }
    private void Move()
    {

        characterController.Move(inputDir * Time.deltaTime * 5);
    }


    private void LookDiraction() //ī�޶� ������ �������� �Ǵ� ������ ����
    {
       

        float hAxis = 0f;
        float vAxis = 0f;

        if (Input.GetKey(KeyCode.UpArrow)) { vAxis = 1f; }

        if (Input.GetKey(KeyCode.DownArrow)) { vAxis = -1f; }

        if (Input.GetKey(KeyCode.RightArrow)) { hAxis = 1f; }

        if (Input.GetKey(KeyCode.LeftArrow)) { hAxis = -1f; }

        Vector3 cameraForward = unitCamera.forward;
        Vector3 cameraRight = unitCamera.right;

        inputDir = cameraForward * vAxis + cameraRight * hAxis;

        inputDir.y = 0;
        inputDir = inputDir.normalized;

        if (inputDir != Vector3.zero)
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
        cameraArm.rotation = Quaternion.Euler(camAngle.x, camAngle.y, 0); //z�� ȸ�� ����
        // �ð��� ����Ͽ� ȸ�� ����
        cameraArm.rotation = Quaternion.Slerp(cameraArm.rotation, targetRotation, Time.deltaTime * 10f);

        // ������ ī�޶�
        //Vector3 targetPos = new Vector3(transform.position.x, transform.position.y + cameraArm.position.y, transform.position.z);
        cameraArm.position = Vector3.Lerp(cameraArm.position, transform.position, Time.deltaTime * 5f);
        cameraArm.position = new Vector3(cameraArm.position.x, transform.position.y, cameraArm.position.z);

        //�ٷ� ĳ���� ��
        Distance += Input.GetAxis("Mouse ScrollWheel") * 5;
        unitCamera.localPosition = new Vector3(0, 2, Distance);

    }

}
