using Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : UnitController
{
    [SerializeField] private bool isPlayer = false;
    [SerializeField] private GameObject cameraArm;
    [SerializeField] private Transform unitCamera;
    [SerializeField] private Katana kanata;
    [SerializeField] private Smash smash;
    [SerializeField] private Quaternion targetRotation;
    [SerializeField] private Vector2 keyDelta = Vector2.zero;
    [SerializeField] private Vector3 inputDir;
    public Vector3 InputDir => inputDir;
    [SerializeField] private float rotationSpeed = 150f;
    [SerializeField] private float targetMoveSpeed = 5;
    [SerializeField] private Vector2 turnAngle = new Vector2(10, 30);
    [SerializeField] private Vector3 actionZoom = Vector3.zero;
    [SerializeField] private float distance = -5f;

    bool isCombo = false;
    bool isSmash = false;
    protected override void Awake()
    {
        base.Awake();

        weaponTrigger = kanata.GetComponent<BoxCollider>();
        smashTrigger = smash.GetComponent<BoxCollider>();

        InitStateMachine();
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        transform.position = DungeonGenerator.Instance.StartPos;
    }

    protected override void OnEnable()
    {
        InitCamera();
        ChangeAnimation("Unequip");
        smashTrigger.enabled = false;
        weaponTrigger.enabled = false;
        isCounter = 0;

        base.OnEnable();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            inputBuffer.Add(new KeyValuePair<string, float>("a", Time.time));
        }
        inputBuffer.RemoveAll(input => Time.time - input.Value > bufferTime);

        if (Input.GetKeyDown("x"))
        {
            bool isEquip = animator.GetBool("Equip");

            animator.SetBool("IsFinished", false);
            animator.SetBool("Equip", !isEquip);
        }

        CheckInputDir();

        base.Update();
    }

    protected override void LateUpdate()
    {
        LookAround();

        base.LateUpdate();
    }
    private void OnTriggerEnter(Collider other)
    {
        DamageState.HitChangeState(other, UnitInfo, transform, stateMachine);
    }

    private void InitCamera()
    {
        cameraArm = Instantiate(cameraArm);
        unitCamera = cameraArm.transform.GetChild(0);
        CameraManager.Instance.CamRegister("PlayerCamera", cameraArm);
    }

    private void InitStateMachine()
    {
        stateMachine = new StateMachine(UnitState.IDLE, new IdleState(this));
        stateMachine.AddState(UnitState.MOVE, new MoveState(this));

        stateMachine.AddState(UnitState.COMBO_01, new Combo_01_State(this));
        stateMachine.AddState(UnitState.COMBO_02, new Combo_02_State(this));
        stateMachine.AddState(UnitState.COMBO_03, new Combo_03_State(this));

        stateMachine.AddState(UnitState.SMASH_00, new Smash_00_State(this));
        stateMachine.AddState(UnitState.SMASH_01, new Smash_01_State(this));

        stateMachine.AddState(UnitState.GUARD_01, new GuardState(this));
        stateMachine.AddState(UnitState.GUARD_02, new GuardHitState(this));

        stateMachine.AddState(UnitState.HIT, new HitState(this));

        stateMachine.AddState(UnitState.EVADE, new EvadeState(this));
    }

    private void CheckInputDir()
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

    private void LookAround()
    {
        Vector3 camAngle = cameraArm.transform.rotation.eulerAngles;

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
        cameraArm.transform.rotation = Quaternion.Euler(camAngle.x, camAngle.y, 0); //z축 회전 고정
        // 시간을 사용하여 회전 보간
        cameraArm.transform.rotation = Quaternion.Slerp(cameraArm.transform.rotation, targetRotation, Time.deltaTime * 10f);

        // 스무딩 카메라
        //Vector3 targetPos = new Vector3(transform.position.x, transform.position.y + cameraArm.position.y, transform.position.z);
        cameraArm.transform.position = Vector3.Lerp(cameraArm.transform.position, transform.position, Time.deltaTime * 5f);
        cameraArm.transform.position = new Vector3(cameraArm.transform.position.x, transform.position.y, cameraArm.transform.position.z);

        //휠로 캐릭터 줌
        distance += Input.GetAxis("Mouse ScrollWheel") * 5;

        unitCamera.localPosition = Vector3.Lerp(unitCamera.localPosition, new Vector3(0, 2, distance), Time.deltaTime * 30f);
    }
    public bool IsMove()
    {
        if (0f < inputDir.magnitude)
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

    public bool IsBufferedInput(string key)
    {
        // Check if the key is in the buffer
        return inputBuffer.Exists(input => input.Key == key);
    }

    public bool IsGuard()
    {
        if (IsBufferedInput("a") && (0 >= inputDir.magnitude) && !isGuardCooldown)
            return true;

        return false;
    }


    protected bool isGuardCooldown = false;

    public void StartGuardCooldown()
    {
        isGuardCooldown = true;
        StartCoroutine(GuardCooldown());
    }

    protected IEnumerator GuardCooldown()
    {
        yield return YieldCache.WaitForSeconds(0.3f);
        isGuardCooldown = false;
    }

    //public bool EmergencyGuard()
    //{
    //    if (Input.GetKeyDown(KeyCode.Tab) && (0 >= inputDir.magnitude))
    //        return true;

    //    return false;
    //}

    public bool IsEvade()
    {
        if (IsBufferedInput("a") && (0 < inputDir.magnitude))
            return true;

        return false;
    }

    public void SetEquip(bool isEquip, float crossfade = 0.2f)
    {
        if (isEquip)
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
    public void LookDiraction() //카메라 방향이 정면으로 되는 움직임 구현
    {

        if (0f < inputDir.magnitude) //방향키 입력이 되었을때
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
            Transform nearUnitTransform = unitChecker.InNearUnitTransform();
            if (nearUnitTransform != null)
            {
                Vector3 directionToNearUnit = nearUnitTransform.position - transform.position;
                directionToNearUnit.y = 0; // y 축 방향을 무시하고 수평 방향으로만 향하도록 설정
                moveRotation = Quaternion.LookRotation(directionToNearUnit);
            }
            else
            {
                Vector3 cameraForward = unitCamera.forward;
                cameraForward.y = 0;
                cameraForward.Normalize();
                moveRotation = Quaternion.LookRotation(cameraForward);
            }
        }

        transform.rotation = moveRotation;
    }

    protected void IsCombo()
    {
        weaponTrigger.enabled = false;
        isCombo = true;
    }

    protected void IsWeaponEquipFinished()
    {
        animator.SetBool("IsFinished", true);
    }

    public void EnableSmashTrigger()
    {
        smashTrigger.enabled = true;
    }
    public void DisableSmashTrigger()
    {
        smashTrigger.enabled = false;
    }

    public void ExitComnbo()
    {
        isCombo = false;
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
    public bool CheckSmashAnimation()
    {
        if (isSmash)
        {
            moveSpeed = 0;
            isSmash = false;
            return true;
        }

        return false;
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
    
}
