using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnitController;
using static UnityEngine.GraphicsBuffer;
using PlayerController;
using StateMachine = PlayerController.StateMachine;
using System.Linq;
using Unity.Burst.CompilerServices;

public class UnitController : MonoBehaviour
{
    [SerializeField] private bool isPlayer = false;
    // Start is called before the first frame update
    //[SerializeField] private GameObject cameraPrefeb;
    [SerializeField] private Transform cameraArm;
    [SerializeField] private Transform unitCamera;
    [SerializeField] private Katana kanata;
    [SerializeField] private Smash smash;
     
    [SerializeField] private Quaternion targetRotation;
    [SerializeField] private Vector2 keyDelta = Vector2.zero;
    [SerializeField] private Vector3 inputDir;
    [SerializeField] private float rotationSpeed = 150f;

    [SerializeField] float targetMoveSpeed = 5;

    [SerializeField] private Vector2 turnAngle = new Vector2(10, 30);
    [SerializeField] private Vector3 actionZoom = Vector3.zero;
    [SerializeField] private float distance = -5f;
    
    private BoxCollider weaponTrigger;
    private BoxCollider smashTrigger;

    public Animator animator;
    public CharacterController characterController;
    public StateMachine stateMachine;

    private float moveSpeed = 0f;
    private string currentAnimation = "";
    bool isAnimationFinished = false;
    bool isCombo = false;

    public float smashSpeed = 30;
    float verticalSpeed = -10f;
    RaycastHit hit;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.updateMode = AnimatorUpdateMode.Normal;

