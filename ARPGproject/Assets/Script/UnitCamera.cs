using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class UnitCamera : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Transform target;

    [SerializeField] private float Distance = 5;

    [SerializeField] private float xmove = 0;
    [SerializeField] private float ymove = 0;

    [SerializeField] Quaternion targetRotation;

    [SerializeField] float CameraSpeed = 10;
    [SerializeField] float XSpeed = 30;
    [SerializeField] float YSpeed = 15;

    float TargetY = -1f;

    void Start()
    {
        transform.LookAt(target.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {

        if (Input.GetMouseButton(1))
        {
            xmove += Input.GetAxis("Mouse X") * 2.5f;
            ymove -= Input.GetAxis("Mouse Y") * 2.2f;

            transform.rotation = Quaternion.Euler(ymove, xmove, 0);

            targetRotation = transform.rotation;
        }


        if (Input.GetKey("q"))
        {
            xmove -= XSpeed;        
        }
        else if (Input.GetKey("e"))
        {
            xmove += XSpeed;
        }

        if (Input.GetKey("r"))
        {
            ymove -= YSpeed;
        }
        else if (Input.GetKey("f"))
        {
            ymove += YSpeed;
        }

        if(Input.GetKey("v"))
        {
            targetRotation = target.rotation;
        }

        targetRotation = Quaternion.Euler(ymove, xmove, 0);

        float t = Mathf.Clamp(Time.deltaTime * CameraSpeed, 0f, 0.99f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, t);


        Distance -= Input.GetAxis("Mouse ScrollWheel") * 5;

        if (Distance > 10f)
        {
            Distance = 10f;
        }
        if (Distance < 2f)
        {
            TargetY = Mathf.Lerp(TargetY, -1.7f, Time.deltaTime * 5); 
        }
        else
            TargetY = Mathf.Lerp(TargetY, -1.5f, Time.deltaTime * 5);

        transform.position = target.position - transform.rotation * new Vector3(0, TargetY, Distance);


    }
}
