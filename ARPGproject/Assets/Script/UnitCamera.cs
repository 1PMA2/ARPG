using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitCamera : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Transform target;

    [SerializeField] private float Distance = 5;

    [SerializeField] private float xmove = 0;
    [SerializeField] private float ymove = 0;

    [SerializeField] Quaternion targetRotation;
    [SerializeField] float rotationSpeed = 0;

    bool isKeydown = false;

    void Start()
    {
        transform.LookAt(target.position);
        transform.parent = target;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {

        if (Input.GetMouseButton(1))
        {
            isKeydown = false;

            xmove += Input.GetAxis("Mouse X") * 2.57f;
            ymove -= Input.GetAxis("Mouse Y") * 2.25f;

            transform.rotation = Quaternion.Euler(ymove, xmove, 0);
        }


        if (Input.GetKey("q"))
        {
            isKeydown = true;
            xmove -= 10;
            targetRotation = Quaternion.Euler(ymove, xmove, 0);
        }
        else if (Input.GetKey("e"))
        {
            isKeydown = true;
            xmove += 10;
            targetRotation = Quaternion.Euler(ymove, xmove, 0);
        }

        if (Input.GetKey("r"))
        {
            isKeydown = true;
            targetRotation = Quaternion.Euler(ymove - 15f, xmove, 0);
        }
        else if (Input.GetKey("f"))
        {
            isKeydown = true;
            targetRotation = Quaternion.Euler(ymove + 15f, xmove, 0);
        }

        if (isKeydown)
        {
            xmove = transform.rotation.eulerAngles.y;
            ymove = transform.rotation.eulerAngles.x;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.2f);
        }


        Distance -= Input.GetAxis("Mouse ScrollWheel") * 10;
        if (Distance < 5f) Distance = 5f;
        if (Distance > 20f) Distance = 20f;

        transform.position = target.position - transform.rotation * new Vector3(0, 0, Distance);
    }
}