        if(isPlayer)
        {
            characterController = GetComponent<CharacterController>();
            weaponTrigger = kanata.GetComponent<BoxCollider>();
            smashTrigger = smash.GetComponent<BoxCollider>();
            InitCamera();
        }
        
        
        InitStateMachine();
    }
    void Start()
    {
        if (isPlayer)
        {
            transform.position = DungeonGenerator.Instance.StartPos;
            weaponTrigger.enabled = false;
            smashTrigger.enabled = false;
            ChangeAnimation("Unequip");
        }
        
    }

    private void FixedUpdate()
    {
        stateMachine?.OnFixedUpdateState();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {    
        if (hit.rigidbody)
        {
            SetVelocity(hit);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayer)
        {
            if (Input.GetKeyDown("x"))
            {
                bool isEquip = animator.GetBool("Equip");

                animator.SetBool("IsFinished", false);
                animator.SetBool("Equip", !isEquip);
            }

            CheckInputDir();

            SetGravity();

        }

        stateMachine?.OnUpdateState();
    }
    public bool CheckAnimation()
    {
        if (isAnimationFinished)
        {
            moveSpeed = 0;
            isAnimationFinished = false;
            return true;
        }

        return false;
    }

    public bool CheckComboAnimation()
    {
        if (isCombo)
        {
            moveSpeed = 0;
            isCombo = false;
            return true;
        }

        return false;
    }


    public void ChangeAnimation(string animation, float crossfade = 0.2f, float animationSpeed = 1f)
    {
        if(currentAnimation != animation)
        {
            currentAnimation = animation;
            animator.CrossFade(animation, crossfade);
            animator.speed = animationSpeed;
        }
    }

    private void LateUpdate()
    {
        if(isPlayer)
            LookAround();
        
        stateMachine?.OnLateUpdateState();
    }

    private void SetGravity()
    {
        //Debug.DrawRay(transform.position, transform.up * -1f, Color.cyan);

        if(!Physics.Raycast(transform.position, transform.up * -1f, out hit, 0.01f))
            characterController.Move(Vector3.up * verticalSpeed * Time.deltaTime);
    }
    public bool IsMove()
    {
        if (0.01f < inputDir.magnitude)
            return true;

        return false;
    }

    public bool IsSmash()
    {
        if (Input.GetKeyDown("d"))
            return true;

        return false;
    }

    public bool IsComboAttack()
    {
        if (Input.GetKeyDown("s"))
            return true;

        return false;
    }

    public bool IsGuard()
    {
        if (Input.GetKey("a") && (0 >= inputDir.magnitude))
            return true;

        return false;
    }

    public bool IsEvade()
    {
        if (Input.GetKey("a") && (0 < inputDir.magnitude))
            return true;

        return false;
    }

    public void SetEquip(bool isEquip, float crossfade = 0.2f)
    {
        if(isEquip)
        {
            ChangeAnimation("Equip", crossfade);     
        }
        else
        {
            ChangeAnimation("Unequip", crossfade);
        }

        animator.SetBool("Equip", isEquip);
    }

    public void Move()
    {
        float lerpSpeed = 10f;
        float moveSync = 5f;

        moveSpeed = Mathf.Lerp(moveSpeed, targetMoveSpeed, Time.deltaTime * lerpSpeed);

        characterController.Move(inputDir * Time.deltaTime * moveSpeed * moveSync);

        animator.SetFloat("MoveSpeed", moveSpeed);
        animator.SetFloat("AnimSpeed", targetMoveSpeed);
    }

    public void Idle()
    {
        float lerpSpeed = 10f;
        float moveSync = 5f;

        moveSpeed = Mathf.Lerp(moveSpeed, 0f, Time.deltaTime * lerpSpeed);

        characterController.Move(transform.forward * Time.deltaTime * moveSpeed * moveSync);

        animator.SetFloat("MoveSpeed", moveSpeed);
        animator.SetFloat("AnimSpeed", 1f);
    }

    public void CheckInputDir()
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
    }


    public void LookDiraction() //카메라 방향이 정면으로 되는 움직임 구현
    {

        if (0f < inputDir.magnitude)
        {
            Quaternion moveRotation = Quaternion.LookRotation(inputDir);

            transform.rotation = Quaternion.Slerp(transform.rotation, moveRotation, 20f * Time.deltaTime);
        }

    }

    public void LookForward()
    {
        Quaternion moveRotation;

        if (0f < inputDir.magnitude)
        {
            moveRotation = Quaternion.LookRotation(inputDir);   
        }
        else
        {
            Vector3 cameraForward = unitCamera.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            moveRotation = Quaternion.LookRotation(cameraForward);
        }

        transform.rotation = moveRotation;
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

    private void InitStateMachine()
    {
        if (isPlayer)
        {
            stateMachine = new StateMachine(UnitState.IDLE, new IdleState(this));
            stateMachine.AddState(UnitState.MOVE, new MoveState(this));

            stateMachine.AddState(UnitState.COMBO_01, new Combo_01_State(this));
            stateMachine.AddState(UnitState.COMBO_02, new Combo_02_State(this));
            stateMachine.AddState(UnitState.COMBO_03, new Combo_03_State(this));

            stateMachine.AddState(UnitState.SMASH_00, new Smash_00_State(this));
            stateMachine.AddState(UnitState.SMASH_01, new Smash_01_State(this));

            stateMachine.AddState(UnitState.GUARD_01, new GuardState(this));

            stateMachine.AddState(UnitState.EVADE, new EvadeState(this));
        }
        else
        {

        }

    }

    private void InitCamera()
    {
        CameraManager.Instance.CamRegister("PlayerCamera", this.gameObject);    
    }

    public void StickCamera(GameObject gameObject)
    {
        cameraArm = gameObject.transform;
        unitCamera = cameraArm.GetChild(0);
    }
    private void SetVelocity(ControllerColliderHit hit)
    {
        Vector3 hitPoint = new Vector3(hit.point.x, 0, hit.point.z);
        Vector3 hitRigidBodyPosition = new Vector3(hit.rigidbody.position.x, 0, hit.rigidbody.position.z);
        Vector3 forceDir = (hitRigidBodyPosition - hitPoint);
        float distance = forceDir.magnitude;
        forceDir = forceDir.normalized;

        float mass = hit.rigidbody.mass;
        Vector3 torque = Vector3.Cross(inputDir, forceDir) * distance;

        hit.rigidbody.velocity = forceDir * targetMoveSpeed / mass;
        hit.rigidbody.angularVelocity = torque * targetMoveSpeed / mass;
    }

    private void IsWeaponEquipFinished()
    {
        animator.SetBool("IsFinished", true);
    }

    private void IsAnimationFinished()
    {
        isAnimationFinished = true;
    }

    private void IsCombo()
    {
        weaponTrigger.enabled = false;
        isCombo = true;
    }

    public void EnableWeaponTrigger()
    {
        weaponTrigger.enabled = true;
    }
    public void DisableWeaponTrigger()
    {
        weaponTrigger.enabled = false;
    }

    public void EnableSmashTrigger()
    {
        smashTrigger.enabled = true;
    }
    public void DisableSmashTrigger()
    {
        smashTrigger.enabled = false;
    }

    public void SetWeaponDamage(int damage)
    {
        if (damage <= 1)
            kanata.Damage = 1;
        else
            kanata.Damage *= damage;
    }

    public void Smash()
    {
        smashSpeed = 0;
        animator.speed = 1f;
    }
    public bool IsSmashMoveStart()
    {
        return smashTrigger.enabled;
    }

    public void SmashMove()
    {
        characterController.Move(transform.forward * Time.deltaTime * smashSpeed);
    }
    private void CreateBrush()
    {
        EffectManager.Instance.GetEffect(1, transform.position, Quaternion.identity, 2f);
    }

    
    //IEnumerator CoDelay()
    //{
    //    while(true)
    //    {
    //        yield return YieldCache.WaitForSecondsRealTime(3);
    //        Debug.Log("3sec");
    //    }
    //}

    private void ActionZoom()
    {

        //if (activeZoom)
        //    unitCamera.localPosition = Vector3.Slerp(unitCamera.localPosition, new Vector3(0, 1, distance + 2f), Time.deltaTime * 50f);
        //else
        //    
    }

}
