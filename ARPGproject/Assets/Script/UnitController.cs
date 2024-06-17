using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnitController;
using static UnityEngine.GraphicsBuffer;
using Controller;
using StateMachine = Controller.StateMachine;
using System.Linq;
using Unity.Burst.CompilerServices;


public class UnitController : MonoBehaviour
{
    [SerializeField] protected BoxCollider weaponTrigger;
    protected BoxCollider smashTrigger;
    public Animator animator;
    public CharacterController characterController;
    public StateMachine stateMachine;
    protected UnitChecker unitChecker;
    public UnitChecker UnitChecker
    {
        get { return unitChecker; }
        set { unitChecker = value; }
    }
    public Transform nearUnitTransform;

    protected UnitInformation unitInfo;
    public UnitInformation UnitInfo
    {
        get { return unitInfo; }
        set { unitInfo = value; }
    }

    protected TestBox statController;
    public TestBox StatController
    {
        get { return statController; }
        set { statController = value; }
    }

    protected float moveSpeed = 0f;
    protected string currentAnimation = "";
    public bool isAnimationFinished = false;
    protected int isCounter;
    public int IsCounter
    {
        get { return isCounter; }
        set { isCounter = value; }
    }
    public float smashSpeed = 30;
    float verticalSpeed = -10f;
    RaycastHit hit;

    protected List<KeyValuePair<string, float>> inputBuffer = new List<KeyValuePair<string, float>>();
    protected float bufferTime = 0.2f; // Time in seconds to buffer input

    protected virtual void Awake()
    { 
        animator = GetComponent<Animator>();
        animator.updateMode = AnimatorUpdateMode.Normal;
        characterController = GetComponent<CharacterController>();
        unitChecker = GetComponent<UnitChecker>();
        unitInfo = GetComponent<UnitInformation>();
        statController = GetComponent<TestBox>();
    }
    protected virtual void Start()
    {
        
    }
    //protected void OnControllerColliderHit(ControllerColliderHit hit)
    //{    
    //    //if (hit.rigidbody)
    //    //{
    //    //    SetVelocity(hit);
    //    //}
    //}

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {

    }
    protected virtual void FixedUpdate()
    {
        stateMachine?.OnFixedUpdateState();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        SetGravity();

        nearUnitTransform = unitChecker.InNearUnitTransform();

        stateMachine?.OnUpdateState();
    }

    protected virtual void LateUpdate()
    {   
        stateMachine?.OnLateUpdateState();
    }

    public void ChangeAnimation(string animation, float crossfade = 0.2f, float animationSpeed = 1f)
    {
        if (currentAnimation != animation)
        {
            currentAnimation = animation;

            animator.CrossFade(animation, crossfade);

            animator.speed = animationSpeed;
        }
    }

    protected void SetGravity()
    {
        //Debug.DrawRay(transform.position, transform.up * -1f, Color.cyan);
        if(!Physics.Raycast(transform.position, transform.up * -1f, out hit, 0.01f))
            characterController.Move(Vector3.up * verticalSpeed * Time.deltaTime);
    }

    //protected void SetVelocity(ControllerColliderHit hit)
    //{
    //    Vector3 hitPoint = new Vector3(hit.point.x, 0, hit.point.z);
    //    Vector3 hitRigidBodyPosition = new Vector3(hit.rigidbody.position.x, 0, hit.rigidbody.position.z);
    //    Vector3 forceDir = (hitRigidBodyPosition - hitPoint);
    //    float distance = forceDir.magnitude;
    //    forceDir = forceDir.normalized;

    //    float mass = hit.rigidbody.mass;
    //    Vector3 torque = Vector3.Cross(inputDir, forceDir) * distance;

    //    hit.rigidbody.velocity = forceDir * targetMoveSpeed / mass;
    //    hit.rigidbody.angularVelocity = torque * targetMoveSpeed / mass;
    //}
    protected void IsAnimationFinished()
    {
        isAnimationFinished = true;
    }
    public void IsAnimationStart()
    {
        isAnimationFinished = false;
    }
    public void EnableWeaponTrigger()
    {
        weaponTrigger.enabled = true;
    }
    public void DisableWeaponTrigger()
    {
        weaponTrigger.enabled = false;
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
    protected void CreateBrush()
    {
        EffectManager.Instance.GetEffect(1, transform.position, Quaternion.identity, 2f);
    }

}
